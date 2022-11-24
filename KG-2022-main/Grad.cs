using Lab1_kg_;
using System.ComponentModel;
using System.Drawing;

namespace WindowsFormsApp1
{
    internal class Grad : MorphOps
    {

        public override Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker)
        {

            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);
            Bitmap resultImage1 = new Bitmap(sourceImage.Width, sourceImage.Height);
            Bitmap resultImage2 = new Bitmap(sourceImage.Width, sourceImage.Height);
            Dilation di;
            Erosion er;
            for (int i = 0; i < sourceImage.Width; i++)
            {
                worker.ReportProgress((int)((float)i / resultImage.Width * 100));

                if (worker.CancellationPending)
                {
                    return null;
                }
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    di = new Dilation();
                    er = new Erosion();
                    resultImage.SetPixel(i, j, di.calculateNewPixelColor(sourceImage, i, j));
                    resultImage2.SetPixel(i, j, er.calculateNewPixelColor(sourceImage, i, j));
                    int newR = Clamp(resultImage.GetPixel(i, j).R - resultImage2.GetPixel(i, j).R, 0, 255);
                    int newG = Clamp(resultImage.GetPixel(i, j).G - resultImage2.GetPixel(i, j).G, 0, 255);
                    int newB = Clamp(resultImage.GetPixel(i, j).B - resultImage2.GetPixel(i, j).B, 0, 255);
                    resultImage1.SetPixel(i, j, Color.FromArgb(newR, newG, newB));
                }

            }

            return resultImage1;
        }
    }
}