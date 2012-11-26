namespace Hidistro.Core.Cryptography
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;
    using System.Web;

    public static class Cryptographer
    {
        public static bool CompareHash(string plaintext, string hashedText)
        {
            return CompareHash(GetHashProvider(), plaintext, hashedText);
        }

        public static bool CompareHash(byte[] plaintext, byte[] hashedText)
        {
            return GetHashProvider().CompareHash(plaintext, hashedText);
        }

        internal static bool CompareHash(IHashProvider provider, string plaintext, string hashedText)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(plaintext);
            byte[] hashedtext = Convert.FromBase64String(hashedText);
            bool flag = provider.CompareHash(bytes, hashedtext);
            CryptographyUtility.GetRandomBytes(bytes);
            return flag;
        }

        public static string CreateHash(string plaintext)
        {
            return CreateHash(GetHashProvider(), plaintext);
        }

        public static byte[] CreateHash(byte[] plaintext)
        {
            return GetHashProvider().CreateHash(plaintext);
        }

        internal static string CreateHash(IHashProvider provider, string plaintext)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(plaintext);
            byte[] inArray = provider.CreateHash(bytes);
            CryptographyUtility.GetRandomBytes(bytes);
            return Convert.ToBase64String(inArray);
        }

        public static byte[] Decrypt(byte[] ciphertext)
        {
            return GetSymmetricCryptoProvider().Decrypt(ciphertext);
        }

        public static string Decrypt(string ciphertextBase64)
        {
            return DecryptSymmetric(GetSymmetricCryptoProvider(), ciphertextBase64);
        }

        internal static string DecryptSymmetric(ISymmetricCryptoProvider provider, string ciphertextBase64)
        {
            if (string.IsNullOrEmpty(ciphertextBase64))
            {
                throw new ArgumentException("The value can not be a null or empty string.", "ciphertextBase64");
            }
            byte[] ciphertext = Convert.FromBase64String(ciphertextBase64);
            byte[] bytes = provider.Decrypt(ciphertext);
            string str = Encoding.Unicode.GetString(bytes);
            CryptographyUtility.GetRandomBytes(bytes);
            return str;
        }

        public static byte[] DecryptWithPassword(byte[] ciphertext, string password)
        {
            MemoryStream stream = new MemoryStream();
            using (RijndaelManaged managed = GetAESCryptoProvider(password))
            {
                CryptoStream stream2 = new CryptoStream(stream, managed.CreateDecryptor(), CryptoStreamMode.Write);
                stream2.Write(ciphertext, 0, ciphertext.Length);
                stream2.FlushFinalBlock();
                stream2.Close();
            }
            byte[] buffer = stream.ToArray();
            stream.Close();
            return buffer;
        }

        public static string DecryptWithPassword(string ciphertextBase64, string password)
        {
            byte[] ciphertext = Convert.FromBase64String(ciphertextBase64);
            byte[] bytes = DecryptWithPassword(ciphertext, password);
            string str = Encoding.UTF8.GetString(bytes);
            CryptographyUtility.ZeroOutBytes(bytes);
            CryptographyUtility.ZeroOutBytes(ciphertext);
            return str;
        }

        public static string Encrypt(string plaintext)
        {
            return EncryptSymmetric(GetSymmetricCryptoProvider(), plaintext);
        }

        public static byte[] Encrypt(byte[] plaintext)
        {
            return GetSymmetricCryptoProvider().Encrypt(plaintext);
        }

        internal static string EncryptSymmetric(ISymmetricCryptoProvider provider, string plaintext)
        {
            if (string.IsNullOrEmpty(plaintext))
            {
                throw new ArgumentException("The value can not be a null or empty string.", "plaintext");
            }
            byte[] bytes = Encoding.Unicode.GetBytes(plaintext);
            byte[] inArray = provider.Encrypt(bytes);
            CryptographyUtility.GetRandomBytes(bytes);
            return Convert.ToBase64String(inArray);
        }

        public static byte[] EncryptWithPassword(byte[] plaintext, string password)
        {
            MemoryStream stream = new MemoryStream();
            using (RijndaelManaged managed = GetAESCryptoProvider(password))
            {
                CryptoStream stream2 = new CryptoStream(stream, managed.CreateEncryptor(), CryptoStreamMode.Write);
                stream2.Write(plaintext, 0, plaintext.Length);
                stream2.FlushFinalBlock();
                stream2.Close();
            }
            byte[] buffer = stream.ToArray();
            stream.Close();
            return buffer;
        }

        public static string EncryptWithPassword(string plaintext, string password)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(plaintext);
            byte[] inArray = EncryptWithPassword(bytes, password);
            string str = Convert.ToBase64String(inArray);
            CryptographyUtility.ZeroOutBytes(bytes);
            CryptographyUtility.ZeroOutBytes(inArray);
            return str;
        }

        internal static RijndaelManaged GetAESCryptoProvider(string password)
        {
            RijndaelManaged managed = new RijndaelManaged();
            byte[] bytes = Encoding.UTF8.GetBytes(password);
            managed.KeySize = 0x100;
            using (SHA256 sha = new SHA256Managed())
            {
                managed.Key = sha.ComputeHash(bytes);
            }
            using (SHA1 sha2 = new SHA1Managed())
            {
                byte[] buffer2 = sha2.ComputeHash(bytes);
                byte[] buffer3 = new byte[0x10];
                for (int i = 0; i < 0x10; i++)
                {
                    buffer3[i] = buffer2[i];
                }
                managed.IV = buffer3;
                CryptographyUtility.ZeroOutBytes(buffer3);
                CryptographyUtility.ZeroOutBytes(buffer2);
            }
            CryptographyUtility.ZeroOutBytes(bytes);
            return managed;
        }

       static IHashProvider GetHashProvider()
        {
            return new HashAlgorithmProvider(typeof(MD5CryptoServiceProvider), true);
        }

       static ISymmetricCryptoProvider GetSymmetricCryptoProvider()
        {
            string str;
            HttpContext current = HttpContext.Current;
            if (current != null)
            {
                str = current.Request.MapPath("~/config/hishop.key");
            }
            else
            {
                str = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"config\hishop.key");
            }
            return new SymmetricAlgorithmProvider(typeof(RijndaelManaged), str, DataProtectionScope.LocalMachine);
        }
    }
}

