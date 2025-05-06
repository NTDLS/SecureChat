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
        /// Identifies this chat session. This is used to identify the chat session when sending messages.
        /// If the session is ended and a new one is started, it will have a different SessionId - even if it is the same contact.
        /// </summary>
        public Guid SessionId { get; set; }

        public ExchangeMessageTextQuery(Guid sessionId, Guid peerConnectionId, byte[] cipherText)
        {
            SessionId = sessionId;
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
