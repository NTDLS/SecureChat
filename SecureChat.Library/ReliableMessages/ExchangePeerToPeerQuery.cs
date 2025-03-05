using NTDLS.ReliableMessaging;

namespace SecureChat.Library.ReliableMessages
{
    public class ExchangePeerToPeerQuery
        : IRmQuery<ExchangePeerToPeerQueryReply>
    {
        public byte[] CipherText { get; set; }
        public Guid ConnectionId { get; set; }
        public Guid PeerToPeerId { get; set; }

        public ExchangePeerToPeerQuery(Guid peerToPeerId, Guid connectionId, byte[] cipherText)
        {
            PeerToPeerId = peerToPeerId;
            ConnectionId = connectionId;
            CipherText = cipherText;
        }
    }

    public class ExchangePeerToPeerQueryReply
    : IRmQueryReply
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }

        public ExchangePeerToPeerQueryReply(Exception exception)
        {
            IsSuccess = false;
            ErrorMessage = exception.GetBaseException().Message;
        }

        public ExchangePeerToPeerQueryReply()
        {
            IsSuccess = true;
        }
    }
}
