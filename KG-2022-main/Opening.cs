using Lab1_kg_;
using System.ComponentModel;
using System.Drawing;

namespace WindowsFormsApp1
{
    internal class Opening : MorphOps
    {
        public override Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker)
        {

            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);
            Erosion er;
            Dilation di;
            for (int i = 0; i < sourceImage.Width; i++)
            {
                worker.ReportProgress((int)((float)i / resultImage.Width * 100)); // будет
                                                                                  // сигнализировать элементу BackgroundWorker о текущем прогрессе.
                if (worker.CancellationPending)
                {
                    return null;
                }
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    er = new Erosion();
                    resultImage.SetPixel(i, j, er.calculateNewPixelColor(sourceImage, i, j));
                }

            }
            Bitmap resultImage2 = new Bitmap(resultImage.Width, resultImage.Height);
            for (int i = 0; i < resultImage.Width; i++)
            {

                if (worker.CancellationPending)
                {
                    return null;
                }
                for (int j = 0; j < resultImage.Height; j++)
                {
                    di = new Dilation();
                    resultImage2.SetPixel(i, j, di.calculateNewPixelColor(resultImage, i, j));
                }
            }
            return resultImage2;
        }
    }
}