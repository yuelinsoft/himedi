namespace Hidistro.Core.Cryptography
{
    using System;
    using System.IO;
    using System.Security.Cryptography;

    public class KeyReaderWriter : IKeyReader, IKeyWriter
    {
        internal const int versionNumber = 0x10e1;
        internal const int versionNumberLength = 4;

        public void Archive(Stream outputStream, ProtectedKey keyToBeArchived, string passphrase)
        {
            byte[] bytes = BitConverter.GetBytes(0x10e1);
            byte[] salt = this.GenerateSalt();
            byte[] buffer = this.GetEncryptedKey(keyToBeArchived, passphrase, salt);
            outputStream.Write(bytes, 0, bytes.Length);
            outputStream.Write(salt, 0, salt.Length);
            outputStream.Write(buffer, 0, buffer.Length);
        }

       byte[] DecryptKeyForRestore(string passphrase, byte[] encryptedKey, byte[] salt)
        {
            RijndaelManaged archivalEncryptionAlgorithm = new RijndaelManaged();
            byte[] rgbKey = this.GenerateArchivalKey(archivalEncryptionAlgorithm, passphrase, salt);
            byte[] rgbIV = new byte[archivalEncryptionAlgorithm.BlockSize / 8];
            byte[] buffer3 = CryptographyUtility.Transform(archivalEncryptionAlgorithm.CreateDecryptor(rgbKey, rgbIV), encryptedKey);
            CryptographyUtility.ZeroOutBytes(rgbKey);
            return buffer3;
        }

       byte[] EncryptKeyForArchival(byte[] keyToExport, string passphrase, byte[] salt)
        {
            RijndaelManaged archivalEncryptionAlgorithm = new RijndaelManaged();
            byte[] rgbKey = this.GenerateArchivalKey(archivalEncryptionAlgorithm, passphrase, salt);
            byte[] rgbIV = new byte[archivalEncryptionAlgorithm.BlockSize / 8];
            return CryptographyUtility.Transform(archivalEncryptionAlgorithm.CreateEncryptor(rgbKey, rgbIV), keyToExport);
        }

       byte[] GenerateArchivalKey(SymmetricAlgorithm archivalEncryptionAlgorithm, string passphrase, byte[] salt)
        {
            Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(passphrase, salt);
            return bytes.GetBytes(archivalEncryptionAlgorithm.KeySize / 8);
        }

       byte[] GenerateSalt()
        {
            return CryptographyUtility.GetRandomBytes(0x10);
        }

       byte[] GetEncryptedKey(ProtectedKey keyToBeArchived, string passphrase, byte[] salt)
        {
            byte[] buffer2;
            byte[] decryptedKey = keyToBeArchived.DecryptedKey;
            try
            {
                buffer2 = this.EncryptKeyForArchival(decryptedKey, passphrase, salt);
            }
            finally
            {
                CryptographyUtility.ZeroOutBytes(decryptedKey);
            }
            return buffer2;
        }

       ProtectedKey ProtectKey(byte[] decryptedKey, DataProtectionScope protectionScope)
        {
            ProtectedKey key = ProtectedKey.CreateFromPlaintextKey(decryptedKey, protectionScope);
            CryptographyUtility.ZeroOutBytes(decryptedKey);
            return key;
        }

        public ProtectedKey Read(Stream protectedKeyStream, DataProtectionScope protectionScope)
        {
            this.ValidateKeyVersion(protectedKeyStream);
            return ProtectedKey.CreateFromEncryptedKey(this.ReadEncryptedKey(protectedKeyStream), protectionScope);
        }

       byte[] ReadEncryptedKey(Stream protectedKeyStream)
        {
            byte[] buffer = new byte[protectedKeyStream.Length - 4];
            protectedKeyStream.Read(buffer, 0, buffer.Length);
            return buffer;
        }

       uint ReadVersionNumber(Stream protectedKeyStream)
        {
            byte[] buffer = new byte[4];
            protectedKeyStream.Read(buffer, 0, buffer.Length);
            return BitConverter.ToUInt32(buffer, 0);
        }

        public byte[] Restore(Stream protectedKeyStream, string passphrase)
        {
            this.ValidateKeyVersion(protectedKeyStream);
            byte[] buffer = new byte[0x10];
            byte[] buffer2 = new byte[(protectedKeyStream.Length - 4) - buffer.Length];
            protectedKeyStream.Read(buffer, 0, buffer.Length);
            protectedKeyStream.Read(buffer2, 0, buffer2.Length);
            return this.DecryptKeyForRestore(passphrase, buffer2, buffer);
        }

        public ProtectedKey Restore(Stream protectedKeyStream, string passphrase, DataProtectionScope protectionScope)
        {
            return this.ProtectKey(this.Restore(protectedKeyStream, passphrase), protectionScope);
        }

       void ValidateKeyVersion(Stream protectedKeyStream)
        {
            if (this.ReadVersionNumber(protectedKeyStream) != 0x10e1)
            {
                throw new InvalidOperationException("Key versions do not match between encrypted key and decryption algorithm");
            }
        }

        public void Write(Stream outputStream, ProtectedKey key)
        {
            this.WriteVersionNumber(outputStream, 0x10e1);
            this.WriteEncryptedKey(outputStream, key);
        }

       void WriteEncryptedKey(Stream outputStream, ProtectedKey key)
        {
            outputStream.Write(key.EncryptedKey, 0, key.EncryptedKey.Length);
        }

       void WriteVersionNumber(Stream outputStream, int versionNumber)
        {
            byte[] bytes = BitConverter.GetBytes(versionNumber);
            outputStream.Write(bytes, 0, bytes.Length);
        }
    }
}

