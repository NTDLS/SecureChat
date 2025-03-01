namespace SecureChat.Library
{
    public class PublicPrivateKeyPair
    {
        public byte[] PublicRsaKey { get; private set; }
        public byte[] PrivateRsaKey { get; private set; }

        public PublicPrivateKeyPair(byte[] publicRsaKey, byte[] privateRsaKey)
        {
            PublicRsaKey = publicRsaKey;
            PrivateRsaKey = privateRsaKey;
        }
    }
}
