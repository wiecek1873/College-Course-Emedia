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
        private bool Debug = false;

        public MainWindow()
        {
            InitializeComponent();
			ConsoleAllocator.ShowConsoleWindow();

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
					png.EncryptIDAT_ECB();
                    png.Save("", "ecb.png");

					var ecb = new PngParser("ecb.png");
                    ecb.DecryptIDAT_ECB();
                    ecb.Save("", "PoDeszyfrowaniuECB.png");

                    var png2 = new PngParser(dlg.FileName);
                    png2.EncryptIDAT_CBC();
                    png.Save("", "cbc.png");

					var cbc = new PngParser("cbc.png");
                    cbc.DecryptIDAT_CBC();
                    cbc.Save("", "PoDeszyfrowaniuCBC.png");

                    var view = new PngParser("ecb.png");
                    view.Rewrite("PodgladECB.png");
                    // view.Rewrite2();
                    // view.Save("", "Zaszyfrowany.png");

                    var view2 = new PngParser("cbc.png");
                    view2.Rewrite("PodgladCBC.png");
                    // view2.Rewrite2();
                    // view2.Save("", "Zaszyfrowany.png");
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
