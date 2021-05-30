using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmediaWPF
{
    public static class BinaryWriterExtensions
    {
        public static byte[] UlongToByteArray(ulong number)
        {
            return BitConverter.GetBytes(number).Reverse().ToArray();
        }

        public static void WriteUlong(this BinaryWriter writer, ulong number)
        {
            writer.Write(UlongToByteArray(number));
        }

        public static void WriteChunk(this BinaryWriter writer, Chunk chunk)
        {
            writer.Write(BitConverter.GetBytes(chunk.length).Reverse().ToArray());
            writer.Write(chunk.type);
            writer.Write(chunk.data);
            writer.Write(chunk.crc);
        }
    }

    public static class MemoryStreamExtensions
    {
        public static void WriteUlong(this MemoryStream memory, ulong number)
        {
            var bytes = BinaryWriterExtensions.UlongToByteArray(number);
            memory.Write(bytes, 0, bytes.Length);
        }

        public static void WriteChunk(this MemoryStream memory, Chunk chunk)
        {
            memory.WriteUlong(chunk.length);
            memory.Write(new byte[]{ (byte)chunk.type[0], (byte)chunk.type[1], (byte)chunk.type[2], (byte)chunk.type[3]}, 0, (int)chunk.length);
            memory.Write(chunk.data, 0, 4);
            memory.Write(chunk.crc, 0, 4);
        }
    }
}
