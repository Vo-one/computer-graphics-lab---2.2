using System.Drawing;


namespace Lab1_kg_
{
    internal class InvertFilter : Filter
    {

        public override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color sourceColor = sourceImage.GetPixel(x, y);
            Color resultColor = Color.FromArgb(255 - sourceColor.R,
                                                255 - sourceColor.G,
                                                255 - sourceColor.B);

            return resultColor;
        }
    }
}
