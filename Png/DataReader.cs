namespace EmediaWPF
{
    public static class DataReader
    {
        public static uint ReadUint32(byte[] bytes, int startIndex = 0)
        {
            uint value = 0;
            for (int i = startIndex; i < startIndex + 4; ++i)
            {
                value <<= 8;
                value += (uint)bytes[i];
            }
            return value;
        }
    }
}