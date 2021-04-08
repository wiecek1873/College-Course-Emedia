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
                uint length = ReadUint32();
                char[] type = fileReader.ReadChars(TypeLength);
                byte[] data = ReadDataOfChunk(length);
                byte[] crc = fileReader.ReadBytes(CRCLength);

                Chunk newChunk = Chunk.Create(type, length, data, crc);
                chunks.Add(newChunk);
                Console.WriteLine(newChunk);

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