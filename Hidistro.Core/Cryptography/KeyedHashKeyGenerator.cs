namespace Hidistro.Core.Cryptography
{
    using System;
    using System.Security.Cryptography;

    public class KeyedHashKeyGenerator : IKeyGenerator
    {
       KeyedHashAlgorithm CreateAlgorithm(string keyedHashAlgorithmName)
        {
            KeyedHashAlgorithm algorithm = (KeyedHashAlgorithm) CryptoConfig.CreateFromName(keyedHashAlgorithmName);
            if (algorithm == null)
            {
                throw new ArgumentException(string.Format("Algorithm of type {0} is not a known keyed hash algorithm", keyedHashAlgorithmName));
            }
            return algorithm;
        }

       KeyedHashAlgorithm CreateAlgorithm(Type keyedHashAlgorithmType)
        {
            KeyedHashAlgorithm algorithm = null;
            try
            {
                algorithm = (KeyedHashAlgorithm) Activator.CreateInstance(keyedHashAlgorithmType);
            }
            catch (Exception)
            {
                throw new ArgumentException("Algorithm of type {0} is not a known keyed hash algorithm", keyedHashAlgorithmType.Name);
            }
            return algorithm;
        }

        public byte[] GenerateKey(string keyedHashAlgorithmName)
        {
            using (KeyedHashAlgorithm algorithm = this.CreateAlgorithm(keyedHashAlgorithmName))
            {
                return algorithm.Key;
            }
        }

        public byte[] GenerateKey(Type keyedHashAlgorithmType)
        {
            using (KeyedHashAlgorithm algorithm = this.CreateAlgorithm(keyedHashAlgorithmType))
            {
                return algorithm.Key;
            }
        }

       ProtectedKey GenerateKey(KeyedHashAlgorithm algorithm, DataProtectionScope dataProtectionScope)
        {
            using (algorithm)
            {
                return ProtectedKey.CreateFromPlaintextKey(algorithm.Key, dataProtectionScope);
            }
        }

        public ProtectedKey GenerateKey(string keyedHashAlgorithmName, DataProtectionScope dataProtectionScope)
        {
            return this.GenerateKey(this.CreateAlgorithm(keyedHashAlgorithmName), dataProtectionScope);
        }

        public ProtectedKey GenerateKey(Type keyedHashAlgorithmType, DataProtectionScope dataProtectionScope)
        {
            return this.GenerateKey(this.CreateAlgorithm(keyedHashAlgorithmType), dataProtectionScope);
        }
    }
}

