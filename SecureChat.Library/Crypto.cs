using System.Security.Cryptography;
using System.Text;

namespace SecureChat.Library
{
    public static class Crypto
    {
        public static string ComputeSha256Hash(string input)
            => ComputeSha256Hash(Encoding.UTF8.GetBytes(input));

        public static string ComputeSha256Hash(byte[] inputBytes)
        {
            var stringBuilder = new StringBuilder();

            var hashBytes = SHA256.HashData(inputBytes);
            for (int i = 0; i < hashBytes.Length; i++)
            {
                stringBuilder.Append(hashBytes[i].ToString("x2"));
            }

            return stringBuilder.ToString();
        }

        public static PublicPrivateKeyPair GeneratePublicPrivateKeyPair()
        {
            using var rsa = RSA.Create(4096);
            return new PublicPrivateKeyPair(rsa.ExportSubjectPublicKeyInfo(), rsa.ExportPkcs8PrivateKey());
        }

        /// <summary>
        /// Encrypts a byte array with AES using a public key and returns array.
        /// </summary>
        public static byte[] AesEncryptBytes(byte[] data, byte[] publicKey)
        {
            using RSA rsa = RSA.Create(4096);
            rsa.ImportSubjectPublicKeyInfo(publicKey, out _);

            using Aes aes = Aes.Create();
            aes.KeySize = 256;
            aes.GenerateKey();
            aes.GenerateIV();

            //Encrypt the AES key with the RSA public key.
            byte[] encryptedKey = rsa.Encrypt(aes.Key, RSAEncryptionPadding.OaepSHA256);

            //Use AES to encrypt the data.
            using ICryptoTransform encryptor = aes.CreateEncryptor();
            byte[] cipherText = encryptor.TransformFinalBlock(data, 0, data.Length);

            //Return the AES encrypted key length + encrypted AES key + IV (always 16 bytes) + AES encrypted data.
            byte[] result = new byte[4 + encryptedKey.Length + aes.IV.Length + cipherText.Length];
            BitConverter.GetBytes(encryptedKey.Length).CopyTo(result, 0);
            encryptedKey.CopyTo(result, 4);
            aes.IV.CopyTo(result, 4 + encryptedKey.Length);
            cipherText.CopyTo(result, 4 + encryptedKey.Length + aes.IV.Length);

            return result;
        }

        /// <summary>
        /// Decrypts a byte array with AES using a private key and returns a bytes array.
        /// </summary>
        public static byte[] AesDecryptBytes(byte[] encryptedData, byte[] privateKey)
        {
            using RSA rsa = RSA.Create(4096);
            rsa.ImportPkcs8PrivateKey(privateKey, out _);

            //Extract the encrypted AES key length.
            int keyLength = BitConverter.ToInt32(encryptedData, 0);

            //Extract the encrypted AES key.
            byte[] encryptedKey = new byte[keyLength];
            Array.Copy(encryptedData, 4, encryptedKey, 0, keyLength);

            //Decrypt the AES key.
            byte[] aesKey = rsa.Decrypt(encryptedKey, RSAEncryptionPadding.OaepSHA256);

            //Extract the AES IV (always 16 bytes).
            int ivOffset = 4 + keyLength;
            byte[] iv = new byte[16];
            Array.Copy(encryptedData, ivOffset, iv, 0, 16);

            //Extract the cypher text.
            int cipherTextOffset = ivOffset + iv.Length;
            byte[] cipherText = new byte[encryptedData.Length - cipherTextOffset];
            Array.Copy(encryptedData, cipherTextOffset, cipherText, 0, cipherText.Length);

            using Aes aes = Aes.Create();
            aes.Key = aesKey;
            aes.IV = iv; // Set IV for decryption

            //Decrypt the cypher text using AES.
            using ICryptoTransform decryptor = aes.CreateDecryptor();
            byte[] decryptedData = decryptor.TransformFinalBlock(cipherText, 0, cipherText.Length);

            return decryptedData;
        }
    }
}
