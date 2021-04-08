namespace EmediaPng
{
    internal class IEND : Chunk
    {
        public IEND(char[] type, uint length, byte[] data, byte[] crc) : base(type, length, data, crc)
        {
        }
    }
}