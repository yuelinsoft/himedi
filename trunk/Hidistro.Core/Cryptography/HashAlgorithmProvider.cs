namespace Hidistro.Core.Cryptography
{
    using System;
    using System.Security.Cryptography;

    public class HashAlgorithmProvider : IHashProvider
    {
       Type algorithmType;
       bool saltEnabled;
        public const int SaltLength = 0x10;

        public HashAlgorithmProvider(Type algorithmType, bool saltEnabled)
        {
            if (algorithmType == null)
            {
                throw new ArgumentNullException("algorithmType");
            }
            if (!typeof(HashAlgorithm).IsAssignableFrom(algorithmType))
            {
                throw new ArgumentException("The type must be a HashAlgorithm.", "algorithmType");
            }
            this.algorithmType = algorithmType;
            this.saltEnabled = saltEnabled;
        }

       void AddSaltToHash(byte[] salt, ref byte[] hash)
        {
            if (this.saltEnabled)
            {
                hash = CryptographyUtility.CombineBytes(salt, hash);
            }
        }

       void AddSaltToPlainText(ref byte[] salt, ref byte[] plaintext)
        {
            if (this.saltEnabled)
            {
                if (salt == null)
                {
                    salt = CryptographyUtility.GetRandomBytes(0x10);
                }
                plaintext = CryptographyUtility.CombineBytes(salt, plaintext);
            }
        }

        public bool CompareHash(byte[] plaintext, byte[] hashedtext)
        {
            if (plaintext == null)
            {
                throw new ArgumentNullException("plainText");
            }
            if (hashedtext == null)
            {
                throw new ArgumentNullException("hashedText");
            }
            if (hashedtext.Length == 0)
            {
                throw new ArgumentException("The value must be greater than 0 bytes.", "hashedText");
            }
            byte[] buffer = null;
            byte[] salt = null;
            try
            {
                salt = this.ExtractSalt(hashedtext);
                buffer = this.CreateHashWithSalt(plaintext, salt);
            }
            finally
            {
                CryptographyUtility.ZeroOutBytes(salt);
            }
            return CryptographyUtility.CompareBytes(buffer, hashedtext);
        }

        public byte[] CreateHash(byte[] plaintext)
        {
            return this.CreateHashWithSalt(plaintext, null);
        }

        protected virtual byte[] CreateHashWithSalt(byte[] plaintext, byte[] salt)
        {
            this.AddSaltToPlainText(ref salt, ref plaintext);
            byte[] hash = this.HashCryptographer.ComputeHash(plaintext);
            this.AddSaltToHash(salt, ref hash);
            return hash;
        }

        protected byte[] ExtractSalt(byte[] hashedtext)
        {
            if (!this.saltEnabled)
            {
                return null;
            }
            byte[] dst = null;
            if (hashedtext.Length > 0x10)
            {
                dst = new byte[0x10];
                Buffer.BlockCopy(hashedtext, 0, dst, 0, 0x10);
            }
            return dst;
        }

        protected Type AlgorithmType
        {
            get
            {
                return this.algorithmType;
            }
        }

        protected virtual Hidistro.Core.Cryptography.HashCryptographer HashCryptographer
        {
            get
            {
                return new Hidistro.Core.Cryptography.HashCryptographer(this.algorithmType);
            }
        }
    }
}

