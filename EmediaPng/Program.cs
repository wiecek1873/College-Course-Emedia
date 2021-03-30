using System;

namespace EmediaPng
{
    class Program
    {
        private const string testFile = "FileExamples/c-sharp-logo.png";

        static void Main(string[] args)
        {
            var png = new PngParser(testFile);
            png.DebugPrint();
        }
    }
}
