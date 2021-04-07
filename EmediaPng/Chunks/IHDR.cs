namespace EmediaPng
{
    public class IHDR : Chunk
    {
        public IHDR(byte[] data, byte[] crc) : base(data, crc)
        {
        }
    }
}