using NTDLS.ReliableMessaging;

namespace SecureChat.Library.ReliableMessages
{
    public class ExchangePublicKeyQuery
        : IRmQuery<ExchangePublicKeyQueryReply>
    {
        public Guid PeerConnectionId { get; set; }
        public Version ClientVersion { get; set; }
        public byte[] PublicRsaKey { get; set; }

        public int RsaKeySize { get; set; }
        public int AesKeySize { get; set; }


        public ExchangePublicKeyQuery(Guid peerConnectionId, Version clientVersion, byte[] publicRsaKey, int rsaKeySize, int aesKeySize)
        {
            PeerConnectionId = peerConnectionId;
            PublicRsaKey = publicRsaKey;
            ClientVersion = clientVersion;
            RsaKeySize = rsaKeySize;
            AesKeySize = aesKeySize;
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
