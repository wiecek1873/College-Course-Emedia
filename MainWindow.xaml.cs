using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;

namespace EmediaWPF
{
	public partial class MainWindow : Window
	{
		private const string clearFilePath = "../../";
		private const string examples = "../../FileExamples/";
		public MainWindow()
		{
			InitializeComponent();
		}

		private void LoadFile_Click(object sender, RoutedEventArgs e)
		{
			ConsoleAllocator.ShowConsoleWindow();
			string[] files = Directory.GetFiles(examples);

			for (int i = 0; i < files.Length; i++)
			{
				Console.Write(i + ": ");
				Console.WriteLine(files[i].Substring(examples.Length));
			}

			int choice = Convert.ToInt32(Console.ReadLine());
			if (0 <= choice && choice < files.Length)
			{
				var png = new PngParser(files[choice]);
				png.SaveCriticalOnly(clearFilePath, "test.png");
				png.SaveWithoutMetadata(clearFilePath, "bezMetadanych.png");
			}
			else
				throw new Exception();
		}
	}
}
