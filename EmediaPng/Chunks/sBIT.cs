namespace EmediaPng
{
	internal class sBIT : Chunk
	{
		public byte[] SignificantBits { get; private set; }

		public sBIT(char[] type, uint length, byte[] data, byte[] crc) : base(type, length, data, crc)
		{
			SignificantBits = new byte[length];
			for (int i = 0; i < length; i++)
			{
				SignificantBits[i] = data[i];
			}
		}

		public override string ToString()
		{
			return $"{base.ToString()} | significant bits: {string.Join(" ",SignificantBits)}";
		}
	}
}
