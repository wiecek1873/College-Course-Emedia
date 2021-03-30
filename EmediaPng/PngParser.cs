using System;
using System.IO;
using System.Collections.Generic;

namespace EmediaPng
{
    public class PngParser : IDisposable
    {
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
                    Console.Write(a);
                }
            }
            catch (EndOfStreamException)
            {

            }
        }

        public void Dispose()
        {
            fileReader.Close(); // w ostatecznosci
            fileReader.Dispose();
        }
    }
}