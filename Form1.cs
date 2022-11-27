using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using WindowsFormsApp1;


namespace Lab1_kg_
{
    public partial class Form1 : Form
    {
        private Bitmap image, image2, image3, prevImage;
        private BitmapData ImageData, ImageData2;
        private byte[] buffer, buffer2;
        private double gammacorrection;
        private int b, g, r, r_x, g_x, b_x, r_y, g_y, b_y, grayscale, location, location2;
        private sbyte weight_x, weight_y;
        private sbyte[,] weights_x;
        private sbyte[,] weights_y;

        private bool ImageIsNull()
        {
            if (pictureBox1.Image == null) MessageBox.Show("Загрузите фото.", "Ошибка!");
            return pictureBox1.Image == null;
        }
        //save
        private void save1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = ".txt";
            saveFileDialog.Filter = "Image files | *.png; *.jpg; *.bmp | All Files (*.*) | *.*";
            if (saveFileDialog.ShowDialog() == DialogResult.OK) //если в диалоговом окне нажата кнопка "ОК"
            {
                try
                {
                    pictureBox1.Image.Save(saveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                catch
                {
                    MessageBox.Show("Невозможно сохранить изображение", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            /*if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK && saveFileDialog.FileName.Length > 0)
            {
                using (StreamWriter sw = new StreamWriter(saveFileDialog.FileName, true))
                {
                    sw.WriteLine(pictureBox1.Image);
                }
            }*/
        }

        private void gaussToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Gauss filter = new Gauss();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void openingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filter filter = new Opening();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void closingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filter filter = new Closing();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void gradToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filter filter = new Grad();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void dilationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filter filter = new Dilation();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void erosionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filter filter = new Erosion();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void binarToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (image != null)
            {

                Color curColor;
                int ret;

                for (int iX = 0; iX < image.Width; iX++)
                {

                    for (int iY = 0; iY < image.Height; iY++)
                    {
                        curColor = image.GetPixel(iX, iY);

                        ret = (int)((curColor.R + curColor.G + curColor.B)/3.0);

                        if (ret > 120)
                            ret = 255;
                        else
                            ret = 0;

                        image.SetPixel(iX, iY, Color.FromArgb(ret, ret, ret));
                    }
                }
                Invalidate();
                pictureBox1.Image = image;
            }
        }

        private void inversionToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void arifmtecsToolStripMenuItem_Click(object sender, EventArgs e)
        {

             Bitmap bmp = new Bitmap(pictureBox1.Image);
             int r = 0;
             int g = 0;
             int b = 0;

             int total = 0;

             for (int x = 0; x < bmp.Width; x++)
             {
                  for (int y = 0; y < bmp.Height; y++)
                  {
                        Color c = bmp.GetPixel(x, y);

                        r += c.R;
                        g += c.G;
                        b += c.B;

                        total++;

                        r /= total;
                        g /= total;
                        b /= total;
                        int avg = (r + g + b) / 3;

                    bmp.SetPixel(255, y, Color.FromArgb(r, g, b));
                    
                }
                //pictureBox1.Image = bmp;
                bmp.SetPixel(x, 255, Color.FromArgb(r, g, b));
                
            }
            pictureBox1.Image = bmp;
        }

        private void grayToolStripMenuItem_Click(object sender, EventArgs e)
        {
           

                Bitmap bt = new Bitmap(pictureBox1.Image);
                {
                    for (int y = 0; y < image.Height; y++)
                        for (int x = 0; x < image.Width; x++)
                        {
                            Color c = bt.GetPixel(x, y);

                            // int a = c.A;
                            int r = c.R;
                            int g = c.G;
                            int b = c.B;

                            int avg = (r + g + b) / 3;
                            bt.SetPixel(x, y, Color.FromArgb(avg, avg, avg));
                        }
                    pictureBox1.Image = bt;


                }
            }

        private void arifmeticToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filter filter = new BlurFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void pSNRToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            PSNR filter = new PSNR();
            if (ImageIsNull()) return;
            Cursor.Current = Cursors.WaitCursor;
            MessageBox.Show(PSNR.Execute((Bitmap)pictureBox1.Image, (Bitmap)pictureBox3.Image).ToString());
            Cursor.Current = Cursors.Default;
        }

        private void sSIMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SSIM filter = new SSIM();
            if (ImageIsNull()) return;
            Cursor.Current = Cursors.WaitCursor;
            MessageBox.Show(SSIM.Execute((Bitmap)pictureBox1.Image, (Bitmap)pictureBox3.Image).ToString());
            Cursor.Current = Cursors.Default;
        }
        #region Niblack
        int Slice(int val, int min, int max)
        {
            if (val < min) return min;
            if (val > max) return max;
            return val;
        }

        int binSlice(int val, int level)
        {
            int resVal = 0;
            int maxVal = 255;
            if (val >= level) return maxVal;
            return resVal;
        }
        void niblackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int m_size = 3;
            int T = 0;
            double k = 0.2;
            double sig = 0;
            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    int radX = m_size / 2;
                    int radY = m_size / 2;

                    double new_color = 0;
                    for (int a = -radY; a <= radY; a++)

                        for (int b = -radX; b <= radX; b++)
                        {
                            int idX = Slice(i + b, 0, image.Width - 1);
                            int idY = Slice(j + a, 0, image.Height - 1);
                            Color neibCol = image.GetPixel(idX, idY);
                            new_color += neibCol.G;
                        }

                    new_color = new_color / (m_size * m_size);

                    for (int a = -radY; a <= radY; a++)

                        for (int b = -radX; b <= radX; b++)
                        {
                            int idX = Slice(i + b, 0, image.Width - 1);
                            int idY = Slice(j + a, 0, image.Height - 1);
                            Color neibCol = image.GetPixel(idX, idY);
                            sig += (neibCol.G - new_color) * (neibCol.G - new_color);
                        }

                    sig = Math.Sqrt(sig / Math.Pow(m_size, 2));


                    T = (int)(new_color + k * sig);
                }
            }


            Bitmap temp = new Bitmap(image.Width, image.Height);
            for (int i = 0; i < image.Width; i++)
                for (int j = 0; j < image.Height; j++)
                {
                    Color sourceCol2 = image.GetPixel(i, j);
                    Color resultCol = Color.FromArgb((int)(binSlice((int)(0.299 * sourceCol2.R + 0.587 * sourceCol2.G + 0.114 * sourceCol2.B), T)),
                                                     (int)(binSlice((int)(0.299 * sourceCol2.R + 0.587 * sourceCol2.G + 0.114 * sourceCol2.B), T)),
                                                     (int)(binSlice((int)(0.299 * sourceCol2.R + 0.587 * sourceCol2.G + 0.114 * sourceCol2.B), T)));
                    temp.SetPixel(i, j, resultCol);
                }

            pictureBox1.Image = temp;
            pictureBox1.Refresh();

        }
        #endregion

        #region Global
        public int[] CalculateHistogram(Bitmap image)
        {
            int[] hist = new int[256];

            for (int y = 0; y < image.Height; y++)
                for (int x = 0; x < image.Width; x++)
                {
                    Color color = image.GetPixel(x, y);
                    hist[color.R]++;
                }
            return hist;
        }

        public Bitmap GlobalGist(Bitmap sourceImage)
        {
            int width = sourceImage.Width;
            int height = sourceImage.Height;
            Bitmap resImage = new Bitmap(width, height);

            int[] hist = CalculateHistogram(sourceImage);

            int histSum = hist.Sum();
            int cut = (int)(histSum * 0.05);

            for (int i = 0; i < 255; i++)
            {
                if (hist[i] < cut)
                {
                    cut -= hist[i];
                    hist[i] = 0;
                }

                if (cut <= 0) break;
            }

            cut = (int)(histSum * 0.05);

            for (int i = 255; i < 0; i--)
            {
                if (hist[i] < cut)
                {
                    cut -= hist[i];
                    hist[i] = 0;
                }

                if (cut <= 0) break;
            }

            int t = 0;

            int weight = 0;
            for (int i = 0; i < 255; i++)
            {
                if (hist[i] == 0) continue;

                weight += hist[i] * i;
            }

            t = (int)(weight / hist.Sum());

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    Color color = sourceImage.GetPixel(x, y);
                    if (color.R >= t) resImage.SetPixel(x, y, Color.White);
                    else resImage.SetPixel(x, y, Color.Black);

                }
            return resImage;
        }

        private void globalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap result = GlobalGist(image);
            pictureBox1.Image = result;
            pictureBox1.Refresh();
        }

        #endregion

        #region Gauss_noise
        public int clamp(int value, int min, int max) { return value < min ? min : value > max ? max : value; }
        private void gaussnoiseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap res = (Bitmap)image.Clone();
            var rnd = new Random();
            int noisePower = 100;

            for (int y = 0; y < image.Height; y++)
                for (int x = 0; x < image.Width; x++)
                {
                    Color color = res.GetPixel(x, y);
                    var noise = (rnd.NextDouble() + rnd.NextDouble() + rnd.NextDouble() + rnd.NextDouble() - 2) * noisePower;
                    Color newColor = Color.FromArgb(clamp(color.R + (int)noise, 0, 255),
                                                    clamp(color.G + (int)noise, 0, 255),
                                                    clamp(color.B + (int)noise, 0, 255));
                    res.SetPixel(x, y, newColor);
                }

            
            prevImage = image;
            image = res;
            pictureBox1.Image = image;
            pictureBox1.Refresh();
            pictureBox3.Image = prevImage;
            pictureBox3.Refresh();
        }
        #endregion

        #region Uniform
        public float[] Uniform(int size)
        {
            double a = 32;
            double b = 120;

            var uniform = new float[256];
            float sum = 0f;

            for (int i = 0; i < 256; i++)
            {
                float step = i;
                if (step >= a && step <= b)
                {
                    uniform[i] = (1 / (float)(b - a));
                }
                else
                {
                    uniform[i] = 0;
                }
                sum += uniform[i];
            }

            for (int i = 0; i < 256; i++)
            {
                uniform[i] /= sum;
                uniform[i] *= size;
                uniform[i] = (int)Math.Floor(uniform[i]);
            }

            return uniform;
        }
        protected byte[] ComputeNoise(float[] uniform, int size)
        {
            Random random = new Random();
            int count = 0;
            var noise = new byte[size];
            for (int i = 0; i < 256; i++)
            {
                for (int j = 0; j < (int)uniform[i]; j++)
                {
                    noise[j + count] = (byte)i;
                }
                count += (int)uniform[i];
            }

            for (int i = 0; i < size - count; i++)
            {
                noise[count + i] = 0;
            }

            noise = noise.OrderBy(x => random.Next()).ToArray();
            return noise;
        }

        public Bitmap CalculateBitmap(Bitmap sourceImage, float[] uniform)
        {
            int size = sourceImage.Width * sourceImage.Height;

            var noise = ComputeNoise(uniform, size);

            var resImage = new Bitmap(sourceImage);

            for (int y = 0; y < sourceImage.Height; y++)
                for (int x = 0; x < sourceImage.Width; x++)
                {
                    Color color = sourceImage.GetPixel(x, y);
                    var newValue = clamp(GetBrightness(color) +
                    noise[sourceImage.Width * y + x], 0, 255);

                    resImage.SetPixel(x, y, Color.FromArgb(newValue, newValue, newValue));

                }
            return resImage;
        }
        private static byte GetBrightness(Color color)
        {
            return (byte)(.299 * color.R + .587 * color.G + .114 * color.B);
        }

        private void gaussToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Filter filter = new Gauss();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void medianToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Filter filter = new Median();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void равномерныйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap result = CalculateBitmap(image, Uniform(image.Width * image.Height));
            prevImage = image;
            image = result;
            pictureBox1.Image = result;
            pictureBox1.Refresh();
            pictureBox3.Image = prevImage;
            pictureBox3.Refresh();
        }
        #endregion
        private void autoToolStripMenuItem_Click(object sender, EventArgs e)   
        {
                double rmax = image.GetPixel(0, 0).R;
                double gmax = image.GetPixel(0, 0).G;
                double bmax = image.GetPixel(0, 0).B;

                double rmin = image.GetPixel(0, 0).R;
                double gmin = image.GetPixel(0, 0).G;
                double bmin = image.GetPixel(0, 0).B;

                image3 = new Bitmap(image.Width, image.Height);

                for (int i = 0; i < image.Width; i++)
                    for (int j = 0; j < image.Height; j++)
                    {
                        Color tmp = image.GetPixel(i, j);
                        if (tmp.R > rmax)
                            rmax = tmp.R;
                        if (tmp.G > gmax)
                            gmax = tmp.G;
                        if (tmp.B > bmax)
                            bmax = tmp.B;

                        if (tmp.R < rmin)
                            rmin = tmp.R;
                        if (tmp.G < gmin)
                            gmin = tmp.G;
                        if (tmp.B < bmin)
                            bmin = tmp.B;
                    }

                for (int i = 0; i < image.Width; i++)
                    for (int j = 0; j < image.Height; j++)
                    {
                        Color tmp = image.GetPixel(i, j);
                        image3.SetPixel(i, j, Color.FromArgb((int)((tmp.R - rmin) / (rmax - rmin) * 255.0),
                                                         (int)((tmp.G - gmin) / (gmax - gmin) * 255.0),     
                                                         (int)((tmp.B - bmin) / (bmax - bmin) * 255.0)));                               
                    } //New_val= (val-min_val) / (max_val-min_val) *255
                pictureBox1.Image = image3;
                pictureBox1.Refresh();

        }

        private IntPtr pointer, pointer2;

        public Form1()
        {
            InitializeComponent();
            simpleOpenGlControl1.InitializeContexts();
            weights_x = new sbyte[,] { { 1, 0, -1 }, { 2, 0, -2 }, { 1, 0, -1 } };
            weights_y = new sbyte[,] { { 1, 2, 1 }, { 0, 0, 0 }, { -1, -2, -1 } };
        }

        //dropdown table
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image files | *.png; *.jpg; *.bmp | All Files (*.*) | *.*";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                image = null;
                // image.Dispose();
                image = new Bitmap(dialog.FileName);
                image2 = new Bitmap(image.Width, image.Height);
                pictureBox1.Image = image;
                pictureBox1.Refresh();
            }

        }

        //inersion filter
        private void inversionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InvertFilter filter = new InvertFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        //blur filter
        private void blurToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filter filter = new BlurFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void medianToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filter filter = new Median();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        //waves filter
        private void wavesToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Filter filter = new Waves();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        //download stop 
        private void button1_Click(object sender, EventArgs e) 
        { 
            backgroundWorker1.CancelAsync(); 
        }
        #region background
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Bitmap newImage = ((Filter)e.Argument).processImage(image, backgroundWorker1);
            if (backgroundWorker1.CancellationPending != true)
                image = newImage;
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                pictureBox1.Image = image;
                pictureBox1.Refresh();
            }
            progressBar1.Value = 0;
        }



        #endregion


        //convert
        private void button2_Click(object sender, EventArgs e)
        {
            Bitmap Y, I, Q;
            Convert Test = new Convert();
            Test.convertation(image, out Y, out I, out Q);

            pbY.Image = Y;
            pbI.Image = I;
            pbQ.Image = Q;
        }

        //back load
        private void Form1_Load(object sender, EventArgs e)
        {

        }


        //Gamma filter
        private void button3_Click(object sender, EventArgs e)
        {
            using (Bitmap buffer_image = (Bitmap)image.Clone())
            {
                gammacorrection = 1 / ((double)trackBar1.Value / 10);
                ImageData = buffer_image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                buffer = new byte[3 * image.Width * image.Height];
                pointer = ImageData.Scan0;
                Marshal.Copy(pointer, buffer, 0, buffer.Length);
                for (int i = 0; i < image.Height * 3 * image.Width; i += 3)
                {
                    b = (int)(120 * Math.Pow((buffer[i] / 120.0), gammacorrection));
                    g = (int)(120 * Math.Pow((buffer[i + 1] / 120.0), gammacorrection));
                    r = (int)(120 * Math.Pow((buffer[i + 2] / 120.0), gammacorrection));
                    if (b > 255) b = 255;
                    if (g > 255) g = 255;
                    if (r > 255) r = 255;
                    buffer[i] = (byte)b;
                    buffer[i + 1] = (byte)g;
                    buffer[i + 2] = (byte)r;
                }
                Marshal.Copy(buffer, 0, pointer, buffer.Length);
                buffer_image.UnlockBits(ImageData);
                pictureBox1.Image = (Bitmap)buffer_image.Clone();
            }
        }

        //Sobel filter
        private void sobelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImageData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            ImageData2 = image2.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
            buffer = new byte[ImageData.Stride * image.Height];
            buffer2 = new byte[ImageData.Stride * image.Height];
            pointer = ImageData.Scan0;
            pointer2 = ImageData2.Scan0;
            Marshal.Copy(pointer, buffer, 0, buffer.Length);
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width * 3; x += 3)
                {
                    r_x = g_x = b_x = 0;
                    r_y = g_y = b_y = 0;
                    location = x + y * ImageData.Stride;
                    for (int yy = -(int)Math.Floor(weights_y.GetLength(0) / 2.0d), yyy = 0; yy <= (int)Math.Floor(weights_y.GetLength(0) / 2.0d); yy++, yyy++)
                    {
                        if (y + yy >= 0 && y + yy < image.Height)
                        {
                            for (int xx = -(int)Math.Floor(weights_x.GetLength(1) / 2.0d) * 3, xxx = 0; xx <= (int)Math.Floor(weights_x.GetLength(1) / 2.0d) * 3; xx += 3, xxx++)
                            {
                                if (x + xx >= 0 && x + xx <= image.Width * 3 - 3)
                                {
                                    location2 = x + xx + (yy + y) * ImageData.Stride;
                                    weight_x = weights_x[yyy, xxx];
                                    weight_y = weights_y[yyy, xxx];

                                    b_x += buffer[location2] * weight_x;
                                    g_x += buffer[location2 + 1] * weight_x;
                                    r_x += buffer[location2 + 2] * weight_x;
                                    b_y += buffer[location2] * weight_y;
                                    g_y += buffer[location2 + 1] * weight_y;
                                    r_y += buffer[location2 + 2] * weight_y;
                                }
                            }
                        }
                    }
                    b = (int)Math.Sqrt(Math.Pow(b_x, 2) + Math.Pow(b_y, 2));
                    g = (int)Math.Sqrt(Math.Pow(g_x, 2) + Math.Pow(g_y, 2));
                    r = (int)Math.Sqrt(Math.Pow(r_x, 2) + Math.Pow(r_y, 2));

                    if (b > 255) b = 255;
                    if (g > 255) g = 255;
                    if (r > 255) r = 255;


                    grayscale = (b + g + r) / 3;


                    buffer2[location] = (byte)grayscale;
                    buffer2[location + 1] = (byte)grayscale;
                    buffer2[location + 2] = (byte)grayscale;
                }
            }
            Marshal.Copy(buffer2, 0, pointer2, buffer.Length);
            image.UnlockBits(ImageData);
            image2.UnlockBits(ImageData2);
            pictureBox1.Image = image2;
        }

    }

}