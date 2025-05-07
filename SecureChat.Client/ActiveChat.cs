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
        public FormMessage? Form { get; set; }
        public Guid AccountId { get; private set; }
        public string DisplayName { get; private set; }
        public Guid PeerConnectionId { get; private set; }
        public Dictionary<Guid, FileReceiveBuffer> FileReceiveBuffers { get; set; } = new();
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

            var keepAliveThread = new Thread(() =>
            {
                while (!IsTerminated)
                {
                    try
                    {
                        ServerConnection.Current?.ReliableClient.Notify(new SessionKeepAliveNotification(SessionId));
                    }
                    catch (Exception ex)
                    {
                        //TODO: Log or report
                    }

                    var breakTime = DateTime.UtcNow.AddSeconds(10);
                    while (!IsTerminated && DateTime.UtcNow < breakTime)
                    {
                        Thread.Sleep(500);
                    }
                }
            });

            keepAliveThread.Start();
        }

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

        public void Terminate()
        {
            if (IsTerminated)
            {
                return;
            }
            IsTerminated = true;
            ServerConnection.Current?.ReliableClient.Notify(new TerminateChatNotification(SessionId, PeerConnectionId));
            AppendSystemMessageLine($"Conversation has ended.");
            StopAudioPump();
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
        /// Tell the remote client that we are canceling the file transmission.
        /// </summary>
        /// <param name="fileId"></param>
        public void CancelFileTransmission(Guid fileId)
        {
            ServerConnection.Current?.ReliableClient.Notify(new FileTransmissionCancelNotification(SessionId, PeerConnectionId, fileId));
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

        public void ReceiveImage(byte[] imageBytes)
        {
            if (IsTerminated)
            {
                return;
            }

            AppendImageMessage(DisplayName, imageBytes, true, ScConstants.FromRemoteColor);
        }

        public void AlertOfIncomingCall()
        {
            if (IsTerminated)
            {
                return;
            }

            AppendIncomingCall(DisplayName, true, Color.Blue);
        }

        public void ReceiveMessage(byte[] cipherText)
        {
            if (IsTerminated)
            {
                return;
            }

            AppendReceivedMessageLine(DisplayName, DecryptString(cipherText), true, ScConstants.FromRemoteColor);
        }

        public bool SendMessage(string plaintText)
        {
            if (IsTerminated)
            {
                return false;
            }

            return ServerConnection.Current?.ReliableClient.Query(new ExchangeMessageTextQuery(SessionId,
                    PeerConnectionId, EncryptString(plaintText))).ContinueWith(o =>
                    {
                        if (!o.IsFaulted && o.Result.IsSuccess)
                        {
                            return true;
                        }
                        return false;
                    }).Result ?? false;
        }

        public void CancelTransmitFile()
        {
            ServerConnection.Current?.ReliableClient.Notify(new CancelVoiceCallRequestNotification(SessionId, PeerConnectionId));
        }


        public void TransmitFileAsync(string fileName)
        {
            long fileSize = (new FileInfo(fileName)).Length;

            if (fileSize > Settings.Instance.MaxFileTransmissionSize)
            {
                AppendErrorLine($"File is too large {Formatters.FileSize(fileSize)}, max size is {Formatters.FileSize(Settings.Instance.MaxFileTransmissionSize)}.");
            }
            else if (fileSize > 0)
            {
                Task.Run(() =>
                {
                    using var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                    TransmitFile(fileName, fileSize, fileStream);
                });
            }
        }

        public void TransmitFileAsync(string fileName, byte[] fileBytes)
        {
            long fileSize = fileBytes.LongLength;

            if (fileSize > Settings.Instance.MaxFileTransmissionSize)
            {
                AppendErrorLine($"File is too large {Formatters.FileSize(fileSize)}, max size is {Formatters.FileSize(Settings.Instance.MaxFileTransmissionSize)}.");
            }
            else if (fileSize > 0)
            {
                Task.Run(() =>
                {
                    using var stream = new MemoryStream(fileBytes);
                    TransmitFile(fileName, fileBytes.LongLength, stream);
                });
            }
        }

        private void TransmitFile(string fileName, long fileSize, Stream fileStream)
        {
            bool isImage = ScConstants.ImageFileTypes.Contains(Path.GetExtension(fileName), StringComparer.InvariantCultureIgnoreCase);
            var fileId = Guid.NewGuid();

            var progressControl = AppendFileTransmissionProgress(fileId, fileName, fileSize);
            if (progressControl == null)
            {
                return;
            }

            ServerConnection.Current?.ReliableClient.Query(new FileTransmissionBeginQuery(SessionId, PeerConnectionId, fileId, fileName, fileSize));

            double totalBytesSent = 0;

            try
            {
                var buffer = new byte[Settings.Instance.FileTransmissionChunkSize];
                int bytesRead;

                while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) > 0 && !progressControl.IsCancelled)
                {
                    var chunkToSend = buffer;
                    if (bytesRead < buffer.Length) // Handle the last partial chunk
                    {
                        chunkToSend = new byte[bytesRead];
                        Array.Copy(buffer, chunkToSend, bytesRead);
                    }

                    totalBytesSent += bytesRead;
                    double completionPercentage = (totalBytesSent / fileSize) * 100.0;
                    progressControl.SetProgressValue((int)completionPercentage);

                    // Transmit the current chunk
                    ServerConnection.Current?.ReliableClient.Query(new FileTransmissionChunkQuery(SessionId, PeerConnectionId, fileId, Cipher(chunkToSend)));
                }

                if (!progressControl.IsCancelled)
                {
                    ServerConnection.Current?.ReliableClient.Query(new FileTransmissionEndQuery(SessionId, PeerConnectionId, fileId)).ContinueWith(o =>
                    {
                        if (!o.IsFaulted && o.Result.IsSuccess)
                        {
                            if (isImage)
                            {
                                // Load the image only after successful transmission
                                var imageData = File.ReadAllBytes(fileName);
                                AppendImageMessage(ServerConnection.Current.DisplayName, imageData, false, ScConstants.FromMeColor);
                            }
                            else
                            {
                                AppendSystemMessageLine($"File '{fileName}' transmitted successfully.", Color.Green);
                            }
                        }
                        else
                        {
                            AppendErrorLine($"Failed to transmit file '{fileName}'.", Color.Red);
                        }
                    });
                }
                else
                {
                    AppendSystemMessageLine($"File transmission canceled.", Color.Red);
                }
            }
            catch (Exception ex)
            {
                AppendErrorLine($"Error transmitting file: {ex.Message}", Color.Red);
            }
            finally
            {
                progressControl.Remove();
            }
        }


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

        public void AppendImageMessage(string fromName, byte[] imageBytes, bool playNotifications, Color color)
        {
            try
            {
                if (Form == null || Form.FlowPanel == null)
                {
                    return;
                }

                AppendFlowControl(new FlowControlImage(Form.FlowPanel, fromName, imageBytes, color));

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

        public FlowControlFileTransmissionProgress? AppendFileTransmissionProgress(Guid fileId, string fileName, long fileSize)
        {
            if (Form == null || Form.FlowPanel == null)
            {
                return null;
            }

            var control = new FlowControlFileTransmissionProgress(Form.FlowPanel, this, fileId, fileName, fileSize);
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

        public void AppendReceivedMessageLine(string fromName, string plainText, bool playNotifications, Color? color = null)
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
                    AppendFlowControl(new FlowControlHyperlink(Form.FlowPanel, fromName, plainText, color));
                }
                else
                {
                    AppendFlowControl(new FlowControlTextMessage(Form.FlowPanel, fromName, plainText, color));
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
    }
}
