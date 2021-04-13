namespace EmediaWPF
{
	internal class cHRM : Chunk
	{
		public uint WhitePointX { get; private set; }
		public uint WhitePointY { get; private set; }
		public uint RedX { get; private set; }
		public uint RedY { get; private set; }
		public uint GreenX { get; private set; }
		public uint GreenY { get; private set; }
		public uint BlueX { get; private set; }
		public uint BlueY { get; private set; }

		public cHRM(char[] type, uint length, byte[] data, byte[] crc) : base(type, length, data, crc)
		{
			WhitePointX = DataReader.ReadUint32(data, 0);
			WhitePointY = DataReader.ReadUint32(data, 4);
			RedX = DataReader.ReadUint32(data, 8);
			RedY = DataReader.ReadUint32(data, 12);
			GreenX = DataReader.ReadUint32(data, 16);
			GreenY = DataReader.ReadUint32(data, 20);
			BlueX = DataReader.ReadUint32(data, 24);
			BlueY = DataReader.ReadUint32(data, 28);
		}

		public override string ToString()
		{
			return $"{base.ToString()} | white point x: {WhitePointX} | white point y: {WhitePointY} | red x: {RedX} | red y: {RedY} | green x: {GreenX} | green y: {GreenY} | blue x: {BlueX} | blue y: {BlueY}";
		}
	}
}
