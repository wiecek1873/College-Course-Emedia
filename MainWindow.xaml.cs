using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using AForge.Imaging;

namespace EmediaWPF
{
	public partial class MainWindow : Window
	{
		private string imageName;
		private ComplexImage complexImage;
		private const string clearFilePath = "../../";
		private bool Debug = true;

		public MainWindow()
		{
			InitializeComponent();
			ConsoleAllocator.ShowConsoleWindow();
		}

		private void LoadFile_Click(object sender, RoutedEventArgs e)
		{
			DataEncryption dataEncryption = new DataEncryption();
			dataEncryption.Run();
			//Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
			//dlg.FileName = "Image"; // Default file name
			//dlg.DefaultExt = ".png"; // Default file extension

			//Nullable<bool> result = dlg.ShowDialog();

			//if (result == true)
			//{
			//	imageName = dlg.FileName;
			//	var png = new PngParser(dlg.FileName);
			//	if (!Debug)
			//	{
			//		Console.Clear();
			//		png.PrintChunks();
			//	}
			//	var uri = new Uri(dlg.FileName);
			//	var image = new BitmapImage(uri);
			//	MainImage.Source = image;
			//	FourierImage.Source = FFT.FastFourierTransform(image, out complexImage);
			//}
			//PhaseFFTButton.IsEnabled = true;
			//BackwardFFTButton.IsEnabled = true;
		}

		private void ClearFile_Click(object sender, RoutedEventArgs e)
		{
			Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

			Nullable<bool> result = dlg.ShowDialog();

			if (result == true)
			{
				imageName = dlg.FileName;
				var png = new PngParser(dlg.FileName);
				if (!Debug)
				{
					Console.Clear();
					png.PrintChunks();
				}
				DirectoryInfo di = Directory.CreateDirectory(clearFilePath + "output");

				//png.SaveCriticalOnly(clearFilePath, "test.png");
				//lub
				png.SaveWithoutMetadata(clearFilePath + "output/", "Clear_" + System.IO.Path.GetFileName(dlg.FileName));
				var uri = new Uri(dlg.FileName);
				var image = new BitmapImage(uri);
				MainImage.Source = image;
				FourierImage.Source = FFT.FastFourierTransform(image);

				if(!Debug)
				Console.WriteLine("Clear file saved at \"output\" directory!");
			}
		}

		private void BackwardFourierTransform_Click(object sender, RoutedEventArgs e)
		{
			FourierImage.Source = FFT.BackwardFourierTransform(complexImage);
		}

		private void FourierTransformPhase_Click(object sender, RoutedEventArgs e)
		{
			FourierImage.Source = FFT.FromFourierToPhase(complexImage, imageName);
		}
	}
}
