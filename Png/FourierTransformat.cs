namespace EmediaWPF
{
    public static class FourierTransformat
    {
        public static void zrobto(image)
        {
            // create complex image
            ComplexImage complexImage = ComplexImage.FromBitmap( image );
            // do forward Fourier transformation
            complexImage.ForwardFourierTransform( );
            // get complex image as bitmat
            Bitmap fourierImage = complexImage.ToBitmap( );
        }
    }
}