namespace EmediaPng
{
	internal class IHDR : Chunk
	{
		public uint Width { get; private set; }
		public uint Height { get; private set; }
		public byte ColorType { get; private set; }
		public byte BitDepth { get; private set; }
		public byte CompressionMethod { get; private set; }
		public byte FilterMethod { get; private set; }
		public byte InterlanceMethod { get; private set; }

		public IHDR(char[] type, uint length, byte[] data, byte[] crc) : base(type, length, data, crc)
		{
			Width = DataReader.ReadUint32(data, 0);
			Height = DataReader.ReadUint32(data, 4);
			BitDepth = data[8];
			ColorType = data[9];
			CompressionMethod = data[10];
			FilterMethod = data[11];
			InterlanceMethod = data[12];
		}

		public override string ToString()
		{
			return $"{base.ToString()} | size: {Width} x {Height} | bit depth: {BitDepth} | color type: {ColorType} | compression: {CompressionMethod} | filter: {FilterMethod} | interlance: {InterlanceMethod}";
		}
	}
}