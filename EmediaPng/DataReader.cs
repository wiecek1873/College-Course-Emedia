namespace EmediaPng
{
    public static class DataReader
    {
        public static uint ReadUint32(byte[] bytes)
        {
            uint value = 0;
            foreach (byte b in bytes)
            {
                value <<= 8;
                value += (uint) b;
            }
            return value;
        }
    }
}