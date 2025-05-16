using NTDLS.Helpers;
using NTDLS.Permafrost;
using SecureChat.Client.Audio;
using SecureChat.Client.Controls;
using SecureChat.Client.Controls.FlowControls;
using SecureChat.Client.Forms;
using SecureChat.Client.Helpers;
using SecureChat.Client.Properties;
using SecureChat.Library;
using SecureChat.Library.DatagramMessages;
using SecureChat.Library.ReliableMessages;
using static SecureChat.Library.ScConstants;

namespace SecureChat.Client
{
    internal class ActiveChat
    {
        private readonly PermafrostCipher _streamCryptography;
        private AudioPump? _audioPump;
        private int _inputDeviceIndex;
        private int _outputDeviceIndex;
        private int _bitrate;
        private ScOrigin _lastMessageOrigin = ScOrigin.None;

        /// <summary>
        /// When a voice call is sent, this will be the control that is displayed in the chat window.
        /// We save it so we can remove it when the call is accepted or canceled.
        /// </summary>
        public FlowControlOutgoingCall? LastOutgoingCallControl { get; set; }
        public PublicPrivateKeyPair PublicPrivateKeyPair { get; private set; }

        /// <summary>
        /// Shared secret used for symmetric end-to-end encryption.
        /// </summary>
        public byte[] SharedSecret { get; private set; }

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

            SharedSecret = sharedSecret;

            //Obtain the public and private key-pair from the reliable connection so we can use it for the datagram messaging.
            var rmCryptographyProvider = ServerConnection.Current?.Connection.Client.GetCryptographyProvider() as ReliableCryptographyProvider
                ?? throw new Exception("Reliable cryptography has not been initialized.");

            SessionId = sessionId;
            _streamCryptography = new PermafrostCipher(sharedSecret, PermafrostMode.AutoReset);
            PublicPrivateKeyPair = rmCryptographyProvider.PublicPrivateKeyPair;
            PeerConnectionId = peerConnectionId;
            AccountId = accountId;
            DisplayName = displayName;

            Form = ServerConnection.Current?.FormHome.CreateMessageForm(this)
                ?? throw new Exception("Unable to create message form. Server connection is not established.");

            if (Form.IsRecycled)
            {
                AppendSuccessMessageLine($"Conversation with {displayName} reconnected.");
            }
            else
            {
                AppendSuccessMessageLine($"Conversation with {displayName} started.");
            }

            new Thread(() =>
                {
                    while (!IsTerminated && ServerConnection.Current?.Connection.Client.IsConnected == true)
                    {
                        try
                        {
                            ServerConnection.Current?.Connection.Client.Notify(new SessionKeepAliveNotification(SessionId));
                        }
                        catch (Exception ex)
                        {
                            Program.Log.Error("Error sending session keep-alive notification.", ex);
                        }

                        var breakTime = DateTime.UtcNow.AddSeconds(10);
                        while (!IsTerminated && ServerConnection.Current?.Connection.Client.IsConnected == true && DateTime.UtcNow < breakTime)
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

            if (ServerConnection.Current?.Connection?.Client.IsConnected == true)
            {
                Exceptions.Ignore(() =>
                ServerConnection.Current.Connection.Client.Notify(new TerminateChatNotification(SessionId, PeerConnectionId)));
            }

            Exceptions.Ignore(() => AppendSystemMessageLine($"Conversation with {DisplayName} disconnected."));
            Exceptions.Ignore(() => StopAudioPump());
        }

        public void ReceiveFileTransferAcknowledgment(Guid fileId)
        {
            if (IsTerminated)
            {
                return;
            }

            try
            {
                if (Form?.FlowPanel == null || Form.IsDisposed || Form.Disposing)
                {
                    return;
                }

                Form.Invoke(() =>
                {
                    lock (Form.FlowPanel)
                    {
                        var bubble = Form.FlowPanel.Controls.OfType<FlowControlImage>().FirstOrDefault(o => o.FileId == fileId);
                        bubble?.SetStatusDelivered();
                    }
                });
            }
            catch (Exception ex)
            {
                AppendErrorLine(ex);
            }
        }

        public void ReceiveTextMessageAcknowledgment(Guid messageId)
        {
            if (IsTerminated)
            {
                return;
            }

            try
            {
                if (Form?.FlowPanel == null || Form.IsDisposed || Form.Disposing)
                {
                    return;
                }

                Form.Invoke(() =>
                {
                    lock (Form.FlowPanel)
                    {
                        var bubble = Form.FlowPanel.Controls.OfType<FlowControlOriginBubble>().FirstOrDefault(o => o.UID == messageId);
                        bubble?.SetStatusDelivered();
                    }
                });
            }
            catch (Exception ex)
            {
                AppendErrorLine(ex);
            }
        }

        public void ReceiveTextMessage(TextMessageNotification param)
        {
            if (IsTerminated)
            {
                return;
            }

            AppendChatMessage(DisplayName, DecryptString(param.CipherText), ScOrigin.Remote);

            //Let the server know that we have received the message.
            ServerConnection.Current?.Connection.Client.Notify(
                new TextMessageAcknowledgmentNotification(SessionId, PeerConnectionId, param.MessageId));
        }

        public void SendTextMessage(Guid messageId, string plaintText)
        {
            ServerConnection.Current?.Connection.Client.Notify(
                new TextMessageNotification(SessionId, PeerConnectionId, messageId, EncryptString(plaintText)));
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
                    ServerConnection.Current?.Connection.Client.Notify(new TerminateVoiceCallNotification(SessionId, PeerConnectionId));
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

            ServerConnection.Current?.Connection.Client.Notify(new RequestVoiceCallNotification(SessionId, PeerConnectionId));
        }

        /// <summary>
        /// Original requesting client is canceling a voice call request.
        /// </summary>
        public void CancelVoiceCallRequest()
        {
            ServerConnection.Current?.Connection.Client.Notify(new CancelVoiceCallRequestNotification(SessionId, PeerConnectionId));
        }

        /// <summary>
        /// Client which received the request for a voice call is is accepting the request.
        /// </summary>
        public void AcceptVoiceCallRequest(int inputDeviceIndex, int outputDeviceIndex, int bitrate)
        {
            _inputDeviceIndex = inputDeviceIndex;
            _outputDeviceIndex = outputDeviceIndex;
            _bitrate = bitrate;

            ServerConnection.Current?.Connection.Client.Notify(new AcceptVoiceCallNotification(SessionId, PeerConnectionId));
            AppendSystemMessageLine("Voice call is now connected.");
        }

        /// <summary>
        /// Client which received the request for a voice call is is declining the request.
        /// </summary>
        public void DeclineVoiceCallRequest()
        {
            ServerConnection.Current?.Connection.Client.Notify(new DeclineVoiceCallNotification(SessionId, PeerConnectionId));
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

            AppendFileTransferRequestMessage(DisplayName, fileId, fileName, fileSize, isImage, true, Theming.FromRemoteColor);
        }

        /// <summary>
        /// A file transfer was completed for what is presumably a non-image, show the user a link to the file.
        /// </summary>
        public void ReceiveFileMessage(Guid fileId, string? saveAsFileName)
        {
            if (IsTerminated)
            {
                return;
            }

            AppendFolderLinkMessage(DisplayName, fileId, Path.GetFileName(saveAsFileName) ?? "Open File Location",
                 Path.GetDirectoryName(saveAsFileName) ?? Environment.GetEnvironmentVariable("SystemDrive") ?? string.Empty, ScOrigin.Remote);
        }

        /// <summary>
        /// A file transfer was completed for an image, show it to the user.
        /// </summary>
        public void ReceiveImageMessage(Guid fileId, byte[] imageBytes)
        {
            if (IsTerminated)
            {
                return;
            }

            AppendImageMessage(DisplayName, fileId, imageBytes, ScOrigin.Remote);
        }

        /// <summary>
        /// Tell the remote client that we are canceling the file transfer.
        /// </summary>
        public void CancelFileTransfer(Guid fileId)
        {
            ServerConnection.Current?.Connection.Client.Notify(new FileTransferCancelNotification(SessionId, PeerConnectionId, fileId));
        }

        /// <summary>
        /// Tell the remote client that we are accepting the file transfer.
        /// </summary>
        /// <param name="fileId"></param>
        public void AcceptFileTransfer(Guid fileId)
        {
            ServerConnection.Current?.Connection.Client.Notify(new FileTransferAcceptRequestNotification(SessionId, PeerConnectionId, fileId));
        }

        /// <summary>
        /// Tell the remote client that we are declining the file transfer.
        /// </summary>
        /// <param name="fileId"></param>
        public void DeclineFileTransfer(FlowControlFileTransferRequest ftc)
        {
            ServerConnection.Current?.Connection.Client.Notify(new FileTransferDeclineRequestNotification(SessionId, PeerConnectionId, ftc.FileId));
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
                ServerConnection.Current?.Connection.Client.Notify(new FileTransferBeginRequestNotification(
                    SessionId, PeerConnectionId, ftc.Transfer.FileId, Path.GetFileName(ftc.Transfer.FileName), ftc.Transfer.FileSize, ftc.Transfer.IsImage));
            }
        }

        private void TransmitFileChunks(FlowControlFileTransferSendProgress ftc)
        {
            try
            {
                ServerConnection.Current?.Connection.Client.Query(new FileTransferBeginQuery(
                    SessionId, PeerConnectionId, ftc.Transfer.FileId, ftc.Transfer.FileName, ftc.Transfer.FileSize, ftc.Transfer.IsImage));

                double totalBytesSent = 0;

                var buffer = new byte[Settings.Instance.FileTransferChunkSize];
                int bytesRead;
                int chunkNumber = 0;

                using var crypto = new PermafrostCipher(SharedSecret, PermafrostMode.Continuous);

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

                    ServerConnection.Current?.Connection.Client.Notify( // Transmit the current chunk.
                        new FileTransferChunkQuery(SessionId, PeerConnectionId, ftc.Transfer.FileId, chunkNumber++, crypto.Cipher(chunkToSend)));
                }

                if (!ftc.IsCancelled && ServerConnection.Current != null)
                {
                    if (ftc.Transfer.IsImage)
                    {
                        // Load the image only after successful transfer
                        var imageData = ftc.Transfer.GetFileBytes();
                        AppendImageMessage(ServerConnection.Current.DisplayName, ftc.Transfer.FileId, imageData, ScOrigin.Local);
                    }
                    else
                    {
                        AppendSuccessMessageLine($"File '{Path.GetFileName(ftc.Transfer.FileName)}' transferred successfully.");
                    }
                }

            }
            catch (Exception ex)
            {
                if (ftc.IsCancelled == false)
                {
                    AppendErrorLine($"Error transferring file: {ex.Message}");
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
                if (Form?.FlowPanel == null || Form.IsDisposed || Form.Disposing)
                {
                    return;
                }

                _lastMessageOrigin = ScOrigin.None;

                Form.Invoke(() =>
                {
                    lock (Form.FlowPanel)
                    {
                        Form.FlowPanel.Controls.Add(control);
                        while (Form.FlowPanel.Controls.Count > Settings.Instance.MaxMessages)
                        {
                            Form.FlowPanel.Controls.RemoveAt(0);
                        }
                        //Form.FlowPanel.ScrollControlIntoView(control);
                        Form.FlowPanel.VerticalScroll.Value = Form.FlowPanel.VerticalScroll.Maximum;
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
                if (Form?.FlowPanel == null || Form.IsDisposed || Form.Disposing)
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
                            Notifications.MessageReceived(fromName, Form);
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                AppendErrorLine(ex);
            }
        }

        private void AppendFolderLinkMessage(string fromName, Guid fileId, string displayText, string folderPath, ScOrigin origin)
        {
            try
            {
                if (Form?.FlowPanel == null || Form.IsDisposed || Form.Disposing)
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

                    if (origin == ScOrigin.Remote)
                    {
                        if (WindowFlasher.FlashWindow(Form))
                        {
                            Notifications.MessageReceived(fromName, Form);
                        }
                    }
                });

                LastMessageReceived = DateTime.Now;

                AppendFlowControl(new FlowControlFolderHyperlink(Form.FlowPanel, displayText, folderPath, fileId, origin, null, fromName));
            }
            catch (Exception ex)
            {
                AppendErrorLine(ex);
            }
        }

        private void AppendImageMessage(string fromName, Guid fileId, byte[] imageBytes, ScOrigin origin)
        {
            try
            {
                if (Form?.FlowPanel == null || Form.IsDisposed || Form.Disposing)
                {
                    return;
                }

                AppendFlowControl(new FlowControlImage(Form.FlowPanel, imageBytes, fileId, origin,
                    origin == ScOrigin.Local ? Resources.MessageStatusSent16 : null, //If the message is being sent, show the sent icon.
                    fromName));

                Form.Invoke(() =>
                {
                    if (Form.Visible == false)
                    {
                        //We want to show the dialog, but keep it minimized so that it does not jump in front of the user.
                        Form.WindowState = FormWindowState.Minimized;
                        Form.Visible = true;
                    }

                    if (origin == ScOrigin.None)
                    {
                        if (WindowFlasher.FlashWindow(Form))
                        {
                            Notifications.MessageReceived(fromName, Form);
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
            if (Form?.FlowPanel == null || Form.IsDisposed || Form.Disposing)
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
            if (Form?.FlowPanel == null || Form.IsDisposed || Form.Disposing)
            {
                throw new Exception("Form is not initialized.");
            }

            var control = new FlowControlFileTransferSendProgress(Form.FlowPanel, this, fileName, fileSize, stream);
            AppendFlowControl(control);

            return control;
        }

        public void AppendErrorLine(Exception ex, Color? color = null)
        {
            if (Form?.FlowPanel == null || Form.IsDisposed || Form.Disposing)
            {
                return;
            }

            var baseException = ex.GetBaseException();
            AppendFlowControl(new FlowControlInformationText(Form.FlowPanel, baseException.Message, Color.Red));
            Program.Log.Error(baseException.Message, baseException);
        }

        public void AppendErrorLine(string message)
        {
            if (Form?.FlowPanel == null || Form.IsDisposed || Form.Disposing)
            {
                return;
            }

            AppendFlowControl(new FlowControlInformationText(Form.FlowPanel, message, Color.Red));
            Program.Log.Error(message);
        }

        public void AppendSystemMessageLine(string message)
        {
            if (Form?.FlowPanel == null || Form.IsDisposed || Form.Disposing)
            {
                return;
            }

            AppendFlowControl(new FlowControlInformationText(Form.FlowPanel, message, Color.Gray));
        }

        public void AppendSuccessMessageLine(string message)
        {
            if (Form?.FlowPanel == null || Form.IsDisposed || Form.Disposing)
            {
                return;
            }

            AppendFlowControl(new FlowControlInformationText(Form.FlowPanel, message, Color.Green));
        }

        public void AppendIncomingCallRequest(string fromName)
        {
            if (Form?.FlowPanel == null || Form.IsDisposed || Form.Disposing)
            {
                return;
            }

            AppendFlowControl(new FlowControlIncomingCall(Form.FlowPanel, this, fromName));
        }

        public void AppendOutgoingCallRequest(string toName)
        {
            if (Form?.FlowPanel == null || Form.IsDisposed || Form.Disposing)
            {
                return;
            }

            LastOutgoingCallControl = new FlowControlOutgoingCall(Form.FlowPanel, this, toName);
            AppendFlowControl(LastOutgoingCallControl);
        }

        public FlowControlOriginBubble? AppendChatMessage(string fromName, string plainText, ScOrigin origin)
        {
            FlowControlOriginBubble? control = null;

            try
            {
                if (Form?.FlowPanel == null || Form.IsDisposed || Form.Disposing)
                {
                    return null;
                }

                Form.Invoke(() =>
                {
                    if (Form.Visible == false)
                    {
                        //We want to show the dialog, but keep it minimized so that it does not jump in front of the user.
                        Form.WindowState = FormWindowState.Minimized;
                        Form.Visible = true;
                    }

                    if (origin == ScOrigin.Remote)
                    {
                        if (WindowFlasher.FlashWindow(Form))
                        {
                            Notifications.MessageReceived(fromName, Form);
                        }
                    }
                });

                LastMessageReceived = DateTime.Now;

                if (plainText.StartsWith("http://") || plainText.StartsWith("https://"))
                {
                    control = new FlowControlHyperlink(Form.FlowPanel, plainText, origin,
                        origin == ScOrigin.Local ? Resources.MessageStatusSending16 : null, //If the message is being sent, show the sending icon.
                        _lastMessageOrigin == origin ? null : fromName); //Only show the name if it is different from the last message.
                    AppendFlowControl(control);
                }
                else
                {
                    control = new FlowControlMessage(Form.FlowPanel, plainText, origin,
                        origin == ScOrigin.Local ? Resources.MessageStatusSending16 : null, //If the message is being sent, show the sending icon.
                        _lastMessageOrigin == origin ? null : fromName); //Only show the name if it is different from the last message.
                    AppendFlowControl(control);
                }

                _lastMessageOrigin = origin;
            }
            catch (Exception ex)
            {
                AppendErrorLine(ex);
            }

            return control;
        }

        public void AppendIncomingCall(string fromName, bool playNotifications, Color? color = null)
        {
            try
            {
                if (Form?.FlowPanel == null || Form.IsDisposed || Form.Disposing)
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
