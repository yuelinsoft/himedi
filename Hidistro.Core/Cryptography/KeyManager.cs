namespace Hidistro.Core.Cryptography
{
    using System;
    using System.IO;
    using System.Security.Cryptography;

    public static class KeyManager
    {
       static readonly ProtectedKeyCache cache = new ProtectedKeyCache();
       static readonly KeyedHashKeyGenerator keyedHashKeyGenerator = new KeyedHashKeyGenerator();
       static readonly SymmetricKeyGenerator symmetricKeyGenerator = new SymmetricKeyGenerator();

        public static void ArchiveKey(Stream outputStream, ProtectedKey keyToArchive, string passphrase)
        {
            IKeyWriter writer = new KeyReaderWriter();
            writer.Archive(outputStream, keyToArchive, passphrase);
        }

        public static void ClearCache()
        {
            cache.Clear();
        }

        public static byte[] GenerateKeyedHashKey(string keyedHashKeyAlgorithmName)
        {
            return keyedHashKeyGenerator.GenerateKey(keyedHashKeyAlgorithmName);
        }

        public static byte[] GenerateKeyedHashKey(Type keyedHashAlgorithmType)
        {
            return keyedHashKeyGenerator.GenerateKey(keyedHashAlgorithmType);
        }

        public static ProtectedKey GenerateKeyedHashKey(string keyedHashKeyAlgorithmName, DataProtectionScope dataProtectionScope)
        {
            return keyedHashKeyGenerator.GenerateKey(keyedHashKeyAlgorithmName, dataProtectionScope);
        }

        public static ProtectedKey GenerateKeyedHashKey(Type keyedHashAlgorithmType, DataProtectionScope dataProtectionScope)
        {
            return keyedHashKeyGenerator.GenerateKey(keyedHashAlgorithmType, dataProtectionScope);
        }

        public static byte[] GenerateSymmetricKey(string symmetricAlgorithmName)
        {
            return symmetricKeyGenerator.GenerateKey(symmetricAlgorithmName);
        }

        public static byte[] GenerateSymmetricKey(Type symmetricAlgorithmType)
        {
            return symmetricKeyGenerator.GenerateKey(symmetricAlgorithmType);
        }

        public static ProtectedKey GenerateSymmetricKey(string symmetricAlgorithmName, DataProtectionScope dataProtectionScope)
        {
            return symmetricKeyGenerator.GenerateKey(symmetricAlgorithmName, dataProtectionScope);
        }

        public static ProtectedKey GenerateSymmetricKey(Type symmetricAlgorithmType, DataProtectionScope dataProtectionScope)
        {
            return symmetricKeyGenerator.GenerateKey(symmetricAlgorithmType, dataProtectionScope);
        }

        public static ProtectedKey Read(Stream inputStream, DataProtectionScope dpapiProtectionScope)
        {
            IKeyReader reader = new KeyReaderWriter();
            return reader.Read(inputStream, dpapiProtectionScope);
        }

        public static ProtectedKey Read(string protectedKeyFileName, DataProtectionScope dpapiProtectionScope)
        {
            if (cache[protectedKeyFileName] != null)
            {
                return cache[protectedKeyFileName];
            }
            using (FileStream stream = new FileStream(protectedKeyFileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                ProtectedKey key = Read(stream, dpapiProtectionScope);
                cache[protectedKeyFileName] = key;
                return key;
            }
        }

        public static ProtectedKey RestoreKey(Stream inputStream, string passphrase, DataProtectionScope protectionScope)
        {
            IKeyReader reader = new KeyReaderWriter();
            return reader.Restore(inputStream, passphrase, protectionScope);
        }

        public static void Write(Stream outputStream, ProtectedKey key)
        {
            IKeyWriter writer = new KeyReaderWriter();
            writer.Write(outputStream, key);
        }

        public static void Write(Stream outputStream, byte[] encryptedKey, DataProtectionScope dpapiProtectionScope)
        {
            ProtectedKey key = ProtectedKey.CreateFromEncryptedKey(encryptedKey, dpapiProtectionScope);
            Write(outputStream, key);
        }
    }
}

