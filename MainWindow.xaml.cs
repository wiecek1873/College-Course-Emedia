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

			Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
			dlg.FileName = "Image"; // Default file name
			dlg.DefaultExt = ".png"; // Default file extension

			Nullable<bool> result = dlg.ShowDialog();

			if (result == true)
			{
				var png = new PngParser(dlg.FileName);
				png.SaveCriticalOnly(clearFilePath, "test.png");
				png.SaveWithoutMetadata(clearFilePath, "bezMetadanych.png");
				var uri = new Uri(dlg.FileName);
				var image = new BitmapImage(uri);
				MainImage.Source = image;
				FourierImage.Source = FFT.FastFourierTransform(image);
			}
		}
	}
}
