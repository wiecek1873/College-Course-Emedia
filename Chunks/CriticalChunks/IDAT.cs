namespace EmediaWPF
{
    internal class IDAT : Chunk
    {
        public IDAT(char[] type, uint length, byte[] data, byte[] crc) : base(type, length, data, crc)
        {
        }
    }
}