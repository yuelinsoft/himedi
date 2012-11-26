namespace Hidistro.Core.Cryptography
{
    using System;
    using System.IO;
    using System.Security.Cryptography;

    public interface IKeyReader
    {
        ProtectedKey Read(Stream protectedKeyContents, DataProtectionScope protectionScope);
        byte[] Restore(Stream protectedKeyContents, string passphrase);
        ProtectedKey Restore(Stream protectedKeyContents, string passphrase, DataProtectionScope protectionScope);
    }
}

