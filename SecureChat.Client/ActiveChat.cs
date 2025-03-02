using NTDLS.NASCCL;
using SecureChat.Client.Forms;
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

        public string Decrypt(byte[] cypherText)
        {
            lock (_streamCryptography)
            {
                var plainTextBytes = _streamCryptography.Cipher(cypherText);
                _streamCryptography.ResetStream();
                return Encoding.UTF8.GetString(plainTextBytes);
            }
        }

        public byte[] Encrypt(string plainText)
        {
            lock (_streamCryptography)
            {
                var cypherText = _streamCryptography.Cipher(plainText);
                _streamCryptography.ResetStream();
                return cypherText;
            }
        }
    }
}
