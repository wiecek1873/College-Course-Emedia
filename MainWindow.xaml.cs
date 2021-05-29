using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using AForge.Imaging;
using System.Linq;
using System.Numerics;
using System.Collections.Generic;

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

			TemporaryRSATest();
		}

        private void LoadFile_Click(object sender, RoutedEventArgs e)
        {
			TemporaryRSATest();

			//Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
			//dlg.FileName = "Image"; // Default file name
			//dlg.DefaultExt = ".png"; // Default file extension

			//Nullable<bool> result = dlg.ShowDialog();

			//if (result == true)
			//{
			//    imageName = dlg.FileName;
			//    var png = new PngParser(dlg.FileName);

			//    if (!Debug)
			//    {
			//        Console.Clear();
			//        png.PrintChunks();
			//    }

			//    var uri = new Uri(dlg.FileName);
			//    var image = new BitmapImage(uri);
			//    MainImage.Source = image;
			//    FourierImage.Source = FFT.FastFourierTransform(image, out complexImage);
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

        private void TemporaryRSATest()
        {
            RandomNumberGenerator randomNumberGenerator = new RandomNumberGenerator();
            Random rng = new Random();

			int filed = 0;
            DataEncryption dataEncryption = new DataEncryption();
            dataEncryption.PrepareKeys(8);
			for (int j = 0; j < 1000; ++j)
			{

				for (int i = 0; i < 10000; i++)
				{
					// DataEncryption dataEncryption = new DataEncryption();
					byte[] data = randomNumberGenerator.GenerateRandomBytes(rng.Next(1, 15));
					byte[] enc = dataEncryption.EncryptData(data);
					byte[] dec = dataEncryption.DecryptData(enc);

					if (!Comparer(data, dec))
					{
						Console.WriteLine();
						Console.WriteLine("Błąd dla i : " + i);
						Console.WriteLine("Data: " + string.Join(" ", data.Select((b) => string.Format("{0,3}", b))));
						Console.WriteLine("|Enc: " + string.Join(" ", enc.Select((b) => string.Format("{0,3}", b))));
						Console.WriteLine("Dec:  " + string.Join(" ", dec.Select((b) => string.Format("{0,3}", b))));
						Console.WriteLine($"e = {dataEncryption.e}");
						Console.WriteLine($"d = {dataEncryption.d}");
						Console.WriteLine($"n = {dataEncryption.n}");
						Console.WriteLine($"key length = {dataEncryption.KeyLength}");
						++filed;
						break;
					}
				}
				Console.WriteLine("| Jesteśmy na j: " + j);
			}
			Console.WriteLine("Oblane: " + filed);
		}

        private bool Comparer(byte[] data, byte[] dec)
        {
            if (dec.Length < data.Length)
            {
                Console.WriteLine("mniejsza dlugosc! " + dec.Length + " " + data.Length);
                return false;
            }

            for (int i = 0; i < dec.Length; ++i)
                if (i < data.Length)
                {
                    if (data[i] != dec[i])
                    {
                        Console.WriteLine("rozne wartosci!");
                        return false;
                    }
                }
                else
                {
                    if (dec[i] != 0)
                    {
                        Console.WriteLine("nie zera na koncu :(");
                        return false;
                    }
                }

            return true;
        }
    }
}
