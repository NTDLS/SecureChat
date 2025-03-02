using NTDLS.NASCCL;
using SecureChat.Client.Forms;
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

        public ActiveChat(Guid connectionId, Guid accountId, string displayName, byte[] sharedSecret)
        {
            _streamCryptography = new CryptoStream(sharedSecret);
            ConnectionId = connectionId;
            AccountId = accountId;
            DisplayName = displayName;
        }

        private string Decrypt(byte[] cipherText)
        {
            lock (_streamCryptography)
            {
                var plainTextBytes = _streamCryptography.Cipher(cipherText);
                _streamCryptography.ResetStream();
                return Encoding.UTF8.GetString(plainTextBytes);
            }
        }

        private byte[] Encrypt(string plainText)
        {
            lock (_streamCryptography)
            {
                var cipherText = _streamCryptography.Cipher(plainText);
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
            LocalSession.Current?.Client.Notify(new TerminateChat(ConnectionId, LocalSession.Current.AccountId));
            Form?.AppendSystemMessageLine(Color.Red, $"The chat ended at {DateTime.Now}.");
        }

        public void ReceiveMessage(byte[] cipherText)
        {
            if (IsTerminated)
            {
                return;
            }

            Form?.AppendReceivedMessageLine(Color.DarkRed, DisplayName, Decrypt(cipherText));
        }

        public bool SendMessage(string plaintText)
        {
            if (IsTerminated)
            {
                return false;
            }

            return LocalSession.Current?.Client.Query(new ExchangePeerToPeerQuery(
                    ConnectionId, LocalSession.Current.AccountId, Encrypt(plaintText))).ContinueWith(o =>
                    {
                        if (!o.IsFaulted && o.Result.IsSuccess)
                        {
                            return true;
                        }
                        return false;
                    }).Result ?? false;
        }
    }
}
