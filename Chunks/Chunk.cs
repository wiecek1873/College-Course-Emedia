using System;
using System.IO;

namespace EmediaWPF
{
	public abstract class Chunk
	{
		public uint length;
		public char[] type;
		public byte[] data;
		public byte[] crc;

        public uint SizeInMemory { get => length + (uint) 12; }

        public Chunk(char[] type, uint length, byte[] data, byte[] crc)
		{
			this.length = length;
			this.type = type;
			this.data = data;
			this.crc = crc;
		}

		public override string ToString() => $"{new string(type)} | {length}";

		public bool IsCritical() => IsCritical(type);
		public bool IsPublic() => IsPublic(type);
		public bool IsSafeToCopy() => IsSafeToCopy(type);

		public byte[] ToArray()
		{
			using (MemoryStream memory = new MemoryStream())
			{
				memory.WriteChunk(this);
				return memory.ToArray();
			}
		}

		// public static implicit operator byte[] (Chunk chunk) => chunk.ToArray();

		public static bool IsCritical(char[] type) => char.IsUpper(type[0]);
		public static bool IsPublic(char[] type) => char.IsUpper(type[1]);
		public static bool IsSafeToCopy(char[] type) => char.IsLower(type[3]);

		public static Chunk Create(char[] type, uint length, byte[] data, byte[] crc)
		{
			string sType = new string(type);
			switch (sType)
			{
				case "IHDR": return new IHDR(type, length, data, crc);
				case "IDAT": return new IDAT(type, length, data, crc);
				case "PLTE": return new PLTE(type, length, data, crc);
				case "IEND": return new IEND(type, length, data, crc);

				case "gAMA": return new gAMA(type, length, data, crc);
				case "sRGB": return new sRGB(type, length, data, crc);
				case "pHYs": return new pHYs(type, length, data, crc);
				case "sBIT": return new sBIT(type, length, data, crc);
				case "cHRM": return new cHRM(type, length, data, crc);
				case "bKGD": return new bKGD(type, length, data, crc);
				case "tIME": return new tIME(type, length, data, crc);
				case "tEXt": return new tEXt(type, length, data, crc);
                case "iTXt": return new iTXt(type, length, data, crc);

				default:
					if (IsCritical(type))
						throw new NotImplementedException(sType);
					else
						return new NotSupportedChunk(type, length, data, crc);
			}
		}

		private static void ThrowIfCritical(char[] type)
		{
			if (IsCritical(type))
				throw new NotImplementedException(new string(type));
		}
    }
}