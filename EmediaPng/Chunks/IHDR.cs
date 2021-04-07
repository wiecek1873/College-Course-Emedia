namespace EmediaPng
{
    public class IHDR : Chunk
    {
        public IHDR(char[] type, byte[] data, byte[] crc) : base(type, data, crc)
        {
        }
    }
}