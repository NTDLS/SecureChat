using NTDLS.ReliableMessaging;

namespace SecureChat.Library.ReliableMessages
{
    public class ExchangePublicKeyQuery
        : IRmQuery<ExchangePublicKeyQueryReply>
    {
        public byte[] PublicRsaKey { get; set; }
        public ExchangePublicKeyQuery(byte[] publicRsaKey)
        {
            PublicRsaKey = publicRsaKey;
        }
    }

    public class ExchangePublicKeyQueryReply
        : IRmQueryReply
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
        public byte[] PublicRsaKey { get; set; }

        public ExchangePublicKeyQueryReply(Exception exception)
        {
            PublicRsaKey = Array.Empty<byte>();
            IsSuccess = false;
            ErrorMessage = exception.GetBaseException().Message;
        }

        public ExchangePublicKeyQueryReply(byte[] publicRsaKey)
        {
            PublicRsaKey = publicRsaKey;
            IsSuccess = true;
        }

        public ExchangePublicKeyQueryReply()
        {
            PublicRsaKey = Array.Empty<byte>();
        }
    }
}
