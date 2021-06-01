using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using Force.Crc32;

namespace EmediaWPF
{
	public class PngParser : IDisposable
	{
		private static readonly byte[] PngSignature = { 137, 80, 78, 71, 13, 10, 26, 10 };
		private readonly List<Chunk> chunks = new List<Chunk>();
		private const int TypeLength = 4;
		private const int CRCLength = 4;
		private BinaryReader fileReader;

		public PngParser(string filePath)
		{
			fileReader = new BinaryReader(File.Open(filePath, FileMode.Open));
			AssertPng();
			ReadChunks();
			fileReader.Close();
		}

		public void PrintChunks()
		{
			foreach (var chunk in chunks)
			{
				Console.WriteLine(chunk);
			}
		}

		public void Dispose()
		{
			fileReader.Dispose();
		}

		public void SaveCriticalOnly(string path, string fileName)
		{
			BinaryWriter writer = new BinaryWriter(File.Open(path + fileName, FileMode.Create));

			IHDR IHDR = chunks.Find(chunk => new string(chunk.type) == "IHDR") as IHDR;
			PLTE PLTE = chunks.Find(chunk => new string(chunk.type) == "PLTE") as PLTE;
			List<Chunk> IDATs = GetIDATChunks();
			IEND IEND = chunks.Find(chunk => new string(chunk.type) == "IEND") as IEND;

			writer.Write(PngSignature);

			writer.WriteChunk(IHDR);

			if (PLTE != null)
			{
				writer.WriteChunk(PLTE);
			}

			foreach (IDAT data in IDATs)
			{
				writer.WriteChunk(data);
			}

			writer.WriteChunk(IEND);

			writer.Close();
		}

		public void Save(string path, string fileName)
		{
			BinaryWriter writer = new BinaryWriter(File.Open(path + fileName, FileMode.Create));
			writer.Write(PngSignature);
			foreach (Chunk chunk in chunks)
			{
				writer.WriteChunk(chunk);
			}
			writer.Close();
		}

        public void SaveWithoutMetadata(string path, string fileName)
		{
			BinaryWriter writer = new BinaryWriter(File.Open(path + fileName, FileMode.Create));
			writer.Write(PngSignature);
			foreach (Chunk chunk in chunks)
			{
				if (new string(chunk.type) == "pHYs" || new string(chunk.type) == "tIME" || new string(chunk.type) == "tEXt" || new string(chunk.type) == "iTXt")
					continue;
				else
				{
					writer.WriteChunk(chunk);
				}
			}
			writer.Close();
		}

		public void EncryptIDAT_ECB()
		{
			List<Chunk> idats = GetIDATChunks();
			List<ulong> idataLengths = new List<ulong>();

			foreach (var chunk in idats)
			{
				idataLengths.Add(chunk.length);
				chunk.data = DataEncryption.Instance.EncryptDataECB(chunk.data);
				chunk.length = (uint)chunk.data.Length;
			}

			var tEXt = iTXt.Create(string.Join(",", idataLengths.Select((c) => c.ToString())));
            chunks.Insert(chunks.Count-2, tEXt);
		}

		public void DecryptIDAT_ECB()
		{
			List<Chunk> idats = GetIDATChunks();
			var tEXt = chunks.Last((c) => c is iTXt) as iTXt;
			List<uint> idataLength = new List<uint>(tEXt.Text.Split(",").Select((l) => uint.Parse(l)));

			int i = 0;
			foreach (var chunk in idats)
			{
				chunk.data = DataEncryption.Instance.DecryptDataECB(chunk.data);
				Array.Resize<byte>(ref chunk.data, (int)idataLength[i]);
				chunk.length = (uint)chunk.data.Length;
				++i;
			}

			chunks.Remove(tEXt);
		}

		public void EncryptIDAT_CBC()
		{
			List<Chunk> idats = GetIDATChunks();
			List<ulong> idataLengths = new List<ulong>();

			foreach (var chunk in idats)
			{
				idataLengths.Add(chunk.length);
				chunk.data = DataEncryption.Instance.EncryptDataCBC(chunk.data);
				chunk.length = (uint)chunk.data.Length;
			}

			var tEXt = iTXt.Create(string.Join(",", idataLengths.Select((c) => c.ToString())));
            chunks.Insert(chunks.Count-2, tEXt);
		}

		public void DecryptIDAT_CBC()
		{
			List<Chunk> idats = GetIDATChunks();
			var tEXt = chunks.Last((c) => c is iTXt) as iTXt;
			List<uint> idataLength = new List<uint>(tEXt.Text.Split(",").Select((l) => uint.Parse(l)));

			int i = 0;
			foreach (var chunk in idats)
			{
				chunk.data = DataEncryption.Instance.DecryptDataCBC(chunk.data);
				Array.Resize<byte>(ref chunk.data, (int)idataLength[i]);
				chunk.length = (uint)chunk.data.Length;
				++i;
			}

			chunks.Remove(tEXt);
		}

        internal void Rewrite(string fileName)
        {
			var ihdr = chunks.Where((c) => c is IHDR).First() as IHDR;
			int width = (int)ihdr.Width;

			Color color = Color.Aqua;
			List<byte> idataData = new List<byte>();
			foreach (var idat in GetIDATChunks())
			{
				idataData.AddRange(idat.data);
			}

			int height = idataData.Count / width;

			using (Bitmap b = new Bitmap(width, height)) {
				for (int y = 0; y < height; y++)
				{
					for (int x = 0; x < width; x++)
					{
						int place = (y*height + x) % idataData.Count;
                        b.SetPixel(x, y, Color.FromArgb(idataData[place], idataData[place], idataData[place]));
					}
				}
				b.Save(fileName, ImageFormat.Png);
			}
        }

		internal void Rewrite2()
        {
			List<Chunk> idats = GetIDATChunks();
			var tEXt = chunks.Last((c) => c is iTXt) as iTXt;
			List<uint> idataLength = new List<uint>(tEXt.Text.Split(",").Select((l) => uint.Parse(l)));

			int i = 0;
			foreach (var chunk in idats)
			{
				Array.Resize<byte>(ref chunk.data, (int)idataLength[i]);
				chunk.length = (uint)chunk.data.Length;
				chunk.crc = CheckCrc32(chunk);
				++i;
			}

			chunks.Remove(tEXt);
        }

		private void AssertPng()
		{
			byte[] signature = new byte[8];
			for (int i = 0; i < 8; i++)
			{
				signature[i] = fileReader.ReadByte();
			}
			if (!Enumerable.SequenceEqual(PngSignature, signature))
				throw new FileIsNotPngFile();
		}

		private void ReadChunks()
		{
			while (true)
			{
				uint length = ReadUint32();
				char[] type = fileReader.ReadChars(TypeLength);
				byte[] data = ReadDataOfChunk(length);
				byte[] crc = fileReader.ReadBytes(CRCLength);

				Chunk newChunk = Chunk.Create(type, length, data, crc);
				chunks.Add(newChunk);

				if (newChunk is IEND)
					break;
			}
		}

		private List<Chunk> GetIDATChunks()
		{
			return chunks.FindAll(chunk => chunk is IDAT);
		}

		private List<Chunk> GetChunks(Type type)
		{
			return chunks.FindAll(chunk => chunk.GetType() == type);
		}

		private Chunk GetChunk(Type type)
		{
			return chunks.Find(chunk => chunk.GetType() == type);
		}

		private void InsertChunks(List<Chunk> newChunks)
		{
			chunks.InsertRange(chunks.Count - 2, newChunks);
		}

		private void DeleteChunksOfType(string type)
		{
			chunks.RemoveAll(chunk => new string(chunk.type) == type);
		}

		private void DeleteChunksOfType(Type type)
		{
			chunks.RemoveAll(chunk => chunk.GetType() == type);
		}

		private byte[] ReadDataOfChunk(uint length)
		{
			byte[] output = new byte[length];
			for (uint i = 0; i < length; ++i)
				output[i] = fileReader.ReadByte();
			return output;
		}

		private uint ReadUint32()
		{
			return DataReader.ReadUint32(fileReader.ReadBytes(4));
		}

		private byte[] CheckCrc32(Chunk chunk)
		{
			var inputArray = new byte[chunk.type.Length + chunk.data.Length];
			Array.Copy(chunk.type.Select((c) => (byte)c).ToArray(), inputArray, chunk.type.Length);
			Array.Copy(chunk.data, 0, inputArray, chunk.type.Length, chunk.data.Length);
			var crc32 = Crc32Algorithm.Compute(inputArray);

			return BitConverter.GetBytes(crc32).Reverse().ToArray();
		}
	}
}