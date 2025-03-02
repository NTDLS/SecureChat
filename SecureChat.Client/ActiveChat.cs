using NTDLS.NASCCL;
using SecureChat.Client.Forms;
using SecureChat.Library.ReliableMessages;
using System.Text;

namespace SecureChat.Client
{
    internal class ActiveChat
    {
        public FormMessage? Form { get; set; }
        public Guid AccountId { get; set; }
        public string DisplayName { get; set; }
        public Guid ConnectionId { get; set; }
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

        public void ReceiveMessage(byte[] cipherText)
        {
            Form?.AppendReceivedMessageFrom(Color.DarkRed, DisplayName, Decrypt(cipherText));
        }

        public bool SendMessage(string plaintText)
        {
            return SessionState.Instance?.Client.Query(new ExchangePeerToPeerQuery(
                    ConnectionId, SessionState.Instance.AccountId, Encrypt(plaintText))).ContinueWith(o =>
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
