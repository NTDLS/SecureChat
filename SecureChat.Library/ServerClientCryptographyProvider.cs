using NTDLS.ReliableMessaging;

namespace SecureChat.Library
{
    public class ServerClientCryptographyProvider
        : IRmCryptographyProvider
    {
        private readonly PublicPrivateKeyPair _publicPrivateKeyPair;

        public ServerClientCryptographyProvider(byte[] publicRsaKey, byte[] privateRsaKey)
            => _publicPrivateKeyPair = new PublicPrivateKeyPair(publicRsaKey, privateRsaKey);

        public byte[] Decrypt(RmContext context, byte[] encryptedPayload)
            => Crypto.AesDecryptBytes(encryptedPayload, _publicPrivateKeyPair.PrivateRsaKey);

        public byte[] Encrypt(RmContext context, byte[] payload)
            => Crypto.AesEncryptBytes(payload, _publicPrivateKeyPair.PublicRsaKey);
    }
}
