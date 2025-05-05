using NTDLS.DatagramMessaging;
using NTDLS.Permafrost;
using SecureChat.Client.Audio;
using SecureChat.Client.Controls;
using SecureChat.Client.Forms;
using SecureChat.Library;
using SecureChat.Library.DatagramMessages;
using SecureChat.Library.ReliableMessages;

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
        public bool IsTerminated { get; private set; } = false;
        public FormMessage? Form { get; set; }
        public Guid AccountId { get; private set; }
        public string DisplayName { get; private set; }
        public Guid PeerConnectionId { get; private set; }
        public Dictionary<Guid, FileReceiveBuffer> FileReceiveBuffers { get; set; } = new();
        public Guid PeerToPeerId { get; private set; }

        public DmClient? DatagramClient { get; private set; }

        public ActiveChat(Guid peerToPeerId, Guid peerConnectionId, Guid accountId, string displayName, byte[] sharedSecret)
        {
            PeerToPeerId = peerToPeerId;
            _streamCryptography = new PermafrostCipher(sharedSecret, PermafrostMode.AutoReset);
            PeerConnectionId = peerConnectionId;
            AccountId = accountId;
            DisplayName = displayName;
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

        public void StartAudioPump()
        {
            _audioPump = new AudioPump(_inputDeviceIndex, _outputDeviceIndex, _bitrate);

            _audioPump.OnFrameProduced += (byte[] bytes, int byteCount) =>
            {
                //Sends the recorded audio to the server, for dispatch to the correct client.
                DatagramClient?.Dispatch(new VoicePacketMessage(PeerToPeerId, PeerConnectionId, bytes));
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
        /// Plays the received audio packet.
        /// </summary>
        /// <param name="bytes"></param>
        public void IngestAudioPacket(byte[] bytes)
        {
            _audioPump?.IngestFrame(bytes, bytes.Length);
        }

        /// <summary>
        /// Sends a packet to the server letting it know that we will be using UPD.
        /// This tells the server to establish encryption using the reliable messaging CryptographyProvider.
        /// After this packet is sent, we also set the local datagram client to use the local reliable messaging CryptographyProvider.
        /// 
        /// This is called by the sender client via ActiveChat.RequestVoiceCall() and the
        ///     recipient client via ClientReliableMessageHandlers.RequestVoiceCallNotification().
        /// </summary>
        public void InitiateNetworkAddressTranslationMessage(Guid peerToPeerId, Guid connectionId)
        {
            DatagramClient = Settings.Instance.CreateDmClient();

            DatagramClient.OnDatagramReceived += DatagramClient_OnDatagramReceived;

            DatagramClient.Dispatch(new InitiateNetworkAddressTranslationMessage(peerToPeerId, connectionId));

            if (ServerConnection.Current == null)
                throw new Exception("Local connection is not established.");

            //Obtain the public and private key-pair from the reliable connection so we can use it for the datagram messaging.
            var rmCryptographyProvider = ServerConnection.Current?.ReliableClient.GetCryptographyProvider() as ReliableCryptographyProvider
                ?? throw new Exception("Reliable cryptography has not been initialized.");

            DatagramClient.Context.SetCryptographyProvider(new DatagramCryptographyProvider(rmCryptographyProvider.PublicPrivateKeyPair));
        }

        private void DatagramClient_OnDatagramReceived(DmContext context, IDmDatagram datagram)
        {

        }

        public void Terminate()
        {
            if (IsTerminated)
            {
                return;
            }
            IsTerminated = true;
            ServerConnection.Current?.ReliableClient.Notify(new TerminateChatNotification(PeerToPeerId, PeerConnectionId));
            DatagramClient?.Stop();
            Form?.AppendSystemMessageLine($"Chat ended at {DateTime.Now}.", Color.Red);
        }

        /// <summary>
        /// Client is sending another client a request for a voice call.
        /// </summary>
        public void RequestVoiceCall(int inputDeviceIndex, int outputDeviceIndex, int bitrate)
        {
            _inputDeviceIndex = inputDeviceIndex;
            _outputDeviceIndex = outputDeviceIndex;
            _bitrate = bitrate;

            ServerConnection.Current?.ReliableClient.Notify(new RequestVoiceCallNotification(PeerToPeerId, PeerConnectionId));

            //Prop up the UDP connection:
            InitiateNetworkAddressTranslationMessage(PeerToPeerId, PeerConnectionId);
        }

        /// <summary>
        /// Original requesting client is canceling a voice call request.
        /// </summary>
        public void CancelVoiceCallRequest()
        {
            ServerConnection.Current?.ReliableClient.Notify(new CancelVoiceCallRequestNotification(PeerToPeerId, PeerConnectionId));
        }

        /// <summary>
        /// Client which received the request for a voice call is is accepting the request.
        /// </summary>
        public void AcceptVoiceCallRequest(int inputDeviceIndex, int outputDeviceIndex, int bitrate)
        {
            _inputDeviceIndex = inputDeviceIndex;
            _outputDeviceIndex = outputDeviceIndex;
            _bitrate = bitrate;

            ServerConnection.Current?.ReliableClient.Notify(new AcceptVoiceCallNotification(PeerToPeerId, PeerConnectionId));
        }

        /// <summary>
        /// Client which received the request for a voice call is is declining the request.
        /// </summary>
        public void DeclineVoiceCallRequest()
        {
            ServerConnection.Current?.ReliableClient.Notify(new DeclineVoiceCallNotification(PeerToPeerId, PeerConnectionId));
        }

        public void ReceiveImage(byte[] imageBytes)
        {
            if (IsTerminated)
            {
                return;
            }

            Form?.AppendImageMessage(DisplayName, imageBytes, true);
        }

        public void AlertOfIncomingCall()
        {
            if (IsTerminated)
            {
                return;
            }

            Form?.AppendIncomingCall(DisplayName, true, Color.Blue);
        }

        public void ReceiveMessage(byte[] cipherText)
        {
            if (IsTerminated)
            {
                return;
            }

            Form?.AppendReceivedMessageLine(DisplayName, DecryptString(cipherText), true, Color.DarkRed);
        }

        public bool SendMessage(string plaintText)
        {
            if (IsTerminated)
            {
                return false;
            }

            return ServerConnection.Current?.ReliableClient.Query(new ExchangeMessageTextQuery(PeerToPeerId,
                    PeerConnectionId, EncryptString(plaintText))).ContinueWith(o =>
                    {
                        if (!o.IsFaulted && o.Result.IsSuccess)
                        {
                            return true;
                        }
                        return false;
                    }).Result ?? false;
        }

        public void TransmitFile(string fileName, byte[] fileBytes)
        {
            var fileId = Guid.NewGuid();

            ServerConnection.Current?.ReliableClient.Notify(new FileTransmissionBeginNotification(PeerToPeerId, PeerConnectionId, fileId, fileName, fileBytes.Length));

            using (var memoryStream = new MemoryStream(fileBytes))
            {
                var buffer = new byte[Settings.Instance.FileTransmissionChunkSize];

                int bytesRead;

                while ((bytesRead = memoryStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    var chunkToSend = buffer; // Default: Full buffer
                    if (bytesRead < buffer.Length) // Handle last partial chunk
                    {
                        chunkToSend = new byte[bytesRead];
                        Array.Copy(buffer, chunkToSend, bytesRead);
                    }

                    ServerConnection.Current?.ReliableClient.Notify(new FileTransmissionChunkNotification(PeerToPeerId, PeerConnectionId, fileId, Cipher(chunkToSend)));
                }
            }

            ServerConnection.Current?.ReliableClient.Query(new FileTransmissionEndQuery(PeerToPeerId, PeerConnectionId, fileId)).ContinueWith(o =>
            {
                if (!o.IsFaulted && o.Result.IsSuccess)
                {
                    //Only show the image locally if the file was successfully transmitted.
                    Form?.AppendImageMessage(ServerConnection.Current.DisplayName, fileBytes, false);
                }
                else
                {
                    Form?.AppendSystemMessageLine($"Failed to transmit file.", Color.Red);
                }
            });
        }
    }
}
