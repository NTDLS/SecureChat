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
        /// Identifies a unique session. If a session is ended and a new one is started, it will have a different PeerToPeerId.
        /// </summary>
        public Guid PeerToPeerId { get; set; }

        /// <summary>
        /// The ConnectionId that we need to tell the server to route the messages to.
        /// This is enriched at the server before dispatching the query to the client by the AccountId
        /// </summary>
        public Guid PeerConnectionId { get; set; }

        public InitiateEndToEndCryptographyQuery(Guid peerToPeerId, Guid sourceAccountId, Guid targetAccountId, string displayName, byte[] negotiationToken)
        {
            PeerToPeerId = peerToPeerId;
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
