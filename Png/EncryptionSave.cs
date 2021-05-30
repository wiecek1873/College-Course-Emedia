using System;
using System.Numerics;

namespace EmediaWPF
{
    [Serializable]
    public class EncryptionSave
    {
        public byte[] d { get; set; }
        public byte[] e { get; set; }
        public byte[] n { get; set; }
    }
}
