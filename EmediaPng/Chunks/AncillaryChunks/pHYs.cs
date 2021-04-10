namespace EmediaPng
{
	internal class pHYs : Chunk
	{
		public uint PixelsPerUnitXAxis { get; private set; }
		public uint PixelsPerUnitYAxis { get; private set; }
		public byte UnitSpecifier { get; private set; }

		public pHYs(char[] type, uint length, byte[] data, byte[] crc) : base(type, length, data, crc)
		{
			PixelsPerUnitXAxis = DataReader.ReadUint32(data, 0);
			PixelsPerUnitYAxis = DataReader.ReadUint32(data, 4);
			UnitSpecifier = data[8];
		}

		public override string ToString()
		{
			return $"{base.ToString()} | pixels per unit X axis: {PixelsPerUnitXAxis} | pixels per unit Y axis: {PixelsPerUnitYAxis} | unit: {UnitSpecifier}";
		}
	}
}
