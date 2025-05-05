using NTDLS.ReliableMessaging;

namespace SecureChat.Library.ReliableMessages
{
    public class ExchangeMessageTextQuery
        : IRmQuery<ExchangeMessageTextQueryReply>
    {
        public byte[] CipherText { get; set; }

        /// <summary>
        /// The connection id of the remote peer that this message is being sent to.
        /// </summary>
        public Guid PeerConnectionId { get; set; }

        /// <summary>
        /// Identifies a unique session. If a session is ended and a new one is started, it will have a different PeerToPeerId.
        /// </summary>
        public Guid PeerToPeerId { get; set; }

        public ExchangeMessageTextQuery(Guid peerToPeerId, Guid peerConnectionId, byte[] cipherText)
        {
            PeerToPeerId = peerToPeerId;
            PeerConnectionId = peerConnectionId;
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
