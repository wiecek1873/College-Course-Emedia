namespace EmediaPng
{
	internal class PLTE : Chunk
	{
		public class Palette
		{
			public byte Red;
			public byte Green;
			public byte Blue;

			public Palette(byte red, byte green, byte blue)
			{
				Red = red;
				Green = green;
				Blue = blue;
			}
		}

		public Palette[] Palettes { get; private set; }

		public PLTE(char[] type, uint length, byte[] data, byte[] crc) : base(type, length, data, crc)
		{
			Palettes = new Palette[length / 3];

			int index = 0;
			for (int i = 0; i < Palettes.Length; i += 3)
			{
				Palettes[index] = new Palette(data[i], data[i + 1], data[i + 2]);
				index++;
			}
		}

		public override string ToString()
		{
			return $"{base.ToString()} | pallets count: {Palettes.Length}";
		}
	}
}