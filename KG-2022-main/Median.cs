using Lab1_kg_;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;


namespace WindowsFormsApp1
{
    class Median : Filter
    {
        public override Color calculateNewPixelColor(Bitmap sourseImage, int x, int y)
        {
            Color resultColor = sourseImage.GetPixel(x, y);
            int radiusX = 3;
            int radiusY = 3;
            List<int> listR = new List<int>();
            List<int> listG = new List<int>();
            List<int> listB = new List<int>();

            int idx;
            int idy;
            for (int l = -radiusX; l <= radiusX; ++l)
                for (int k = -radiusY; k <= radiusY; ++k)
                {
                    idx = Clamp(x + l, 0, sourseImage.Width - 1);
                    idy = Clamp(y + k, 0, sourseImage.Height - 1);
                    listR.Add(sourseImage.GetPixel(idx, idy).R);
                    listG.Add(sourseImage.GetPixel(idx, idy).G);
                    listB.Add(sourseImage.GetPixel(idx, idy).B);
                }
            listR.Sort();
            listG.Sort();
            listB.Sort();

            int d1, d2, d3;
            d1 = Clamp((int)listR[listR.Count() / 2], 0, 255);
            d2 = Clamp((int)listG[listG.Count() / 2], 0, 255);
            d3 = Clamp((int)listB[listB.Count() / 2], 0, 255);
            resultColor = Color.FromArgb(d1, d2, d3);
            return resultColor;
        }
    }
}