namespace EmediaWPF
{
	internal class sRGB : Chunk
	{
		public byte RenderingIntent { get; private set; }

		public sRGB(char[] type, uint length, byte[] data, byte[] crc) : base(type, length, data, crc)
		{
			RenderingIntent = data[0];
		}

		public override string ToString()
		{
			return $"{base.ToString()} | rendering intent: {RenderingIntent}";
		}
	}
}
