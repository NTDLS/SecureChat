using NTDLS.ReliableMessaging;

namespace SecureChat.Library.ReliableMessages
{
    public class ExchangeMessageTextQuery
        : IRmQuery<ExchangeMessageTextQueryReply>
    {
        public byte[] CipherText { get; set; }
        public Guid ConnectionId { get; set; }
        public Guid PeerToPeerId { get; set; }

        public ExchangeMessageTextQuery(Guid peerToPeerId, Guid connectionId, byte[] cipherText)
        {
            PeerToPeerId = peerToPeerId;
            ConnectionId = connectionId;
            CipherText = cipherText;
        }
    }

    public class ExchangeMessageTextQueryReply
    : IRmQueryReply
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }

        public ExchangeMessageTextQueryReply(Exception exception)
        {
            IsSuccess = false;
            ErrorMessage = exception.GetBaseException().Message;
        }

        public ExchangeMessageTextQueryReply()
        {
            IsSuccess = true;
        }
    }
}
