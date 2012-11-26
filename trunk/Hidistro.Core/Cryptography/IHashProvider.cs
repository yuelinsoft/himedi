namespace Hidistro.Core.Cryptography
{
    using System;

    public interface IHashProvider
    {
        bool CompareHash(byte[] plaintext, byte[] hashedtext);
        byte[] CreateHash(byte[] plaintext);
    }
}

