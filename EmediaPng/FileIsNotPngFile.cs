using System;
using System.Runtime.Serialization;

namespace EmediaPng
{
    [Serializable]
    internal class FileIsNotPngFile : Exception
    {
        public FileIsNotPngFile()
        {
        }
    }
}