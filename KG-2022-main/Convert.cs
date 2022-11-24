using System.Drawing;

namespace Lab1_kg_
{
    class Convert
    {
        public void convertation(Bitmap sourceImage, out Bitmap Y, out Bitmap I, out Bitmap Q)
        {
            int w = sourceImage.Width;
            int h = sourceImage.Height;

            Y = new Bitmap(w, h);
            I = new Bitmap(w, h);
            Q = new Bitmap(w, h);

            for (int yy = 0; yy < h; yy++)
                for (int x = 0; x < w; x++)
                {
                    //получаем пикселb в RGB
                    Color c = sourceImage.GetPixel(x, yy);
                    //переводим в YIQ
                    float y, i, q;
                    RGB2YIQ(c, out y, out i, out q);
                    //переводим обратно в RGB, зануляя два канала
                    Y.SetPixel(x, yy, YIQ2RGB(y, 0, 0));
                    I.SetPixel(x, yy, YIQ2RGB(127, i, 0));
                    Q.SetPixel(x, yy, YIQ2RGB(127, 0, q));
                }
        }
        void RGB2YIQ(Color c, out float y, out float i, out float q)
        {
            y = 0.299f * c.R + 0.587f * c.G + 0.114f * c.B;
            i = 0.596f * c.R - 0.274f * c.G - 0.322f * c.B;
            q = 0.211f * c.R - 0.523f * c.G + 0.312f * c.B;
            
        }

        Color YIQ2RGB(float y, float i, float q)
        {
            var r = 1 * y + 0.956f * i + 0.621f * q;
            var g = 1 * y - 0.272f * i - 0.647f * q;
            var b = 1 * y - 1.106f * i + 1.703f * q;

            return Color.FromArgb(255, (byte)r, (byte)g, (byte)b);
        }
    }
}
