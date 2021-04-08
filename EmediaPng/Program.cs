using System;
using System.IO;

namespace EmediaPng
{
	class Program
	{

		private const string examples = "../../../FileExamples/";

		static void Main(string[] args)
		{
			string[] files = Directory.GetFiles(examples);

			Console.WriteLine("Choose image");

			for(int i =0; i < files.Length; i++)
			{
				Console.Write(i + ": ");
				Console.WriteLine(files[i].Substring(examples.Length));
			}

			int choice = Convert.ToInt32(Console.ReadLine());
			if (0 <= choice && choice < files.Length)
			{
				var png = new PngParser(files[choice]);
			}
			else
				throw new Exception();
		}
	}
}
