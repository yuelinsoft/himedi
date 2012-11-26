namespace Hidistro.Core.Cryptography
{
    using System;
    using System.Security.Cryptography;

    public class ProtectedKey
    {
       byte[] protectedKey;
       DataProtectionScope protectionScope;

       ProtectedKey(byte[] protectedKey, DataProtectionScope protectionScope)
        {
            this.protectionScope = protectionScope;
            this.protectedKey = protectedKey;
        }

        public static ProtectedKey CreateFromEncryptedKey(byte[] encryptedKey, DataProtectionScope dataProtectionScope)
        {
            return new ProtectedKey(encryptedKey, dataProtectionScope);
        }

        public static ProtectedKey CreateFromPlaintextKey(byte[] plaintextKey, DataProtectionScope dataProtectionScope)
        {
            return new ProtectedKey(ProtectedData.Protect(plaintextKey, null, dataProtectionScope), dataProtectionScope);
        }

        public virtual byte[] Unprotect()
        {
            return ProtectedData.Unprotect(this.protectedKey, null, this.protectionScope);
        }

        public byte[] DecryptedKey
        {
            get
            {
                return this.Unprotect();
            }
        }

        public byte[] EncryptedKey
        {
            get
            {
                return (byte[]) this.protectedKey.Clone();
            }
        }
    }
}

