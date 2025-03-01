using NTDLS.ReliableMessaging;

namespace SecureChat.Library
{
    public class BaselineCryptographyProvider : IRmCryptographyProvider
    {
        private readonly PublicPrivateKeyPair _publicPrivateKeyPair;

        public BaselineCryptographyProvider(byte[] publicRsaKey, byte[] privateRsaKey)
        {
            Console.WriteLine("Encrypt with: " + Crypto.ComputeSha256Hash(publicRsaKey));
            Console.WriteLine("Decrypt with: " + Crypto.ComputeSha256Hash(privateRsaKey));


            _publicPrivateKeyPair = new PublicPrivateKeyPair(publicRsaKey, privateRsaKey);
        }

        public byte[] Decrypt(RmContext context, byte[] encryptedPayload)
        {
            return Crypto.RsaDecryptBytes(encryptedPayload, _publicPrivateKeyPair.PrivateRsaKey);
        }

        public byte[] Encrypt(RmContext context, byte[] payload)
        {
            return Crypto.RsaEncryptBytes(payload, _publicPrivateKeyPair.PublicRsaKey);
        }
    }
}
