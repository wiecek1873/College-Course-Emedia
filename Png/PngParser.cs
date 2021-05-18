using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

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

        public void SaveAndEncrypt(string path, string fileName)
        {
            EncryptMetadata();
            EncryptIDAT();
            Save(path, fileName);
        }

        private void EncryptMetadata()
        {
            foreach (var chunk in chunks)
            {
                if (! chunk.IsCritical())
                {
                    if (chunk.SizeInMemory > DataEncryption.Instance.KeyLength)
                    {
                        chunk.data = DataEncryption.Instance.EncryptData(chunk.data);
                    }
                    else
                    {
                        throw new NotImplementedException(); // todo szyfrowanie wiekszych chunkow
                    }
                }
            }
        }

        private void EncryptIDAT()
        {
            List<Chunk> idats = GetIDATChunks();

            foreach (var chunk in idats)
            {
                if (chunk.SizeInMemory > DataEncryption.Instance.KeyLength)
                {
                    chunk.data = DataEncryption.Instance.EncryptData(chunk.data);
                }
                else
                {
                    throw new NotImplementedException(); // todo szyfrowanie wiekszych chunkow
                }
            }

            DeleteChunksOfType(typeof(IDAT));
            InsertChunks(idats);
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
            chunks.InsertRange(chunks.Count-2, newChunks);
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
    }
}