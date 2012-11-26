namespace Hidistro.Core.Cryptography
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    public static class CryptographyUtility
    {
        public static byte[] CombineBytes(byte[] buffer1, byte[] buffer2)
        {
            byte[] dst = new byte[buffer1.Length + buffer2.Length];
            Buffer.BlockCopy(buffer1, 0, dst, 0, buffer1.Length);
            Buffer.BlockCopy(buffer2, 0, dst, buffer1.Length, buffer2.Length);
            return dst;
        }

        public static bool CompareBytes(byte[] byte1, byte[] byte2)
        {
            if ((byte1 == null) || (byte2 == null))
            {
                return false;
            }
            if (byte1.Length != byte2.Length)
            {
                return false;
            }
            for (int i = 0; i < byte1.Length; i++)
            {
                if (byte1[i] != byte2[i])
                {
                    return false;
                }
            }
            return true;
        }

        public static byte[] GetBytesFromHexString(string hexidecimalNumber)
        {
            StringBuilder builder = new StringBuilder(hexidecimalNumber.ToUpper(CultureInfo.CurrentCulture));
            char ch = builder[0];
            if (ch.Equals('0') && (ch = builder[1]).Equals('X'))
            {
                builder.Remove(0, 2);
            }
            if ((builder.Length % 2) != 0)
            {
                throw new ArgumentException("String must represent a valid hexadecimal (e.g. : 0F99DD)");
            }
            byte[] buffer = new byte[builder.Length / 2];
            try
            {
                for (int i = 0; i < buffer.Length; i++)
                {
                    int startIndex = i * 2;
                    buffer[i] = Convert.ToByte(builder.ToString(startIndex, 2), 0x10);
                }
            }
            catch (FormatException exception)
            {
                throw new ArgumentException("String must represent a valid hexadecimal (e.g. : 0F99DD)", exception);
            }
            return buffer;
        }

        public static string GetHexStringFromBytes(byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException("bytes");
            }
            if (bytes.Length == 0)
            {
                throw new ArgumentException("The value must be greater than 0 bytes.", "bytes");
            }
            StringBuilder builder = new StringBuilder(bytes.Length * 2);
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("X2", CultureInfo.InvariantCulture));
            }
            return builder.ToString();
        }

        public static byte[] GetRandomBytes(int size)
        {
            byte[] bytes = new byte[size];
            GetRandomBytes(bytes);
            return bytes;
        }

        public static void GetRandomBytes(byte[] bytes)
        {
            RandomNumberGenerator.Create().GetBytes(bytes);
        }

        public static byte[] Transform(ICryptoTransform transform, byte[] buffer)
        {
            byte[] buffer2 = null;
            using (MemoryStream stream = new MemoryStream())
            {
                CryptoStream stream2 = null;
                try
                {
                    stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Write);
                    stream2.Write(buffer, 0, buffer.Length);
                    stream2.FlushFinalBlock();
                    buffer2 = stream.ToArray();
                }
                finally
                {
                    if (stream2 != null)
                    {
                        stream2.Close();
                        stream2.Dispose();
                    }
                }
            }
            return buffer2;
        }

        public static void ZeroOutBytes(byte[] bytes)
        {
            if (bytes != null)
            {
                Array.Clear(bytes, 0, bytes.Length);
            }
        }
    }
}

