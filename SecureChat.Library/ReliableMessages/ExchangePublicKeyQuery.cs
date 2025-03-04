using NTDLS.ReliableMessaging;

namespace SecureChat.Library.ReliableMessages
{
    public class ExchangePublicKeyQuery
        : IRmQuery<ExchangePublicKeyQueryReply>
    {
        public Version ClientVersion { get; set; }
        public byte[] PublicRsaKey { get; set; }

        public ExchangePublicKeyQuery(Version clientVersion, byte[] publicRsaKey)
        {
            PublicRsaKey = publicRsaKey;
            ClientVersion = clientVersion;
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
