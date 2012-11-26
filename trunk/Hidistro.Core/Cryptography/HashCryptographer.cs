namespace Hidistro.Core.Cryptography
{
    using System;
    using System.Security.Cryptography;

    public class HashCryptographer
    {
       Type algorithmType;
       ProtectedKey key;

        public HashCryptographer(Type algorithmType)
        {
            if (algorithmType == null)
            {
                throw new ArgumentNullException("algorithmType");
            }
            if (!typeof(HashAlgorithm).IsAssignableFrom(algorithmType))
            {
                throw new ArgumentException("The type must be of type HashAlgorithm.", "algorithmType");
            }
            this.algorithmType = algorithmType;
        }

        public HashCryptographer(Type algorithmType, ProtectedKey protectedKey) : this(algorithmType)
        {
            this.key = protectedKey;
        }

        public byte[] ComputeHash(byte[] plaintext)
        {
            using (HashAlgorithm algorithm = this.GetHashAlgorithm())
            {
                return algorithm.ComputeHash(plaintext);
            }
        }

       HashAlgorithm GetHashAlgorithm()
        {
            HashAlgorithm algorithm = Activator.CreateInstance(this.algorithmType, true) as HashAlgorithm;
            KeyedHashAlgorithm algorithm2 = algorithm as KeyedHashAlgorithm;
            if ((algorithm2 != null) && (this.key != null))
            {
                algorithm2.Key = this.key.DecryptedKey;
            }
            return algorithm;
        }
    }
}

