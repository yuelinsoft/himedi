namespace Hidistro.Core.Cryptography
{
    using System;
    using System.Security.Cryptography;

    public class SymmetricCryptographer : IDisposable
    {
       SymmetricAlgorithm algorithm;
       ProtectedKey key;

        public SymmetricCryptographer(Type algorithmType, ProtectedKey key)
        {
            if (algorithmType == null)
            {
                throw new ArgumentNullException("algorithmType");
            }
            if (!typeof(SymmetricAlgorithm).IsAssignableFrom(algorithmType))
            {
                throw new ArgumentException("The type must be of type SymmetricAlgorithm.", "algorithmType");
            }
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            this.key = key;
            this.algorithm = GetSymmetricAlgorithm(algorithmType);
        }

        public byte[] Decrypt(byte[] encryptedText)
        {
            byte[] buffer = null;
            byte[] buffer2 = this.ExtractIV(encryptedText);
            this.algorithm.Key = this.Key;
            using (ICryptoTransform transform = this.algorithm.CreateDecryptor())
            {
                buffer = Transform(transform, buffer2);
            }
            CryptographyUtility.ZeroOutBytes(this.algorithm.Key);
            return buffer;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.algorithm != null)
            {
                this.algorithm.Clear();
                this.algorithm = null;
            }
        }

        public byte[] Encrypt(byte[] plaintext)
        {
            byte[] dst = null;
            byte[] src = null;
            this.algorithm.Key = this.Key;
            using (ICryptoTransform transform = this.algorithm.CreateEncryptor())
            {
                src = Transform(transform, plaintext);
            }
            dst = new byte[this.IVLength + src.Length];
            Buffer.BlockCopy(this.algorithm.IV, 0, dst, 0, this.IVLength);
            Buffer.BlockCopy(src, 0, dst, this.IVLength, src.Length);
            CryptographyUtility.ZeroOutBytes(this.algorithm.Key);
            return dst;
        }

       byte[] ExtractIV(byte[] encryptedText)
        {
            byte[] dst = new byte[this.IVLength];
            if (encryptedText.Length < (this.IVLength + 1))
            {
                throw new CryptographicException("Unable to decrypt data.");
            }
            byte[] buffer2 = new byte[encryptedText.Length - this.IVLength];
            Buffer.BlockCopy(encryptedText, 0, dst, 0, this.IVLength);
            Buffer.BlockCopy(encryptedText, this.IVLength, buffer2, 0, buffer2.Length);
            this.algorithm.IV = dst;
            return buffer2;
        }

        ~SymmetricCryptographer()
        {
            this.Dispose(false);
        }

       static SymmetricAlgorithm GetSymmetricAlgorithm(Type algorithmType)
        {
            return (Activator.CreateInstance(algorithmType) as SymmetricAlgorithm);
        }

       static byte[] Transform(ICryptoTransform transform, byte[] buffer)
        {
            return CryptographyUtility.Transform(transform, buffer);
        }

       int IVLength
        {
            get
            {
                if (this.algorithm.IV == null)
                {
                    this.algorithm.GenerateIV();
                }
                return this.algorithm.IV.Length;
            }
        }

       byte[] Key
        {
            get
            {
                return this.key.DecryptedKey;
            }
        }
    }
}

