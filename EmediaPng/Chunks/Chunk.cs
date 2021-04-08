using System;

namespace EmediaPng
{
    public abstract class Chunk
    {
        private char[] type;
        private uint length;
        private byte[] data;
        private byte[] crc;

        public Chunk(char[] type, uint length, byte[] data, byte[] crc)
        {
            this.type = type;
            this.length = length;
            this.data = data;
            this.crc = crc;
        }

        public override string ToString() => $"{new string(type)} | {length}";

        public static Chunk Create(char[] type, uint length, byte[] data, byte[] crc)
        {
            string sType = new string(type);
            switch (sType)
            {
                case "IHDR": return new IHDR(type, length, data, crc);
                case "IDAT": return new IDAT(type, length, data, crc);
                case "PLTE": return new PLTE(type, length, data, crc);
                case "IEND": return new IEND(type, length, data, crc);

                default:
                    if (IsCritical(type))
                        throw new NotImplementedException(sType);
                    else
                        return new NotSupportedChunk(type, length, data, crc);
            }
        }

        public bool IsCritical() => IsCritical(type);
        public bool IsPublic() => IsPublic(type);
        public bool IsSafeToCopy() => IsSafeToCopy(type);

        public static bool IsCritical(char[] type) => char.IsUpper(type[0]);
        public static bool IsPublic(char[] type) => char.IsUpper(type[1]);
        public static bool IsSafeToCopy(char[] type) => char.IsLower(type[3]);

        private static void ThrowIfCritical(char[] type)
        {
            if (IsCritical(type))
                throw new NotImplementedException(new string(type));
        }
    }

    // todo
    // public class PLTE : Chunk {}
    // public class IDAT : Chunk {}
    // public class IEND : Chunk {}
    // public class tIME : Chunk {}
    // public class gAMA : Chunk {}
    // public class cHRM : Chunk {}
}