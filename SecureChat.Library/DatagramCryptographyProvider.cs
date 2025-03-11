using NTDLS.DatagramMessaging;

namespace SecureChat.Library
{
    public class DatagramCryptographyProvider
        : IDmCryptographyProvider
    {
        private readonly PublicPrivateKeyPair _publicPrivateKeyPair;

        public DatagramCryptographyProvider(PublicPrivateKeyPair publicPrivateKeyPair)
            => _publicPrivateKeyPair = publicPrivateKeyPair;

        public byte[] Decrypt(DmContext context, byte[] encryptedPayload)
        {
            return encryptedPayload;
            //=> Crypto.AesDecryptBytes(encryptedPayload, _publicPrivateKeyPair.PrivateRsaKey);
        }

        public byte[] Encrypt(DmContext context, byte[] payload)
        {
            return payload;
            //=> Crypto.AesEncryptBytes(payload, _publicPrivateKeyPair.PublicRsaKey);
        }
    }
}
