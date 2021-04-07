using System;

namespace EmediaPng
{
    public abstract class Chunk
    {
        public Chunk(byte[] data, byte[] crc)
        {

        }

        public static Chunk Create(char[] type, byte[] data, byte[] crc)
        {
            string sType = new string(type);
            switch (sType)
            {
                case "IHDR": return new IHDR(data, crc);
                default: ThrowIfCritical(type); break;
            }
            return null;
        }

        public static bool IsCritical(char[] type) => char.IsUpper(type[0]);
        public static bool IsPublic(char[] type) => char.IsUpper(type[1]);
        public static bool SafeToCopy(char[] type) => char.IsLower(type[3]);

        private static void ThrowIfCritical(char[] type)
        {
            if (IsCritical(type))
                throw new NotImplementedException(new string(type));
        }
    }

    // public class PLTE : Chunk {}
    // public class IDAT : Chunk {}
    // public class IEND : Chunk {}
    // public class tIME : Chunk {}
    // public class gAMA : Chunk {}
    // public class cHRM : Chunk {}
}