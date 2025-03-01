using NTDLS.ReliableMessaging;

namespace SecureChat.Library.ReliableMessages
{
    public class InitiateEndToEndCryptography
        : IRmQuery<InitiateEndToEndCryptographyReply>
    {
        public Guid AccountId { get; set; }
        public byte[] NegotiationToken { get; set; }
        public InitiateEndToEndCryptography(Guid accountId, byte[] negotiationToken)
        {
            AccountId = accountId;
            NegotiationToken = negotiationToken;
        }
    }

    public class InitiateEndToEndCryptographyReply
        : IRmQueryReply
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
        public byte[] NegotiationToken { get; set; }

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
