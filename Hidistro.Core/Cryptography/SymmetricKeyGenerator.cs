namespace Hidistro.Core.Cryptography
{
    using System;
    using System.Security.Cryptography;

    public class SymmetricKeyGenerator : IKeyGenerator
    {
       SymmetricAlgorithm CreateAlgorithm(string symmetricAlgorithmName)
        {
            SymmetricAlgorithm algorithm = (SymmetricAlgorithm) CryptoConfig.CreateFromName(symmetricAlgorithmName);
            if (algorithm == null)
            {
                throw new ArgumentException(string.Format("Algorithm of type {0} is not a known symmetric encryption algorithm", symmetricAlgorithmName));
            }
            return algorithm;
        }

       SymmetricAlgorithm CreateAlgorithm(Type symmetricAlgorithmType)
        {
            SymmetricAlgorithm algorithm = null;
            try
            {
                algorithm = (SymmetricAlgorithm) Activator.CreateInstance(symmetricAlgorithmType);
            }
            catch (Exception)
            {
                throw new ArgumentException("Algorithm of type {0} is not a known symmetric encryption algorithm", symmetricAlgorithmType.Name);
            }
            return algorithm;
        }

        public byte[] GenerateKey(string symmetricAlgorithmName)
        {
            using (SymmetricAlgorithm algorithm = this.CreateAlgorithm(symmetricAlgorithmName))
            {
                return this.GenerateUnprotectedKey(algorithm);
            }
        }

        public byte[] GenerateKey(Type symmetricAlgorithmType)
        {
            using (SymmetricAlgorithm algorithm = this.CreateAlgorithm(symmetricAlgorithmType))
            {
                return this.GenerateUnprotectedKey(algorithm);
            }
        }

       ProtectedKey GenerateKey(SymmetricAlgorithm algorithm, DataProtectionScope dataProtectionScope)
        {
            ProtectedKey key;
            byte[] plaintextKey = this.GenerateUnprotectedKey(algorithm);
            try
            {
                key = ProtectedKey.CreateFromPlaintextKey(plaintextKey, dataProtectionScope);
            }
            finally
            {
                if (plaintextKey != null)
                {
                    CryptographyUtility.ZeroOutBytes(plaintextKey);
                }
            }
            return key;
        }

        public ProtectedKey GenerateKey(string symmetricAlgorithmName, DataProtectionScope dataProtectionScope)
        {
            SymmetricAlgorithm algorithm = this.CreateAlgorithm(symmetricAlgorithmName);
            return this.GenerateKey(algorithm, dataProtectionScope);
        }

        public ProtectedKey GenerateKey(Type symmetricAlgorithmType, DataProtectionScope dataProtectionScope)
        {
            SymmetricAlgorithm algorithm = this.CreateAlgorithm(symmetricAlgorithmType);
            return this.GenerateKey(algorithm, dataProtectionScope);
        }

       byte[] GenerateUnprotectedKey(SymmetricAlgorithm algorithm)
        {
            using (algorithm)
            {
                algorithm.GenerateKey();
                return algorithm.Key;
            }
        }
    }
}

