using System;

namespace EmediaPng
{
    public abstract class Chunk
    {
        private char[] type;

        public Chunk(char[] type, byte[] data, byte[] crc)
        {

        }

        public override string ToString() => new string(type);

        public static Chunk Create(char[] type, byte[] data, byte[] crc)
        {
            string sType = new string(type);
            switch (sType)
            {
                case "IHDR": return new IHDR(type, data, crc);

                default:
                    if (IsCritical(type))
                        throw new NotImplementedException(sType);
                    else
                        return new NotSupportedChunk(type, data, crc);
            }
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