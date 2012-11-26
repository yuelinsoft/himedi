namespace Hidistro.Core.Cryptography
{
    using System;
    using System.Security.Cryptography;

    internal interface IKeyGenerator
    {
        byte[] GenerateKey(string algorithmName);
        byte[] GenerateKey(Type algorithmType);
        ProtectedKey GenerateKey(string algorithmName, DataProtectionScope dataProtectionScope);
        ProtectedKey GenerateKey(Type algorithmType, DataProtectionScope dataProtectionScope);
    }
}

