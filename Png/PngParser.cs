using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace EmediaWPF
{
	public class PngParser : IDisposable
	{
		private byte[] PngSignature = { 137, 80, 78, 71, 13, 10, 26, 10 };
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
			List<Chunk> IDATs = chunks.FindAll(chunk => new string(chunk.type) == "IDAT");
			IEND IEND = chunks.Find(chunk => new string(chunk.type) == "IEND") as IEND;

			//Signature
			writer.Write(PngSignature);
			//IHDR
			writer.Write(BitConverter.GetBytes(IHDR.length).Reverse().ToArray());
			writer.Write(IHDR.type);
			writer.Write(IHDR.data);
			writer.Write(IHDR.crc);

			//PLTE
			if (PLTE != null)
			{
				writer.Write(BitConverter.GetBytes(PLTE.length).Reverse().ToArray());
				writer.Write(PLTE.type);
				writer.Write(PLTE.data);
				writer.Write(PLTE.crc);
			}

			//IDAT
			foreach (IDAT data in IDATs)
			{
				writer.Write(BitConverter.GetBytes(data.length).Reverse().ToArray());
				writer.Write(data.type);
				writer.Write(data.data);
				writer.Write(data.crc);
			}

			//IEND
			writer.Write(BitConverter.GetBytes(IEND.length).Reverse().ToArray());
			writer.Write(IEND.type);
			writer.Write(IEND.data);
			writer.Write(IEND.crc);

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
					writer.Write(BitConverter.GetBytes(chunk.length).Reverse().ToArray());
					writer.Write(chunk.type);
					writer.Write(chunk.data);
					writer.Write(chunk.crc);
				}
			}
			writer.Close();
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
	}
}