using System;
using System.Numerics;

namespace EmediaWPF
{
    [Serializable]
    public class EncryptionSave
    {
        public BigInteger d { get; set; }
        public BigInteger e { get; set; }
        public BigInteger n { get; set; }
    }
}
