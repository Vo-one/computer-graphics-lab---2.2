using Lab1_kg_;
using System;
using System.Drawing;

namespace WindowsFormsApp1
{
    class Waves : Filter
    {
        public override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            int nx = Clamp((x + (int)(20 * Math.Sin(2 * Math.PI * y / 100)) * 4), 0, sourceImage.Width - 4);
            Color sourceColor = sourceImage.GetPixel(nx, y);
            Color resultColor = Color.FromArgb(Clamp(sourceColor.R, 0, 255),
                Clamp(sourceColor.G, 0, 255),
               Clamp(sourceColor.B, 0, 255));
            return resultColor;
        }
    }
}