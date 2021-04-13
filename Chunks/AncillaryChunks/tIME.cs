namespace EmediaWPF
{
	internal class tIME : Chunk
	{
		public uint Year { get; private set; }
		public byte Month { get; private set; }
		public byte Day { get; private set; }
		public byte Hour { get; private set; }
		public byte Minute { get; private set; }
		public byte Second { get; private set; }

		public tIME(char[] type, uint length, byte[] data, byte[] crc) : base(type, length, data, crc)
		{
			Year = (uint)(data[0] << 8) | data[1];
			Month = data[2];
			Day = data[3];
			Hour = data[4];
			Minute = data[5];
			Second = data[6];
		}

		public override string ToString()
		{
			return $"{base.ToString()} | year: {Year} | month: {Month} | day: {Day} | hour: {Hour} | minute: {Minute} | second: {Second}";
		}
	}
}
