namespace Hidistro.Core.Cryptography
{
    using System;
    using System.IO;
    using System.Security.Cryptography;

    public class SymmetricAlgorithmProvider : ISymmetricCryptoProvider
    {
       Type algorithmType;
       ProtectedKey key;

        public SymmetricAlgorithmProvider(Type algorithmType, ProtectedKey key)
        {
            if (algorithmType == null)
            {
                throw new ArgumentNullException("algorithmType");
            }
            if (!typeof(SymmetricAlgorithm).IsAssignableFrom(algorithmType))
            {
                throw new ArgumentException("The type must be of type SymmetricAlgorithm.", "algorithmType");
            }
            this.algorithmType = algorithmType;
            this.key = key;
        }

        public SymmetricAlgorithmProvider(Type algorithmType, Stream protectedKeyStream, DataProtectionScope protectedKeyProtectionScope) : this(algorithmType, KeyManager.Read(protectedKeyStream, protectedKeyProtectionScope))
        {
        }

        public SymmetricAlgorithmProvider(Type algorithmType, string protectedKeyFileName, DataProtectionScope protectedKeyProtectionScope) : this(algorithmType, KeyManager.Read(protectedKeyFileName, protectedKeyProtectionScope))
        {
        }

        public byte[] Decrypt(byte[] ciphertext)
        {
            if (ciphertext == null)
            {
                throw new ArgumentNullException("ciphertext");
            }
            if (ciphertext.Length == 0)
            {
                throw new ArgumentException("The value must be greater than 0 bytes.", "ciphertext");
            }
            using (SymmetricCryptographer cryptographer = new SymmetricCryptographer(this.algorithmType, this.key))
            {
                return cryptographer.Decrypt(ciphertext);
            }
        }

        public byte[] Encrypt(byte[] plaintext)
        {
            if (plaintext == null)
            {
                throw new ArgumentNullException("plainText");
            }
            if (plaintext.Length == 0)
            {
                throw new ArgumentException("The value must be greater than 0 bytes.", "plaintext");
            }
            using (SymmetricCryptographer cryptographer = new SymmetricCryptographer(this.algorithmType, this.key))
            {
                return cryptographer.Encrypt(plaintext);
            }
        }
    }
}

