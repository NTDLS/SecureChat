using NTDLS.ReliableMessaging;

namespace SecureChat.Library
{
    public class ReliableCryptographyProvider
        : IRmCryptographyProvider
    {
        public PublicPrivateKeyPair PublicPrivateKeyPair { get; private set; }

        public int RsaKeySize { get; private set; }
        public int AesKeySize { get; private set; }

        public ReliableCryptographyProvider(int rsaKeySize, int aesKeySize, byte[] publicRsaKey, byte[] privateRsaKey)
        {
            RsaKeySize = rsaKeySize;
            AesKeySize = aesKeySize;
            PublicPrivateKeyPair = new PublicPrivateKeyPair(publicRsaKey, privateRsaKey);
        }

        public byte[] Decrypt(RmContext context, byte[] encryptedPayload)
            => Crypto.AesDecryptBytes(RsaKeySize, encryptedPayload, PublicPrivateKeyPair.PrivateRsaKey);

        public byte[] Encrypt(RmContext context, byte[] payload)
            => Crypto.AesEncryptBytes(RsaKeySize, AesKeySize, payload, PublicPrivateKeyPair.PublicRsaKey);
    }
}
