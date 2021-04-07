namespace EmediaPng
{
    internal class NotSupportedChunk : Chunk
    {
        public NotSupportedChunk(char[] type, byte[] data, byte[] crc) : base(type, data, crc)
        {
        }
    }
}