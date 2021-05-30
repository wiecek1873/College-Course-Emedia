using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using AForge.Imaging;
using System.Security.Cryptography;
using System.Linq;
using System.Numerics;
using System.Collections.Generic;
using System.Text;

namespace EmediaWPF
{
	public partial class MainWindow : Window
	{
		private string imageName;
		private ComplexImage complexImage;
		private const string clearFilePath = "../../";
		private bool Debug = false;

		public MainWindow()
		{
			InitializeComponent();
			ConsoleAllocator.ShowConsoleWindow();

			RSAcsp rsaCSP = new RSAcsp();
			RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();

			byte[] dataToEncrypt = new byte[] { 101, 123, 223, 123, 234, 0, 0, 255, 255 };

			byte[] encryptedData;
			byte[] decryptedData;

			encryptedData = rsaCSP.RSAEncrypt(dataToEncrypt, RSA.ExportParameters(false), false);

			decryptedData = rsaCSP.RSADecrypt(encryptedData, RSA.ExportParameters(true), false);

			Console.WriteLine("Decrypted plaintext: {0}", string.Join(" ",decryptedData));

			//TemporaryRSATest();
		}

		private void LoadFile_Click(object sender, RoutedEventArgs e)
{
	//TemporaryRSATest();
	DataEncryption.Instance.SetKeys(SaveManager.Instance.LoadKeys());
	//DataEncryption.Instance.KeyTest();

	Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
	dlg.FileName = "Image"; // Default file name
	dlg.DefaultExt = ".png"; // Default file extension

	Nullable<bool> result = dlg.ShowDialog();

	if (result == true)
	{
		imageName = dlg.FileName;
		var png = new PngParser(dlg.FileName);

		if (!Debug)
		{
			Console.Clear();
			png.PrintChunks();
					//png.EncryptAndSave("", "test.png");
					png.EncryptAndSaveCSP("", "test.png");
			//var xd = new PngParser("test.png");
			//xd.Decrypt();
			//xd.Save("", "PoDeszyfr.png");
		}

		var uri = new Uri(dlg.FileName);
		var image = new BitmapImage(uri);
		MainImage.Source = image;
		FourierImage.Source = FFT.FastFourierTransform(image, out complexImage);
	}

	PhaseFFTButton.IsEnabled = true;
	BackwardFFTButton.IsEnabled = true;
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

		if (!Debug)
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
