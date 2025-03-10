using NTDLS.ReliableMessaging;

namespace SecureChat.Library
{
    public class ReliableCryptographyProvider
        : IRmCryptographyProvider
    {
        public PublicPrivateKeyPair PublicPrivateKeyPair { get; private set; }

        public ReliableCryptographyProvider(byte[] publicRsaKey, byte[] privateRsaKey)
            => PublicPrivateKeyPair = new PublicPrivateKeyPair(publicRsaKey, privateRsaKey);

        public byte[] Decrypt(RmContext context, byte[] encryptedPayload)
            => Crypto.AesDecryptBytes(encryptedPayload, PublicPrivateKeyPair.PrivateRsaKey);

        public byte[] Encrypt(RmContext context, byte[] payload)
            => Crypto.AesEncryptBytes(payload, PublicPrivateKeyPair.PublicRsaKey);
    }
}
