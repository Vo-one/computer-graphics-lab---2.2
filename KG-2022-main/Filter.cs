using System.ComponentModel;
using System.Drawing;

namespace Lab1_kg_
{
    internal abstract class Filter
    {
        public abstract Color calculateNewPixelColor(Bitmap sourceImage, int x, int y);
        public virtual Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker)
        {
            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);
            for (int i = 0; i < sourceImage.Width; i++)
            {
                worker.ReportProgress((int)((float)i / sourceImage.Width * 100));
                if (worker.CancellationPending)
                    return null;

                for (int j = 0; j < sourceImage.Height; j++)
                    resultImage.SetPixel(i, j, calculateNewPixelColor(sourceImage, i, j));
            }

            return resultImage;
        }

        public int Clamp(int value, int min, int max) { return value < min ? min : value > max ? max : value; }
    }
}
