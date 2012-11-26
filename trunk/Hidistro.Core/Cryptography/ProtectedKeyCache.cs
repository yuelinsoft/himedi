namespace Hidistro.Core.Cryptography
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public class ProtectedKeyCache
    {
       readonly Dictionary<string, ProtectedKey> cache = new Dictionary<string, ProtectedKey>();

        public void Clear()
        {
            lock (this.cache)
            {
                this.cache.Clear();
            }
        }

        public ProtectedKey this[string keyFileName]
        {
            get
            {
                if (string.IsNullOrEmpty(keyFileName))
                {
                    throw new ArgumentException("keyFileName");
                }
                lock (this.cache)
                {
                    return (this.cache.ContainsKey(keyFileName) ? this.cache[keyFileName] : null);
                }
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                if (string.IsNullOrEmpty(keyFileName))
                {
                    throw new ArgumentException("keyFileName");
                }
                lock (this.cache)
                {
                    this.cache[keyFileName] = value;
                }
            }
        }
    }
}

