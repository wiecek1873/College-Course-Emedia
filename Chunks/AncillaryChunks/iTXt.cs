using System;
using System.Linq;
using System.Text;

namespace EmediaWPF
{
    internal class iTXt : Chunk
    {
        public string Text;

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

		public static iTXt Create(string txt)
		{
			char[] arr = txt.ToCharArray();
			char[] type = {'i', 'T', 'X', 't'};
			uint length = (uint)arr.Length;
			byte[] data = arr.Select((c) => (byte)c).ToArray();
			byte[] crc = BitConverter.GetBytes(length);

			var tEXt = new iTXt(type, length, data, crc);
			return tEXt;
		}
    }
}