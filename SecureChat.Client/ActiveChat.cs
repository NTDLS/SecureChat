using NTDLS.NASCCL;
using SecureChat.Client.Forms;
using SecureChat.Client.Models;
using SecureChat.Library.ReliableMessages;
using System.Text;

namespace SecureChat.Client
{
    internal class ActiveChat
    {
        public bool IsTerminated { get; private set; } = false;
        public FormMessage? Form { get; set; }
        public Guid AccountId { get; private set; }
        public string DisplayName { get; private set; }
        public Guid ConnectionId { get; private set; }
        private readonly CryptoStream _streamCryptography;
        public Dictionary<Guid, FileReceiveBuffer> FileReceiveBuffers { get; set; } = new();

        public Guid PeerToPeerId { get; private set; }

        public ActiveChat(Guid peerToPeerId, Guid connectionId, Guid accountId, string displayName, byte[] sharedSecret)
        {
            PeerToPeerId = peerToPeerId;
            _streamCryptography = new CryptoStream(sharedSecret);
            ConnectionId = connectionId;
            AccountId = accountId;
            DisplayName = displayName;
        }

        public string DecryptString(byte[] cipherText)
        {
            lock (_streamCryptography)
            {
                var plainTextBytes = _streamCryptography.Cipher(cipherText);
                _streamCryptography.ResetStream();
                return Encoding.UTF8.GetString(plainTextBytes);
            }
        }

        public byte[] EncryptString(string plainText)
        {
            lock (_streamCryptography)
            {
                var cipherText = _streamCryptography.Cipher(plainText);
                _streamCryptography.ResetStream();
                return cipherText;
            }
        }

        public byte[] Cipher(byte[] bytes)
        {
            lock (_streamCryptography)
            {
                var cipherText = _streamCryptography.Cipher(bytes);
                _streamCryptography.ResetStream();
                return cipherText;
            }
        }

        public void Terminate()
        {
            if (IsTerminated)
            {
                return;
            }
            IsTerminated = true;
            LocalSession.Current?.Client.Notify(new TerminateChat(ConnectionId, PeerToPeerId));
            Form?.AppendSystemMessageLine($"Chat ended at {DateTime.Now}.", Color.Red);
        }

        public void ReceiveImage(byte[] imageBytes)
        {
            if (IsTerminated)
            {
                return;
            }

            Form?.AppendImageMessage(DisplayName, imageBytes, true);
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

            return LocalSession.Current?.Client.Query(new ExchangePeerToPeerQuery(PeerToPeerId,
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

            LocalSession.Current?.Client.Notify(new FileTransmissionBegin(PeerToPeerId, ConnectionId, fileId, fileName, fileBytes.Length));

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

                    LocalSession.Current?.Client.Notify(new FileTransmissionChunk(PeerToPeerId, ConnectionId, fileId, Cipher(chunkToSend)));
                }
            }

            LocalSession.Current?.Client.Query(new FileTransmissionEnd(PeerToPeerId, ConnectionId, fileId)).ContinueWith(o =>
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
