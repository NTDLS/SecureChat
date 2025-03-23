using NTDLS.DatagramMessaging;
using NTDLS.Permafrost;
using SecureChat.Client.Forms;
using SecureChat.Library;
using SecureChat.Library.DatagramMessages;
using SecureChat.Library.ReliableMessages;

namespace SecureChat.Client
{
    internal class ActiveChat
    {
        public bool IsTerminated { get; private set; } = false;
        public FormMessage? Form { get; set; }
        public Guid AccountId { get; private set; }
        public string DisplayName { get; private set; }
        public Guid ConnectionId { get; private set; }
        private readonly PermafrostCipher _streamCryptography;
        public Dictionary<Guid, FileReceiveBuffer> FileReceiveBuffers { get; set; } = new();
        public DmClient? DatagramClient { get; private set; }
        public Guid PeerToPeerId { get; private set; }

        public ActiveChat(Guid peerToPeerId, Guid connectionId, Guid accountId, string displayName, byte[] sharedSecret)
        {
            PeerToPeerId = peerToPeerId;
            _streamCryptography = new PermafrostCipher(sharedSecret, PermafrostMode.AutoReset);
            ConnectionId = connectionId;
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

        /// <summary>
        /// Sends a packet to the server letting it know that we will be using UPD.
        /// This tells the server to establish encryption using the reliable messaging CryptographyProvider.
        /// After this packet is sent, we also set the local datagram client to use the local reliable messaging CryptographyProvider.
        /// </summary>
        public void InitiateNetworkAddressTranslationMessage(Guid peerToPeerId, Guid connectionId)
        {
            DatagramClient = Settings.Instance.CreateDmClient();
            DatagramClient.Dispatch(new InitiateNetworkAddressTranslationMessage(peerToPeerId, connectionId));

            if (LocalSession.Current == null)
                throw new Exception("Local connection is not established.");

            //Obtain the public and private key-pair from the reliable connection so we can use it for the datagram messaging.
            var rmCryptographyProvider = LocalSession.Current?.ReliableClient.GetCryptographyProvider() as ReliableCryptographyProvider
                ?? throw new Exception("Reliable cryptography has not been initialized.");

            DatagramClient.Context.SetCryptographyProvider(new DatagramCryptographyProvider(rmCryptographyProvider.PublicPrivateKeyPair));
        }

        public void Terminate()
        {
            if (IsTerminated)
            {
                return;
            }
            IsTerminated = true;
            LocalSession.Current?.ReliableClient.Notify(new TerminateChatNotification(ConnectionId, PeerToPeerId));
            DatagramClient?.Stop();
            Form?.AppendSystemMessageLine($"Chat ended at {DateTime.Now}.", Color.Red);
        }

        public void RequestVoiceCall()
        {
            LocalSession.Current?.ReliableClient.Notify(new RequestVoiceCallNotification(PeerToPeerId, ConnectionId));
            //Prop up the UDP connection:
            InitiateNetworkAddressTranslationMessage(PeerToPeerId, ConnectionId);
        }

        public void CancelVoiceCallRequest()
        {
            LocalSession.Current?.ReliableClient.Notify(new CancelVoiceCallRequestNotification(PeerToPeerId, ConnectionId));
        }

        public void AcceptVoiceCallRequest()
        {
            LocalSession.Current?.ReliableClient.Notify(new AcceptVoiceCallNotification(PeerToPeerId, ConnectionId));
        }

        public void DeclineVoiceCallRequest()
        {
            LocalSession.Current?.ReliableClient.Notify(new DeclineVoiceCallNotification(PeerToPeerId, ConnectionId));
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

            return LocalSession.Current?.ReliableClient.Query(new ExchangeMessageTextQuery(PeerToPeerId,
                    ConnectionId, EncryptString(plaintText))).ContinueWith(o =>
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

            LocalSession.Current?.ReliableClient.Notify(new FileTransmissionBeginNotification(PeerToPeerId, ConnectionId, fileId, fileName, fileBytes.Length));

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

                    LocalSession.Current?.ReliableClient.Notify(new FileTransmissionChunkNotification(PeerToPeerId, ConnectionId, fileId, Cipher(chunkToSend)));
                }
            }

            LocalSession.Current?.ReliableClient.Query(new FileTransmissionEndQuery(PeerToPeerId, ConnectionId, fileId)).ContinueWith(o =>
            {
                if (!o.IsFaulted && o.Result.IsSuccess)
                {
                    //Only show the image locally if the file was successfully transmitted.
                    Form?.AppendImageMessage(LocalSession.Current.DisplayName, fileBytes, false);
                }
                else
                {
                    Form?.AppendSystemMessageLine($"Failed to transmit file.", Color.Red);
                }
            });
        }
    }
}
