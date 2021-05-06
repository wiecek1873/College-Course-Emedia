using System;

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