namespace Hidistro.Core.Cryptography
{
    using System;
    using System.IO;

    public interface IKeyWriter
    {
        void Archive(Stream outputStream, ProtectedKey key, string passphrase);
        void Write(Stream outputStream, ProtectedKey key);
    }
}

