using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace EmediaPng
{
    public class PngParser : IDisposable
    {
        private readonly List<Chunk> chunks = new List<Chunk>();
        private const int TypeLength = 4;
        private const int CRCLength = 4;
        private BinaryReader fileReader;

        public PngParser(string filePath)
        {
            fileReader = new BinaryReader(File.Open(filePath, FileMode.Open));
            AssetPng();
            ReadChunks();
            // todo dalsze przetwarzanie
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
            fileReader.Close(); // w ostateczności
            fileReader.Dispose();
        }

        private void AssetPng()
		{
            byte[] PngSignature = { 137, 80, 78, 71, 13, 10, 26, 10 };
            byte[] signature = new byte[8];
            for(int i = 0; i < 8; i++)
            {
                signature[i] = fileReader.ReadByte();
            }
            if (! Enumerable.SequenceEqual(PngSignature, signature))
                throw new FileIsNotPngFile();
		}

        private void ReadChunks()
        {
            while (true)
            {
                uint length = ReadLengthOfChunk();
                char[] type = fileReader.ReadChars(TypeLength);
                byte[] data = ReadDataOfChunk(length);
                byte[] crc = fileReader.ReadBytes(CRCLength);

                Chunk newChunk = Chunk.Create(type, data, crc);
                chunks.Add(newChunk);

                if (Enumerable.SequenceEqual<char>(type, @"IEND"))
                    break;
            }
        }

        private byte[] ReadDataOfChunk(uint length)
        {
            if (length > Int32.MaxValue)
                throw new NotImplementedException();
            return fileReader.ReadBytes((int) length);
        }

        private uint ReadLengthOfChunk()
        {
            byte[] array = fileReader.ReadBytes(4);
            uint value = 0;
            int shift = 4;
            foreach (byte a in array)
            {
                value += ((uint) a) >> --shift;
            }
            return value;
        }
    }
}