using NTDLS.ReliableMessaging;

namespace SecureChat.Library.ReliableMessages
{
    public class ExchangePeerToPeerMessage
        : IRmNotification
    {
        public byte[] CipherText { get; set; }
        public Guid ConnectionId { get; set; }
        public Guid MessageFromAccountId { get; set; }

        public ExchangePeerToPeerMessage(Guid connectionId, Guid messageFromAccountId, byte[] cipherText)
        {
            ConnectionId = connectionId;
            MessageFromAccountId = messageFromAccountId;
            CipherText = cipherText;
        }
    }
}
