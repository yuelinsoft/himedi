namespace Hidistro.Core.Cryptography
{
    using System;

    public interface ISymmetricCryptoProvider
    {
        byte[] Decrypt(byte[] ciphertext);
        byte[] Encrypt(byte[] plaintext);
    }
}

