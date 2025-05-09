using NTDLS.Helpers;
using NTDLS.Permafrost;
using SecureChat.Client.Audio;
using SecureChat.Client.Controls;
using SecureChat.Client.Forms;
using SecureChat.Client.Helpers;
using SecureChat.Library;
using SecureChat.Library.DatagramMessages;
using SecureChat.Library.ReliableMessages;
using Serilog;

namespace SecureChat.Client
{
    internal class ActiveChat
    {
        private readonly PermafrostCipher _streamCryptography;
        private AudioPump? _audioPump;
        private int _inputDeviceIndex;
        private int _outputDeviceIndex;
        private int _bitrate;

        /// <summary>
        /// When a voice call is sent, this will be the control that is displayed in the chat window.
        /// We save it so we can remove it when the call is accepted or canceled.
        /// </summary>
        public FlowControlOutgoingCall? LastOutgoingCallControl { get; set; }
        public PublicPrivateKeyPair PublicPrivateKeyPair { get; private set; }
        public bool IsTerminated { get; private set; } = false;

        /// <summary>
        /// The form that is used to display the chat messages.
        /// </summary>
        public FormMessage Form { get; private set; }

        /// <summary>
        /// The account id of the user we are chatting with. This is the account id of the contact.
        /// </summary>
        public Guid AccountId { get; private set; }

        /// <summary>
        /// Name of the contact we are chatting with. This is the display name of the contact.
        /// </summary>
        public string DisplayName { get; private set; }

        /// <summary>
        /// The connection id where the messages should be ultimately routed to.
        /// This is the connection id that the server has for the remote peer.
        /// </summary>
        public Guid PeerConnectionId { get; private set; }

        public Dictionary<Guid, FlowControlFileTransferReceiveProgress> InboundFileTransfers { get; set; } = new();
        public Dictionary<Guid, FlowControlFileTransferSendProgress> OutboundFileTransfers { get; set; } = new();
        public Dictionary<Guid, FlowControlFileTransferRequest> PendingFileTransfers { get; set; } = new();
        public DateTime? LastMessageReceived { get; set; }

        /// <summary>
        /// Identifies this chat session. This is used to identify the chat session when sending messages.
        /// If the session is ended and a new one is started, it will have a different SessionId - even if it is the same contact.
        /// </summary>
        public Guid SessionId { get; private set; }

        public ActiveChat(Guid sessionId, Guid peerConnectionId, Guid accountId, string displayName, byte[] sharedSecret)
        {
            if (ServerConnection.Current == null)
                throw new Exception("Local connection is not established.");

            //Obtain the public and private key-pair from the reliable connection so we can use it for the datagram messaging.
            var rmCryptographyProvider = ServerConnection.Current?.ReliableClient.GetCryptographyProvider() as ReliableCryptographyProvider
                ?? throw new Exception("Reliable cryptography has not been initialized.");

            SessionId = sessionId;
            _streamCryptography = new PermafrostCipher(sharedSecret, PermafrostMode.AutoReset);
            PublicPrivateKeyPair = rmCryptographyProvider.PublicPrivateKeyPair;
            PeerConnectionId = peerConnectionId;
            AccountId = accountId;
            DisplayName = displayName;

            Form = ServerConnection.Current?.FormHome.CreateMessageForm(this)
                ?? throw new Exception("Unable to create message form. Server connection is not established.");

            new Thread(() =>
            {
                while (!IsTerminated && ServerConnection.Current?.ReliableClient.IsConnected == true)
                {
                    try
                    {
                        ServerConnection.Current?.ReliableClient.Notify(new SessionKeepAliveNotification(SessionId));
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "Error sending session keep-alive notification.");
                    }

                    var breakTime = DateTime.UtcNow.AddSeconds(10);
                    while (!IsTerminated && ServerConnection.Current?.ReliableClient.IsConnected == true && DateTime.UtcNow < breakTime)
                    {
                        Thread.Sleep(500);
                    }
                }
            }).Start();
        }

        public void Terminate()
        {
            if (IsTerminated)
            {
                return;
            }
            IsTerminated = true;

            if (ServerConnection.Current?.ReliableClient?.IsConnected == true)
            {
                Exceptions.Ignore(() =>
                ServerConnection.Current.ReliableClient.Notify(new TerminateChatNotification(SessionId, PeerConnectionId)));
            }

            Exceptions.Ignore(() => AppendSystemMessageLine($"Conversation has ended."));
            Exceptions.Ignore(() => StopAudioPump());
        }

        public void ReceiveTextMessage(byte[] cipherText)
        {
            if (IsTerminated)
            {
                return;
            }

            AppendReceivedMessageLine(DisplayName, DecryptString(cipherText), true, Themes.FromRemoteColor);
        }

        public bool SendTextMessage(string plaintText)
        {
            if (IsTerminated)
            {
                return false;
            }

            var query = new ExchangeMessageTextQuery(SessionId, PeerConnectionId, EncryptString(plaintText));

            return ServerConnection.Current?.ReliableClient.Query(query)
                .ContinueWith(o => !o.IsFaulted && o.Result.IsSuccess).Result ?? false;
        }

        #region Voice Call.

        public void AlertOfIncomingCall()
        {
            if (IsTerminated)
            {
                return;
            }

            AppendIncomingCall(DisplayName, true, Color.Blue);
        }

        public void PlayAudioPacket(byte[] bytes)
        {
            _audioPump?.IngestFrame(Cipher(bytes));
        }

        public void StartAudioPump()
        {
            _audioPump = new AudioPump(_inputDeviceIndex, _outputDeviceIndex, _bitrate);

            _audioPump.OnFrameProduced += (byte[] bytes, int byteCount) =>
            {
                //Sends the recorded audio to the server, for dispatch to the correct client.
                ServerConnection.Current?.DatagramClient?.Dispatch(new VoicePacketDatagram(SessionId, PeerConnectionId, Cipher(bytes)));
            };

            _audioPump.StartCapture();
            _audioPump.StartPlayback();
        }

        public void StopAudioPump()
        {
            _audioPump?.Stop();
            _audioPump = null;
        }

        /// <summary>
        /// Let the remote client know that we are terminating the voice call.
        /// </summary>
        public void RequestTerminateVoiceCall()
        {
            try
            {
                if (_audioPump != null)
                {
                    ServerConnection.Current?.ReliableClient.Notify(new TerminateVoiceCallNotification(SessionId, PeerConnectionId));
                }
                TerminateVoiceCall();
            }
            catch (Exception ex)
            {
                AppendErrorLine(ex);
            }
        }

        /// <summary>
        /// Terminate the voice call. This is called when the remote client has terminated the voice call.
        /// </summary>
        public void TerminateVoiceCall()
        {
            try
            {
                if (_audioPump != null)
                {
                    _audioPump?.Stop();
                    _audioPump = null;
                    _inputDeviceIndex = 0;
                    _outputDeviceIndex = 0;
                    _bitrate = 0;
                }

                Form?.ToggleVoiceCallButtons(true);
                AppendSystemMessageLine($"Voice call ended.");
            }
            catch (Exception ex)
            {
                AppendErrorLine(ex);
            }
        }

        /// <summary>
        /// Client is sending another client a request for a voice call.
        /// </summary>
        public void RequestVoiceCall(int inputDeviceIndex, int outputDeviceIndex, int bitrate)
        {
            _inputDeviceIndex = inputDeviceIndex;
            _outputDeviceIndex = outputDeviceIndex;
            _bitrate = bitrate;

            ServerConnection.Current?.ReliableClient.Notify(new RequestVoiceCallNotification(SessionId, PeerConnectionId));
        }

        /// <summary>
        /// Original requesting client is canceling a voice call request.
        /// </summary>
        public void CancelVoiceCallRequest()
        {
            ServerConnection.Current?.ReliableClient.Notify(new CancelVoiceCallRequestNotification(SessionId, PeerConnectionId));
        }

        /// <summary>
        /// Client which received the request for a voice call is is accepting the request.
        /// </summary>
        public void AcceptVoiceCallRequest(int inputDeviceIndex, int outputDeviceIndex, int bitrate)
        {
            _inputDeviceIndex = inputDeviceIndex;
            _outputDeviceIndex = outputDeviceIndex;
            _bitrate = bitrate;

            ServerConnection.Current?.ReliableClient.Notify(new AcceptVoiceCallNotification(SessionId, PeerConnectionId));
            AppendSystemMessageLine("Voice call is now connected.");
        }

        /// <summary>
        /// Client which received the request for a voice call is is declining the request.
        /// </summary>
        public void DeclineVoiceCallRequest()
        {
            ServerConnection.Current?.ReliableClient.Notify(new DeclineVoiceCallNotification(SessionId, PeerConnectionId));
        }

        #endregion

        #region File Transfer.

        /// <summary>
        /// Remote client has sent a request for a file transfer.
        /// Show the user a message to accept or decline the file.
        /// </summary>
        public void ReceiveFileTransferRequestMessage(Guid fileId, string fileName, long fileSize, bool isImage)
        {
            if (IsTerminated)
            {
                return;
            }

            AppendFileTransferRequestMessage(DisplayName, fileId, fileName, fileSize, isImage, true, Themes.FromRemoteColor);
        }

        /// <summary>
        /// A file transfer was completed for what is presumably a non-image, show the user a link to the file.
        /// </summary>
        public void ReceiveFileMessage(string? saveAsFileName)
        {
            if (IsTerminated)
            {
                return;
            }

            AppendFolderLinkMessage(DisplayName, Path.GetFileName(saveAsFileName) ?? "Open File Location",
                 Path.GetDirectoryName(saveAsFileName) ?? Environment.GetEnvironmentVariable("SystemDrive") ?? string.Empty,
                 true, Themes.FromRemoteColor);
        }

        /// <summary>
        /// A file transfer was completed for an image, show it to the user.
        /// </summary>
        public void ReceiveImageMessage(byte[] imageBytes)
        {
            if (IsTerminated)
            {
                return;
            }

            AppendImageMessage(DisplayName, imageBytes, true, Themes.FromRemoteColor);
        }

        /// <summary>
        /// Tell the remote client that we are canceling the file transfer.
        /// </summary>
        public void CancelFileTransfer(Guid fileId)
        {
            ServerConnection.Current?.ReliableClient.Notify(new FileTransferCancelNotification(SessionId, PeerConnectionId, fileId));
        }

        /// <summary>
        /// Tell the remote client that we are accepting the file transfer.
        /// </summary>
        /// <param name="fileId"></param>
        public void AcceptFileTransfer(Guid fileId)
        {
            ServerConnection.Current?.ReliableClient.Notify(new FileTransferAcceptRequestNotification(SessionId, PeerConnectionId, fileId));
        }

        /// <summary>
        /// Tell the remote client that we are declining the file transfer.
        /// </summary>
        /// <param name="fileId"></param>
        public void DeclineFileTransfer(FlowControlFileTransferRequest ftc)
        {
            ServerConnection.Current?.ReliableClient.Notify(new FileTransferDeclineRequestNotification(SessionId, PeerConnectionId, ftc.FileId));
        }

        /// <summary>
        /// The remote client has accepted the file transfer.
        /// </summary>
        public void FileTransferAccepted(Guid fileId)
        {
            if (OutboundFileTransfers.TryGetValue(fileId, out var ftc))
            {
                Task.Run(() => TransmitFileChunks(ftc));
            }
            else
            {
                AppendErrorLine($"Accepted file transfer not found.");
                //Tell the remote client that we are canceling the file transfer.
                CancelFileTransfer(fileId);
            }
        }

        /// <summary>
        /// The remote client has declined the file transfer.
        /// </summary>
        public void FileTransferDeclined(Guid fileId)
        {
            if (OutboundFileTransfers.TryGetValue(fileId, out var ftc))
            {
                ftc.Remove();
                AppendSystemMessageLine($"File transfer '{Path.GetFileName(ftc.Transfer.FileName)}' declined.");
            }
            else
            {
                AppendErrorLine($"Accepted file transfer not found.");
            }
        }

        /// <summary>
        /// Transmits a file to the remote client. The file is read from the disk and sent in chunks.
        /// </summary>
        public void TransmitFileAsync(string fileName)
        {
            var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            TransmitFileAsync(fileName, (new FileInfo(fileName)).Length, fileStream);
        }

        /// <summary>
        /// Transmits a file to the remote client. The file is read from the byte-array and sent in chunks.
        /// </summary>
        public void TransmitFileAsync(string fileName, byte[] fileBytes)
        {
            TransmitFileAsync(fileName, fileBytes.LongLength, new MemoryStream(fileBytes));
        }

        private void TransmitFileAsync(string fileName, long fileSize, Stream stream)
        {
            var ftc = AppendFileTransferSendProgress(fileName, fileSize, stream);
            OutboundFileTransfers.Add(ftc.Transfer.FileId, ftc);

            if (ftc.Transfer.IsImage)
            {
                //if this is an image, then we just transfer it because we can store it in the remote clients window.
                Task.Run(() => TransmitFileChunks(ftc));
            }
            else
            {
                //If this is another typo of file, then we need to request the remote
                //  client to accept the file so they can select a location to save it.
                ServerConnection.Current?.ReliableClient.Notify(new FileTransferBeginRequestNotification(
                    SessionId, PeerConnectionId, ftc.Transfer.FileId, Path.GetFileName(ftc.Transfer.FileName), ftc.Transfer.FileSize, ftc.Transfer.IsImage));
            }
        }

        private void TransmitFileChunks(FlowControlFileTransferSendProgress ftc)
        {
            try
            {
                ServerConnection.Current?.ReliableClient.Query(new FileTransferBeginQuery(
                    SessionId, PeerConnectionId, ftc.Transfer.FileId, ftc.Transfer.FileName, ftc.Transfer.FileSize, ftc.Transfer.IsImage));

                double totalBytesSent = 0;

                var buffer = new byte[Settings.Instance.FileTransferChunkSize];
                int bytesRead;

                while (!ftc.IsCancelled && (bytesRead = ftc.Transfer.Stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    var chunkToSend = buffer;
                    if (bytesRead < buffer.Length) // Handle the last partial chunk
                    {
                        chunkToSend = new byte[bytesRead];
                        Array.Copy(buffer, chunkToSend, bytesRead);
                    }

                    totalBytesSent += bytesRead;
                    double completionPercentage = (totalBytesSent / ftc.Transfer.FileSize) * 100.0;
                    ftc.SetProgressValue((int)completionPercentage);

                    // Transmit the current chunk
                    ServerConnection.Current?.ReliableClient.Query(new FileTransferChunkQuery(SessionId, PeerConnectionId, ftc.Transfer.FileId, Cipher(chunkToSend)));
                }

                if (!ftc.IsCancelled)
                {
                    ServerConnection.Current?.ReliableClient.Query(new FileTransferCompleteQuery(SessionId, PeerConnectionId, ftc.Transfer.FileId)).ContinueWith(o =>
                    {
                        if (!o.IsFaulted && o.Result.IsSuccess)
                        {
                            if (ftc.Transfer.IsImage)
                            {
                                // Load the image only after successful transfer
                                var imageData = File.ReadAllBytes(ftc.Transfer.FileName);
                                AppendImageMessage(ServerConnection.Current.DisplayName, imageData, false, Themes.FromMeColor);
                            }
                            else
                            {
                                AppendSuccessMessageLine($"File '{Path.GetFileName(ftc.Transfer.FileName)}' transferred successfully.");
                            }
                        }
                        else
                        {
                            AppendErrorLine($"Failed to transfer file '{Path.GetFileName(ftc.Transfer.FileName)}'.");
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                if (ftc.IsCancelled == false)
                {
                    AppendErrorLine($"Error transferring file: {ex.Message}", Color.Red);
                }
            }
            finally
            {
                ftc.Remove();
                OutboundFileTransfers.Remove(ftc.Transfer.FileId);
            }
        }

        #endregion

        #region Append Flow Controls.

        private void AppendFlowControl(Control control)
        {
            try
            {
                if (Form == null || Form.FlowPanel == null)
                {
                    return;
                }

                Form.Invoke(() =>
                {
                    lock (Form.FlowPanel)
                    {
                        Form.FlowPanel.Controls.Add(control);
                        while (Form.FlowPanel.Controls.Count > Settings.Instance.MaxMessages)
                        {
                            Form.FlowPanel.Controls.RemoveAt(0);
                        }
                        Form.FlowPanel.ScrollControlIntoView(control);
                    }
                });
            }
            catch (Exception ex)
            {
                AppendErrorLine(ex);
            }
        }

        private void AppendFileTransferRequestMessage(string fromName, Guid fileId,
            string fileName, long fileSize, bool isImage, bool playNotifications, Color color)
        {
            try
            {
                if (Form == null || Form.FlowPanel == null)
                {
                    return;
                }

                var control = new FlowControlFileTransferRequest(Form.FlowPanel, this, fromName, fileId, fileName, fileSize, isImage, color);
                AppendFlowControl(control);
                PendingFileTransfers.Add(fileId, control);

                Form.Invoke(() =>
                {
                    if (Form.Visible == false)
                    {
                        //We want to show the dialog, but keep it minimized so that it does not jump in front of the user.
                        Form.WindowState = FormWindowState.Minimized;
                        Form.Visible = true;
                    }

                    if (playNotifications)
                    {
                        if (WindowFlasher.FlashWindow(Form))
                        {
                            Notifications.MessageReceived(fromName);
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                AppendErrorLine(ex);
            }
        }

        private void AppendFolderLinkMessage(string fromName, string displayText, string folderPath, bool playNotifications, Color color)
        {
            try
            {
                if (Form == null || Form.FlowPanel == null)
                {
                    return;
                }

                Form.Invoke(() =>
                {
                    if (Form.Visible == false)
                    {
                        //We want to show the dialog, but keep it minimized so that it does not jump in front of the user.
                        Form.WindowState = FormWindowState.Minimized;
                        Form.Visible = true;
                    }

                    if (playNotifications)
                    {
                        if (WindowFlasher.FlashWindow(Form))
                        {
                            Notifications.MessageReceived(fromName);
                        }
                    }
                });

                LastMessageReceived = DateTime.Now;

                AppendFlowControl(new FlowControlFolderHyperlink(Form.FlowPanel, fromName, displayText, folderPath, color));
            }
            catch (Exception ex)
            {
                AppendErrorLine(ex);
            }
        }

        private void AppendImageMessage(string fromName, byte[] imageBytes, bool playNotifications, Color displayNameColor)
        {
            try
            {
                if (Form == null || Form.FlowPanel == null)
                {
                    return;
                }

                AppendFlowControl(new FlowControlImage(Form.FlowPanel, fromName, imageBytes, displayNameColor));

                Form.Invoke(() =>
                {
                    if (Form.Visible == false)
                    {
                        //We want to show the dialog, but keep it minimized so that it does not jump in front of the user.
                        Form.WindowState = FormWindowState.Minimized;
                        Form.Visible = true;
                    }

                    if (playNotifications)
                    {
                        if (WindowFlasher.FlashWindow(Form))
                        {
                            Notifications.MessageReceived(fromName);
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                AppendErrorLine(ex);
            }
        }

        /// <summary>
        /// Adds a control to monitor the inbound file transfer progress.
        /// </summary>
        /// <returns></returns>
        public FlowControlFileTransferReceiveProgress AppendFileTransferReceiveProgress(Guid fileId, string fileName, long fileSize, bool isImage, string? saveAsFileName = null)
        {
            if (Form == null || Form.FlowPanel == null)
            {
                throw new Exception("Form is not initialized.");
            }

            var control = new FlowControlFileTransferReceiveProgress(Form.FlowPanel, this, fileId, fileName, fileSize, isImage, saveAsFileName);
            AppendFlowControl(control);

            return control;
        }

        /// <summary>
        /// Adds a control to monitor the outbound file transfer progress.
        /// </summary>
        /// <returns></returns>
        public FlowControlFileTransferSendProgress AppendFileTransferSendProgress(string fileName, long fileSize, Stream stream)
        {
            if (Form == null || Form.FlowPanel == null)
            {
                throw new Exception("Form is not initialized.");
            }

            var control = new FlowControlFileTransferSendProgress(Form.FlowPanel, this, fileName, fileSize, stream);
            AppendFlowControl(control);

            return control;
        }

        public void AppendErrorLine(Exception ex, Color? color = null)
        {
            if (Form == null || Form.FlowPanel == null)
            {
                return;
            }

            var baseException = ex.GetBaseException();
            AppendFlowControl(new FlowControlSystemText(Form.FlowPanel, baseException.Message, color ?? Color.Red));
            Log.Error(baseException, baseException.Message);
        }

        public void AppendErrorLine(string message, Color? color = null)
        {
            if (Form == null || Form.FlowPanel == null)
            {
                return;
            }

            AppendFlowControl(new FlowControlSystemText(Form.FlowPanel, message, color ?? Color.Red));
            Log.Error(message);
        }

        public void AppendSystemMessageLine(string message, Color? color = null)
        {
            if (Form == null || Form.FlowPanel == null)
            {
                return;
            }

            AppendFlowControl(new FlowControlSystemText(Form.FlowPanel, message, color));
        }

        public void AppendSuccessMessageLine(string message, Color? color = null)
        {
            color ??= Color.Green;

            if (Form == null || Form.FlowPanel == null)
            {
                return;
            }

            AppendFlowControl(new FlowControlSystemText(Form.FlowPanel, message, color));
        }

        public void AppendIncomingCallRequest(string fromName)
        {
            if (Form == null || Form.FlowPanel == null)
            {
                return;
            }

            AppendFlowControl(new FlowControlIncomingCall(Form.FlowPanel, this, fromName));
        }

        public void AppendOutgoingCallRequest(string toName)
        {
            if (Form == null || Form.FlowPanel == null)
            {
                return;
            }

            LastOutgoingCallControl = new FlowControlOutgoingCall(Form.FlowPanel, this, toName);
            AppendFlowControl(LastOutgoingCallControl);
        }

        public void AppendReceivedMessageLine(string fromName, string plainText, bool playNotifications, Color displayNameColor)
        {
            try
            {
                if (Form == null || Form.FlowPanel == null)
                {
                    return;
                }

                Form.Invoke(() =>
                {
                    if (Form.Visible == false)
                    {
                        //We want to show the dialog, but keep it minimized so that it does not jump in front of the user.
                        Form.WindowState = FormWindowState.Minimized;
                        Form.Visible = true;
                    }

                    if (playNotifications)
                    {
                        if (WindowFlasher.FlashWindow(Form))
                        {
                            Notifications.MessageReceived(fromName);
                        }
                    }
                });

                LastMessageReceived = DateTime.Now;

                if (plainText.StartsWith("http://") || plainText.StartsWith("https://"))
                {
                    AppendFlowControl(new FlowControlHyperlink(Form.FlowPanel, fromName, plainText, displayNameColor));
                }
                else
                {
                    AppendFlowControl(new FlowControlTextMessage(Form.FlowPanel, fromName, plainText, displayNameColor));
                }
            }
            catch (Exception ex)
            {
                AppendErrorLine(ex);
            }
        }

        public void AppendIncomingCall(string fromName, bool playNotifications, Color? color = null)
        {
            try
            {
                if (Form == null || Form.FlowPanel == null)
                {
                    return;
                }

                Form.Invoke(() =>
                {
                    if (Form.Visible == false)
                    {
                        //We want to show the dialog, but keep it minimized so that it does not jump in front of the user.
                        Form.WindowState = FormWindowState.Minimized;
                        Form.Visible = true;
                    }

                    if (playNotifications)
                    {
                        if (WindowFlasher.FlashWindow(Form))
                        {
                            Notifications.IncomingCall(fromName);
                        }
                    }
                });

                LastMessageReceived = DateTime.Now;

                AppendIncomingCallRequest(fromName);
            }
            catch (Exception ex)
            {
                AppendErrorLine(ex);
            }
        }

        #endregion

        #region Symmetric Cryptography.

        public string DecryptString(byte[] cipherText)
        {
            lock (_streamCryptography)
            {
                return _streamCryptography.DecryptString(cipherText);
            }
        }

        public byte[] EncryptString(string plainText)
        {
            lock (_streamCryptography)
            {
                return _streamCryptography.EncryptString(plainText);
            }
        }

        public byte[] Cipher(byte[] bytes)
        {
            lock (_streamCryptography)
            {
                return _streamCryptography.Cipher(bytes);
            }
        }

        #endregion
    }
}
