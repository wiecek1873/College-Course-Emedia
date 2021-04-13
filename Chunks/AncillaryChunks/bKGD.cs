namespace EmediaWPF
{
	internal class bKGD : Chunk
	{
		public byte[] BackgroundColor { get; private set; }

		public bKGD(char[] type, uint length, byte[] data, byte[] crc) : base(type, length, data, crc)
		{
			BackgroundColor = new byte[length];
			for (int i = 0; i < length; i++)
			{
				BackgroundColor[i] = data[i];
			}
		}

		public override string ToString()
		{
			return $"{base.ToString()} | background color: {string.Join(" ", BackgroundColor)}";
		}
	}
}
