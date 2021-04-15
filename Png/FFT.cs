using AForge.Imaging;
using AForge.Imaging.Filters;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace EmediaWPF
{
	public static class FFT
	{
		public static BitmapImage FastFourierTransform(BitmapImage image)
		{
			Bitmap bitmap = image.ToBitmap().ScaleToPowerBy2().ToGrayScale();
			ComplexImage complexImage = ComplexImage.FromBitmap(bitmap);
			complexImage.ForwardFourierTransform();
			return complexImage.ToBitmapImage();
		}

		public static BitmapImage FastFourierTransform(BitmapImage image, out ComplexImage complexImage)
		{
			Bitmap bitmap = image.ToBitmap().ScaleToPowerBy2().ToGrayScale();
			complexImage = ComplexImage.FromBitmap(bitmap);
			complexImage.ForwardFourierTransform();
			return complexImage.ToBitmapImage();
		}

		public static BitmapImage BackwardFourierTransform(ComplexImage image)
		{
			image.BackwardFourierTransform();
			return image.ToBitmapImage();
		}

		private static Bitmap ToBitmap(this BitmapImage bitmapImage)
		{
			using (MemoryStream outStream = new MemoryStream())
			{
				BitmapEncoder enc = new BmpBitmapEncoder();
				enc.Frames.Add(BitmapFrame.Create(bitmapImage));
				enc.Save(outStream);
				Bitmap bitmap = new Bitmap(outStream);
				return new Bitmap(bitmap);
			}
		}

		private static BitmapImage ToBitmapImage(this ComplexImage complexImage)
		{
			Bitmap bitmap = complexImage.ToBitmap();
			using (var memory = new MemoryStream())
			{
				bitmap.Save(memory, ImageFormat.Png);
				memory.Position = 0;

				var bitmapImage = new BitmapImage();
				bitmapImage.BeginInit();
				bitmapImage.StreamSource = memory;
				bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
				bitmapImage.EndInit();
				bitmapImage.Freeze();

				return bitmapImage;
			}
		}

		private static Bitmap ToGrayScale(this Bitmap thisBitmap)
		{
			Grayscale filter = new Grayscale(0.2125, 0.7154, 0.0721);
			Bitmap grayImage = filter.Apply(thisBitmap);
			return grayImage;
		}

		private static Bitmap ToSizePowerBy2(this Bitmap thisBitmap)
		{
			int width = PowerBy2(thisBitmap.Width);
			int height = PowerBy2(thisBitmap.Height);
			Bitmap newBitmap = new Bitmap(width, height);

			for (int i = 0; i < width; i++)
			{
				for (int x = 0; x < height; x++)
				{
					if (i < thisBitmap.Width && x < thisBitmap.Height)
						newBitmap.SetPixel(i, x, thisBitmap.GetPixel(i, x));
					else
						newBitmap.SetPixel(i, x, new Color()); //todo co z obrazemi nie PowerBy2
				}
			}

			return newBitmap;
		}

		private static Bitmap ScaleToPowerBy2(this Bitmap thisBitmap)
		{
			float width = PowerBy2(thisBitmap.Width);
			float height = PowerBy2(thisBitmap.Height);

			var brush = new SolidBrush(Color.Black);

			float scale = Math.Min(width / thisBitmap.Width, height / thisBitmap.Height);

			var bmp = new Bitmap((int)width, (int)height);
			var graph = Graphics.FromImage(bmp);

			// uncomment for higher quality output
			graph.InterpolationMode = InterpolationMode.High;
			graph.CompositingQuality = CompositingQuality.HighQuality;
			graph.SmoothingMode = SmoothingMode.AntiAlias;

			var scaleWidth = (int)(thisBitmap.Width * scale);
			var scaleHeight = (int)(thisBitmap.Height * scale);

			graph.FillRectangle(brush, new RectangleF(0, 0, width, height));
			graph.DrawImage(thisBitmap, ((int)width - scaleWidth) / 2, ((int)height - scaleHeight) / 2, scaleWidth, scaleHeight);

			return bmp;
		}

		private static int PowerBy2(int height)
		{
			int pow = 1;
			while (height > pow)
			{
				pow *= 2;
			}
			return pow;
		}
	}

}
