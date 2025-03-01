using NTDLS.ReliableMessaging;

namespace SecureChat.Library.ReliableMessages
{
    public class InitiateEndToEndCryptography
        : IRmQuery<InitiateEndToEndCryptographyReply>
    {
        /// <summary>
        /// The AccountId that we need to tell the server to route the messages to.
        /// </summary>
        public Guid AccountId { get; set; }
        public string DisplayName { get; set; }
        public byte[] NegotiationToken { get; set; }

        /// <summary>
        /// The ConnectionId that we need to tell the server to route the messages to.
        /// This is enriched at the server before dispatching the query to the client by the AccountId
        /// </summary>
        public Guid PeerConnectionId { get; set; }

        public InitiateEndToEndCryptography(Guid accountId, string displayName, byte[] negotiationToken)
        {
            AccountId = accountId;
            DisplayName = displayName;
            NegotiationToken = negotiationToken;
        }
    }

    public class InitiateEndToEndCryptographyReply
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

        public InitiateEndToEndCryptographyReply(Exception exception)
        {
            NegotiationToken = Array.Empty<byte>();
            IsSuccess = false;
            ErrorMessage = exception.GetBaseException().Message;
        }

        public InitiateEndToEndCryptographyReply(byte[] negotiationToken)
        {
            NegotiationToken = negotiationToken;
            IsSuccess = true;
        }

        public InitiateEndToEndCryptographyReply()
        {
            NegotiationToken = Array.Empty<byte>();
        }
    }
}
