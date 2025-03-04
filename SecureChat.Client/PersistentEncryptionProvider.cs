using NTDLS.Persistence;
using System.Security.Cryptography;

namespace SecureChat.Client
{
    public class PersistentEncryptionProvider : IEncryptionProvider
    {
        public byte[] Encrypt(byte[] plainText)
            => ProtectedData.Protect(plainText, null, DataProtectionScope.CurrentUser);

        public byte[] Decrypt(byte[] encryptedBytes)
            => ProtectedData.Unprotect(encryptedBytes, null, DataProtectionScope.CurrentUser);
    }
}
