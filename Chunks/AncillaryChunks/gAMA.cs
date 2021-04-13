namespace EmediaWPF
{
	internal class gAMA : Chunk
	{
		public uint Gamma { get; private set; }

		public gAMA(char[] type, uint length, byte[] data, byte[] crc) : base(type, length, data, crc)
		{
			Gamma = DataReader.ReadUint32(data, 0);
		}

		public override string ToString()
		{
			return $"{base.ToString()} | gamma: {Gamma}";
		}
	}
}
