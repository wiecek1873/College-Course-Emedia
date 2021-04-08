namespace EmediaPng
{
    internal class IDAT : Chunk
    {
        public IDAT(char[] type, uint length, byte[] data, byte[] crc) : base(type, length, data, crc)
        {
        }
    }

    // public class PLTE : Chunk {}
    // public class IDAT : Chunk {}
    // public class IEND : Chunk {}
    // public class tIME : Chunk {}
    // public class gAMA : Chunk {}
    // public class cHRM : Chunk {}
}