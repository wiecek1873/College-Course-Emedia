using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace EmediaPng
{
    public class PngParser : IDisposable
    {
        private List<Byte> bytes = new List<byte>();
        private List<IChunk> chunks;
        private BinaryReader fileReader;

        public PngParser(string filePath)
        {
            fileReader = new BinaryReader(File.Open(filePath, FileMode.Open));
        }

        public void DebugPrint()
        {
            try 
            {
                byte a;
                while (true)
                {
                    a = fileReader.ReadByte();
                    bytes.Add(a);
                    Console.Write(a + " ");
                }
            }
            catch (EndOfStreamException)
            {

            }
        }

        public bool IsPngFile()
		{
            byte[] PngSignature = { 137, 80, 78, 71, 13, 10, 26, 10 };
            byte[] signature = new byte[8];
            for(int i = 0; i < 8; i++)
            {
                signature[i] = bytes[i];
            }
            if (Enumerable.SequenceEqual(PngSignature,signature))
                return true;
            else
                return false;
		}

        public void Dispose()
        {
            fileReader.Close(); // w ostatecznosci
            fileReader.Dispose();
        }
    }
}