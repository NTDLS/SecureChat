using NTDLS.ReliableMessaging;

namespace SecureChat.Library.ReliableMessages
{
    public class InitiateEndToEndCryptographyQuery
        : IRmQuery<InitiateEndToEndCryptographyQueryReply>
    {
        /// <summary>
        /// The AccountId that is requesting the chat.
        /// </summary>
        public Guid SourceAccountId { get; set; }

        /// <summary>
        /// The AccountId that we need to tell the server to route the messages to.
        /// </summary>
        public Guid TargetAccountId { get; set; }
        public string DisplayName { get; set; }
        public byte[] NegotiationToken { get; set; }

        /// <summary>
        /// Identifies this chat session. This is used to identify the chat session when sending messages.
        /// If the session is ended and a new one is started, it will have a different SessionId - even if it is the same contact.
        /// </summary>
        public Guid SessionId { get; set; }

        /// <summary>
        /// The ConnectionId that we need to tell the server to route the messages to.
        /// This is enriched at the server via the handler for InitiateEndToEndCryptographyQuery()
        ///     before dispatching the query to the client by the AccountId.
        /// </summary>
        public Guid PeerConnectionId { get; set; }

        public InitiateEndToEndCryptographyQuery(Guid sessionId, Guid sourceAccountId, Guid targetAccountId, string displayName, byte[] negotiationToken)
        {
            SessionId = sessionId;
            SourceAccountId = sourceAccountId;
            TargetAccountId = targetAccountId;
            DisplayName = displayName;
            NegotiationToken = negotiationToken;
        }
    }

    public class InitiateEndToEndCryptographyQueryReply
        : IRmQueryReply
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
        public byte[] NegotiationToken { get; set; }
        public string? DisplayName { get; set; }

        /// <summary>
        /// The ConnectionId that we need to tell the server to route the messages to.
        /// This is enriched at the server before dispatching the reply to the requester.
        /// </summary>
        public Guid PeerConnectionId { get; set; }

        public InitiateEndToEndCryptographyQueryReply(Exception exception)
        {
            NegotiationToken = Array.Empty<byte>();
            IsSuccess = false;
            ErrorMessage = exception.GetBaseException().Message;
        }

        public InitiateEndToEndCryptographyQueryReply(byte[] negotiationToken)
        {
            NegotiationToken = negotiationToken;
            IsSuccess = true;
        }

        public InitiateEndToEndCryptographyQueryReply()
        {
            NegotiationToken = Array.Empty<byte>();
        }
    }
}
