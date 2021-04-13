using System.Text;
using System.Collections.Generic;

namespace EmediaWPF
{
    internal class iTXt : Chunk
    {
        private string Text;

        public iTXt(char[] type, uint length, byte[] data, byte[] crc) : base(type, length, data, crc)
        {
            Text = Encoding.UTF8.GetString(data);
        }

        public override string ToString()
        {
            return $"{base.ToString()} | {PrintXml()}";
        }

        private string PrintXml()
        {
            return XmpPrinter.ParseAndPrint(Text);
        }
    }
}