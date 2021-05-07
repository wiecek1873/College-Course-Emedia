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
        public static void WriteUlong(this BinaryWriter writer, ulong number)
        {
            writer.Write(BitConverter.GetBytes(number).Reverse().ToArray());
        }
    }
}
