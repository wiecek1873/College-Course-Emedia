namespace EmediaPng
{
    internal class PLTE : Chunk
    {
        public PLTE(char[] type, uint length, byte[] data, byte[] crc) : base(type, length, data, crc)
        {
        }
    }
}