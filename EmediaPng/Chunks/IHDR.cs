namespace EmediaPng
{
    internal class IHDR : Chunk
    {
        public IHDR(char[] type, uint length, byte[] data, byte[] crc) : base(type, length, data, crc)
        {
        }
    }
}