using System.Text;

namespace EmediaPng
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
            return $"{base.ToString()} | UTF-8 text: {Text}";
        }
    }
}