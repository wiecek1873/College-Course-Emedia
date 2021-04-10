namespace EmediaPng
{
    internal class NotSupportedChunk : Chunk
    {
        public NotSupportedChunk(char[] type, uint length, byte[] data, byte[] crc) : base(type, length, data, crc)
        {
        }

        public override string ToString()
        {
            return $"{base.ToString()} | NotSupportedChunk";
        }
    }
}