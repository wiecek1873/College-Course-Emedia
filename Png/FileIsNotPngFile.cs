using System;
using System.Runtime.Serialization;

namespace EmediaWPF
{
    [Serializable]
    internal class FileIsNotPngFile : Exception
    {
        public FileIsNotPngFile()
        {
        }
    }
}