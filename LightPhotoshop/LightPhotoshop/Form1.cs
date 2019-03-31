using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;
using System.Collections;
namespace LightPhotoshop
{
    public partial class Form1 : Form
    {

        public Form1()
        {   
            InitializeComponent();
        }
        //Geri alma için stack oluşturuluyor
        Stack stack = new Stack(); 

        int deltaCount = 0; //Zoom katsayısı
        int orjWidth = 0;
        int orjHeight = 0;
        Bitmap orjBitmap;
        Bitmap UsedBitmap;
        Bitmap processingBitmap;
        Image backGraund;
        int numericValue = 0;
        int firstWidth = 0;
        int firstHeight = 0;

        Bitmap greyBitmap;
        int formWidth = 0;
        int formHeight = 0;
        Size currentSize;
        Bitmap invertBitmap;
        bool invertStatus = false;
        bool mirrorStatus = false;
        bool rotateStatus = false;
        bool process = false;

        bool redStatus = false;
        bool greenStatus = false;
        bool blueStatus = false;
        bool greySatus = false;
        Image Channel;
        Image Zoom;
        object sender;

        bool rgbStatus = false;

        protected override void OnMouseWheel(MouseEventArgs e )
        {
            if (pictureBox1.Image != null) {
                if (e.Delta <= 0 && (currentSize.Width > firstWidth / 2 || currentSize.Height > firstHeight / 2))
                {
                    deltaCount--;
                    if (deltaCount >= -50)
                        numericUpDown1.Value = 50 + deltaCount;
                }
                else if (e.Delta > 0 && (currentSize.Width <= 2 * firstWidth && currentSize.Height <= 2 * firstHeight))
                {
                    deltaCount++;
                    if (deltaCount <= 50)
                        numericUpDown1.Value = 50 + deltaCount;
                }

                Resize(deltaCount);
                ChannelControl(UsedBitmap, sender, e);
                pictureBox1.Image = UsedBitmap;
            }
        }

        public void Resize(int deltaCount)
        {
            if (pictureBox1.Image != null) {
                if (deltaCount <= 0)
                {
                    Size newSize = new Size((int)((rotateStatus == false ? orjWidth : orjHeight) + (deltaCount * (rotateStatus == false ? orjWidth : orjHeight) * 0.01)), (int)((rotateStatus == false ? orjHeight : orjWidth) + (deltaCount * (rotateStatus == false ? orjHeight : orjWidth) * 0.01)));
                    currentSize = newSize;
                    if ((newSize.Width <= 2 * firstWidth || newSize.Height <= 2 * firstHeight) && (newSize.Width > firstWidth / 2 || newSize.Height > firstHeight / 2))
                    {
                        processingBitmap = new Bitmap(UsedBitmap, newSize);
                        pictureBox1.Image = processingBitmap;
                    }
                }
                else
                {
                    Size newSize = new Size((int)((rotateStatus == false ? orjWidth : orjHeight) + (deltaCount * (rotateStatus == false ? orjWidth : orjHeight) * 0.02)), (int)((rotateStatus == false ? orjHeight : orjWidth) + (deltaCount * (rotateStatus == false ? orjHeight : orjWidth) * 0.02)));
                    currentSize = newSize;
                    if ((newSize.Width <= 2 * firstWidth || newSize.Height <= 2 * firstHeight) && (newSize.Width > firstWidth / 2 || newSize.Height > firstHeight / 2))
                    {
                        processingBitmap = new Bitmap(UsedBitmap, newSize);
                        pictureBox1.Image = processingBitmap;
                    }
                }
            }
        }


        public void GetOriginalSize(Bitmap orj)
        {
            orjBitmap = new Bitmap(pictureBox1.Image);
            processingBitmap = orjBitmap;
            orjWidth = orj.Width;
            orjHeight = orj.Height;
        }

        public void GetFormSize(int getFormWidth, int getFormHeight) {
            
            formWidth = getFormWidth;
            formHeight = getFormHeight;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            OpenFileDialog open = new OpenFileDialog();

            open.Title = "Open Images";
            open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp; *.png)|*.jpg; *.jpeg; *.gif; *.bmp; *.png";

            if (open.ShowDialog() == DialogResult.OK)
            {

                Bitmap bitmap = new Bitmap(open.FileName);
                orjBitmap = bitmap;
                UsedBitmap = bitmap;
                processingBitmap = bitmap;
                invertStatus = false;                
                GetFormSize(this.Width, this.Height);

                double newWidth = 0;
                double newHeight = 0;

                while (bitmap.Width > this.formWidth || bitmap.Height > this.formHeight)
                {
                     newWidth = Convert.ToInt32(bitmap.Width - (bitmap.Width * 0.05));
                     newHeight = Convert.ToInt32(bitmap.Height - (bitmap.Height * 0.05));
                    bitmap = new Bitmap(bitmap, (int)newWidth, (int)newHeight);
                }

                if (bitmap != null && bitmap.Width == newWidth && bitmap.Height == newHeight)
                {
                    bitmap = new Bitmap(bitmap, (int)newWidth/3*2, (int)newHeight/3*2);
                }

                firstHeight = bitmap.Height;
                firstWidth = bitmap.Width;

                currentSize = bitmap.Size;
                pictureBox1.Width = this.Width;
                pictureBox1.Height = this.Height - 100;
                pictureBox1.Image = bitmap;
                GetOriginalSize(bitmap);
                var pictureBoxWidth = pictureBox1.Width;
                var pictureBoxHeight = pictureBox1.Height;
                var pictureWidth = bitmap.Width;
                var pictureHeight = bitmap.Height;
                var formWidth = this.Width;
                var formHeight = this.Height;
                backGraund = pictureBox1.BackgroundImage;
                pictureBox1.BackgroundImage = null;
                numericUpDown1.Enabled = true;
                stack.Push("open");
                rGBToolStripMenuItem2.Image = Channel;

                redToolStripMenuItem.Image = null;
                greenToolStripMenuItem.Image = null;
                blueToolStripMenuItem.Image = null;

                reopenToolStripMenuItem.Enabled = true;
                saveToolStripMenuItem.Enabled = true;
                backToolStripMenuItem.Enabled = true;
                ınvertToolStripMenuItem.Enabled = true;
                mirroringToolStripMenuItem.Enabled = true;
                sağaDöndürmeToolStripMenuItem.Enabled = true;
                solaDöndürmeToolStripMenuItem.Enabled = true;
                greyscaleToolStripMenuItem.Enabled = true;

                rGBToolStripMenuItem2.Enabled = true;
                redToolStripMenuItem.Enabled = true;
                greenToolStripMenuItem.Enabled = true;
                blueToolStripMenuItem.Enabled = true;

                rGBToolStripMenuItem1.Enabled = true;
                redToolStripMenuItem1.Enabled = true;
                greenToolStripMenuItem1.Enabled = true;
                blueToolStripMenuItem1.Enabled = true;
            }

            open.Dispose();
        }

        private void ınvertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            islem1();

            var t = Task.Delay(100);
            t.Wait();

            if (process == true)
            {
                if (UsedBitmap != null)
                {
                    Color color;
                    Bitmap bitmap = new Bitmap(UsedBitmap);

                    for (int i = 0; i < bitmap.Width; i++)
                    {
                        for (int j = 0; j < bitmap.Height; j++)
                        {
                            color = bitmap.GetPixel(i, j);
                            color = Color.FromArgb(255 - color.R, 255 - color.G, 255 - color.B);
                            bitmap.SetPixel(i, j, color);
                        }
                    }

                    invertStatus = true;

                    UsedBitmap = bitmap;
                    bitmap = new Bitmap(UsedBitmap, currentSize);
                    processingBitmap = bitmap;

                    pictureBox1.Image = processingBitmap;
                    stack.Push("invert");
                    ChannelControl(UsedBitmap, sender, e);
                }
            }
            islem3();
            
        }

        private void mirroringToolStripMenuItem_Click(object sender, EventArgs e)
        {
            islem1();

            var t = Task.Delay(100);
            t.Wait();

            if (process == true)
            {
                if (UsedBitmap != null)
                {
                    Color color;
                    Bitmap bitmap = new Bitmap(UsedBitmap);
                    Bitmap bitmapYedek = new Bitmap(UsedBitmap);

                    for (int i = 0; i < bitmap.Width; i++)
                    {
                        for (int j = 0; j < bitmap.Height; j++)
                        {
                            color = bitmapYedek.GetPixel(i, j);

                            bitmap.SetPixel(bitmap.Width - i - 1, j, color);
                        }
                    }
                    processingBitmap = bitmap;
                    pictureBox1.Image = processingBitmap;
                    UsedBitmap = bitmap;
                    bitmap = new Bitmap(UsedBitmap, currentSize);
                    if (mirrorStatus == false)
                    {
                        mirrorStatus = true;
                    }
                    else
                    {
                        mirrorStatus = false;
                    }
                    stack.Push("mirror");
                    ChannelControl(UsedBitmap, sender, e);
                }
            }

            islem3();
        }

        private void sağaDöndürmeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            islem1();

            var t = Task.Delay(100);
            t.Wait();

            if (process == true)
            {
                if (pictureBox1.Image != null)
                {
                    Color color;
                    Bitmap bitmap = new Bitmap(UsedBitmap);
                    Bitmap newBitmap = new Bitmap(bitmap.Height, bitmap.Width);

                    for (int i = 0; i < bitmap.Width; i++)
                    {
                        for (int j = 0; j < bitmap.Height; j++)
                        {
                            color = bitmap.GetPixel(i, j);
                            newBitmap.SetPixel(newBitmap.Width - j -1, i, color);

                        }
                    }
                    
                    processingBitmap = newBitmap;
                    UsedBitmap = newBitmap;
                    pictureBox1.Image = processingBitmap;
                    bitmap = new Bitmap(UsedBitmap, currentSize);
                    ChannelControl(newBitmap, sender, e);

                    if (rotateStatus == false)
                    {
                        rotateStatus = true;
                    }
                    else
                    {
                        rotateStatus = false;
                    }

                    numericUpDown1.Value = 50;
                    numericValue = (int)numericUpDown1.Value;
                    deltaCount = (numericValue - 50);
                    Resize(deltaCount);

                    stack.Push("right");
                }
            }

            islem3();
        }

        private void solaDöndürmeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            islem1();

            var t = Task.Delay(100);
            t.Wait();

            if (process == true)
            {
                if (pictureBox1.Image != null)
                {
                    Color color;
                    Bitmap bitmap = new Bitmap(UsedBitmap);
                    Bitmap newBitmap = new Bitmap(bitmap.Height, bitmap.Width);

                    for (int i = 1; i < bitmap.Width; i++)
                    {
                        for (int j = 1; j < bitmap.Height; j++)
                        {
                            color = bitmap.GetPixel(i, j);
                            newBitmap.SetPixel(j, newBitmap.Height - i, color);

                        }
                    }
                    
                    processingBitmap = newBitmap;
                    pictureBox1.Image = processingBitmap;
                    UsedBitmap = newBitmap;
                    ChannelControl(newBitmap, sender, e);

                    if (rotateStatus == false)
                    {
                        rotateStatus = true;
                    }
                    else
                    {
                        rotateStatus = false;
                    }

                    numericUpDown1.Value = 50;
                    numericValue = (int)numericUpDown1.Value;
                    deltaCount = (numericValue - 50);
                    Resize(deltaCount);

                    stack.Push("left");
                }
            }

            islem3();
        }

        private void greyscaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            islem1();

            var t = Task.Delay(100);
            t.Wait();

            if (process == true)
            {
                if (greySatus == false && UsedBitmap != null)
                {
                    Color color;
                    Bitmap bitmap2 = new Bitmap(UsedBitmap);
                    Bitmap bitmap = new Bitmap(UsedBitmap);
                    greyBitmap = bitmap2;

                    for (int i = 0; i < bitmap.Width; i++)
                    {
                        for (int j = 0; j < bitmap.Height; j++)
                        {
                            color = bitmap.GetPixel(i, j);
                            int mixColor = (color.R + color.G + color.B) / 3;
                            Color newColor = Color.FromArgb(mixColor, mixColor, mixColor);
                            bitmap.SetPixel(i, j, newColor);
                        }
                    }
                    processingBitmap = bitmap;
                    UsedBitmap = bitmap;
                    pictureBox1.Image = processingBitmap;
                    rGBToolStripMenuItem2.Enabled = false;
                    redToolStripMenuItem.Enabled = false;
                    greenToolStripMenuItem.Enabled = false;
                    blueToolStripMenuItem.Enabled = false;
                    stack.Push("grey");
                    ChannelControl(UsedBitmap, sender, e);
                    greySatus = true;

                    rGBToolStripMenuItem1.Enabled = false;
                    redToolStripMenuItem1.Enabled = false;
                    greenToolStripMenuItem1.Enabled = false;
                    blueToolStripMenuItem1.Enabled = false;
                    greyToolStripMenuItem1.Enabled = true;
                }
                else
                {
                    greyback(sender, e);
                }
            }

            islem3();
            
        }

        public void greyback(object sender, EventArgs e)
        {
            greySatus = false;
            UsedBitmap = greyBitmap;
            pictureBox1.Image = greyBitmap;
            processingBitmap = greyBitmap;

            greyscaleToolStripMenuItem.Enabled = true;
            redToolStripMenuItem.Enabled = true;
            greenToolStripMenuItem.Enabled = true;
            blueToolStripMenuItem.Enabled = true;
            rGBToolStripMenuItem2.Enabled = true;
            stack.Push("greyback");
            rGBToolStripMenuItem1.Enabled = true;
            redToolStripMenuItem1.Enabled = true;
            greenToolStripMenuItem1.Enabled = true;
            blueToolStripMenuItem1.Enabled = true;
            greyToolStripMenuItem1.Enabled = false;
            ChannelControl(UsedBitmap, sender, e);
        }

        private void redToolStripMenuItem_Click(object sender, EventArgs e)
        {
            islem1();

            var t = Task.Delay(100);
            t.Wait();

            if (process == true)
            {
                if (UsedBitmap != null)
                {
                    Color color;
                    Bitmap bitmap = new Bitmap(UsedBitmap);

                    for (int i = 0; i < bitmap.Width; i++)
                    {
                        for (int j = 0; j < bitmap.Height; j++)
                        {
                            color = bitmap.GetPixel(i, j);
                            Color newColor = Color.FromArgb(color.R, color.R, color.R);
                            bitmap.SetPixel(i, j, newColor);
                        }
                    }

                    pictureBox1.Image = bitmap;
                    redStatus = true;
                    greenStatus = false;
                    blueStatus = false;

                    rGBToolStripMenuItem2.Image = null;
                    redToolStripMenuItem.Image = Channel;
                    greenToolStripMenuItem.Image = null;
                    blueToolStripMenuItem.Image = null;

                }
            }
            islem3();
            
        }

        private void greenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            islem1();

            var t = Task.Delay(100);
            t.Wait();

            if (process == true)
            {
                if (UsedBitmap != null)
                {
                    Color color;
                    Bitmap bitmap = new Bitmap(UsedBitmap);

                    for (int i = 0; i < bitmap.Width; i++)
                    {
                        for (int j = 0; j < bitmap.Height; j++)
                        {
                            color = bitmap.GetPixel(i, j);
                            Color newColor = Color.FromArgb(color.G, color.G, color.G);
                            bitmap.SetPixel(i, j, newColor);
                        }
                    }

                    pictureBox1.Image = bitmap;
                    redStatus = false;
                    greenStatus = true;
                    blueStatus = false;
                    rGBToolStripMenuItem2.Image = null;
                    redToolStripMenuItem.Image = null;
                    greenToolStripMenuItem.Image = Channel;
                    blueToolStripMenuItem.Image = null;

                }
            }
            islem3();
            
        }

        private void blueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            islem1();

            var t = Task.Delay(100);
            t.Wait();

            if (process == true)
            {
                if (UsedBitmap != null)
                {
                    Color color;
                    Bitmap bitmap = new Bitmap(UsedBitmap);

                    int a = bitmap.Width * bitmap.Height;

                    for (int i = 0; i < bitmap.Width; i++)
                    {
                        for (int j = 0; j < bitmap.Height; j++)
                        {
                            color = bitmap.GetPixel(i, j);
                            Color newColor = Color.FromArgb(color.B, color.B, color.B);
                            bitmap.SetPixel(i, j, newColor);
                        }
                    }
                    redStatus = false;
                    greenStatus = false;
                    blueStatus = true;
                    pictureBox1.Image = bitmap;

                    rGBToolStripMenuItem2.Image = null;
                    redToolStripMenuItem.Image = null;
                    greenToolStripMenuItem.Image = null;
                    blueToolStripMenuItem.Image = Channel;

                }
            }
            islem3();
            
        }

        private void reopenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (orjBitmap != null)
            {
                processingBitmap = orjBitmap;
                orjWidth = orjBitmap.Width;
                orjHeight = orjBitmap.Height;
                currentSize = orjBitmap.Size;
                pictureBox1.Image = processingBitmap;
                UsedBitmap = orjBitmap;
                deltaCount = 0;
                invertStatus = false;
                rotateStatus = false;
                
                greySatus = false;

                redStatus = false;
                greenStatus = false;
                blueStatus = false;

                rGBToolStripMenuItem2.Enabled = true;
                redToolStripMenuItem.Enabled = true;
                greenToolStripMenuItem.Enabled = true;
                blueToolStripMenuItem.Enabled = true;

                rGBToolStripMenuItem1.Enabled = true;
                redToolStripMenuItem1.Enabled = true;
                greenToolStripMenuItem1.Enabled = true;
                blueToolStripMenuItem1.Enabled = true;
                greyToolStripMenuItem1.Enabled = false;

                rGBToolStripMenuItem2.Image = Channel;
                redToolStripMenuItem.Image = null;
                greenToolStripMenuItem.Image = null;
                blueToolStripMenuItem.Image = null;

                numericUpDown1.Value = 50;
                numericValue = (int)numericUpDown1.Value;
                deltaCount = (numericValue - 50);
                Resize(deltaCount);

                stack.Clear();
                stack.Push("open");
            }
            
        }

        public void islem1()
        {
            pictureBox2.Visible = true;
            pictureBox1.Visible = false;
            
            if (pictureBox1.Visible == false && pictureBox2.Visible == true)
            {
                process = true;
            }
        }

        public void islem3()
        {
            pictureBox1.Visible = true;
            pictureBox2.Visible = false;
            process = false;
        }
        private void rGBToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            
            islem1();
            
            var t = Task.Delay(100);
            t.Wait();
            if (process == true)
            {
                Bitmap bmp;
                bmp = UsedBitmap;
                int[] histogram_r = new int[256];
                float max = 0;

                for (int i = 0; i < bmp.Width; i++)
                {
                    for (int j = 0; j < bmp.Height; j++)
                    {
                        int value1 = bmp.GetPixel(i, j).R;
                        int value2 = bmp.GetPixel(i, j).G;
                        int value3 = bmp.GetPixel(i, j).B;
                        histogram_r[value1]++;
                        histogram_r[value2]++;
                        histogram_r[value3]++;
                        if (max < histogram_r[value1])
                        {
                            max = histogram_r[value1];
                        }
                        if (max < histogram_r[value2])
                        {
                            max = histogram_r[value2];
                        }
                        if (max < histogram_r[value3])
                        {
                            max = histogram_r[value3];
                        }
                    }
                }
                int[] kirmizi = new int[256];
                int[] yesil = new int[256];
                int[] mavi = new int[256];

                for (int i = 0; i < bmp.Size.Width; i++)
                    for (int j = 0; j < bmp.Size.Height; j++)
                    {
                        Color renk = bmp.GetPixel(i, j);

                        kirmizi[renk.R]++;
                        yesil[renk.G]++;
                        mavi[renk.B]++;

                    }

                int histHeight = 256;
                Bitmap img = new Bitmap(256, histHeight + 10);
                using (Graphics g = Graphics.FromImage(img))
                {
                    for (int i = 0; i < histogram_r.Length; i++)
                    {
                        int toplam = kirmizi[i] + yesil[i] + mavi[i];
                        float size = (toplam / max) * histHeight * 2;
                        g.DrawLine(Pens.Black, i, histHeight - 5, i, histHeight - 5 - size);
                    }
                }


                Form2 settingsForm = new Form2(img);

                settingsForm.Show();
            }
            
            islem3();
           
        }

        private void redToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (UsedBitmap != null)
            {
                islem1();

                var t = Task.Delay(100);
                t.Wait();
                if (process == true)
                {
                    Bitmap bmp;
                    bmp = UsedBitmap;
                    int[] histogram = new int[256];
                    float max = 0;
                    double multiple = 1;

                    for (int i = 0; i < bmp.Width; i++)
                    {
                        for (int j = 0; j < bmp.Height; j++)
                        {
                            int value = bmp.GetPixel(i, j).R;
                            histogram[value]++;
                            if (max < histogram[value] && value != 0)
                            {
                                max = histogram[value];
                            }
                        }
                    }

                    double sum = 0;

                    for (int i = 0; i < histogram.Length; i++)
                    {
                        sum = sum + (histogram[i] / max) * 256;
                    }
                    double ort = sum / 256;

                    if (ort <= 256 / 4)
                    {
                        multiple = 256 / (4 * ort);
                    }

                    int histHeight = 256;
                    Bitmap img = new Bitmap(256, histHeight + 10);
                    using (Graphics g = Graphics.FromImage(img))
                    {
                        for (int i = 0; i < histogram.Length; i++)
                        {
                            float size = (histogram[i] / max) * histHeight;
                            g.DrawLine(Pens.Red, i, histHeight - 5, i, histHeight - 5 - (float)(size * multiple));
                        }
                    }

                    Form2 settingsForm = new Form2(img);

                    settingsForm.Show();

                }
                islem3();
            }              
        }

        private void greenToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (UsedBitmap != null)
            {
                islem1();

                var t = Task.Delay(100);
                t.Wait();
                if (process == true)
                {
                    Bitmap bmp;
                    bmp = UsedBitmap;
                    int[] histogram = new int[256];
                    float max = 0;

                    for (int i = 0; i < bmp.Width; i++)
                    {
                        for (int j = 0; j < bmp.Height; j++)
                        {
                            int value = bmp.GetPixel(i, j).G;
                            histogram[value]++;
                            if (max < histogram[value] && value != 0)
                            {
                                max = histogram[value];
                            }
                        }
                    }

                    int histHeight = 256;
                    Bitmap img = new Bitmap(256, histHeight + 10);
                    using (Graphics g = Graphics.FromImage(img))
                    {
                        for (int i = 0; i < histogram.Length; i++)
                        {
                            float size = (histogram[i] / max) * histHeight;
                            g.DrawLine(Pens.Green, i, histHeight - 5, i, histHeight - 5 - (size));
                        }
                    }

                    Form2 settingsForm = new Form2(img);

                    settingsForm.Show();
                }
                islem3();
            }
     
        }

        private void blueToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if(UsedBitmap != null)
            {
                islem1();

                var t = Task.Delay(100);
                t.Wait();
                if (process == true)
                {
                    Bitmap bmp;
                    bmp = UsedBitmap;
                    int[] histogram = new int[256];
                    float max = 0;

                    for (int i = 0; i < bmp.Width; i++)
                    {
                        for (int j = 0; j < bmp.Height; j++)
                        {
                            int value = bmp.GetPixel(i, j).B;
                            histogram[value]++;
                            if (max < histogram[value] && value != 0)
                            {
                                max = histogram[value];
                            }
                        }
                    }

                    int histHeight = 256;
                    Bitmap img = new Bitmap(256, histHeight + 10);
                    using (Graphics g = Graphics.FromImage(img))
                    {
                        for (int i = 0; i < histogram.Length; i++)
                        {
                            float size = (histogram[i] / max) * histHeight;
                            g.DrawLine(Pens.Blue, i, img.Height - 5, i, img.Height - 5 - (size));
                        }
                    }


                    Form2 settingsForm = new Form2(img);

                    settingsForm.Show();
                }
                islem3();
            }
 
        }

        private void greyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (UsedBitmap != null)
            {
                islem1();

                var t = Task.Delay(100);
                t.Wait();
                if (process == true)
                {
                    Bitmap bmp;
                    bmp = orjBitmap;
                    int[] histogram_r = new int[256];
                    float max = 0;

                    for (int i = 0; i < bmp.Width; i++)
                    {
                        for (int j = 0; j < bmp.Height; j++)
                        {
                            int greyValue = (bmp.GetPixel(i, j).R + bmp.GetPixel(i, j).G + bmp.GetPixel(i, j).B) / 3;
                            histogram_r[greyValue]++;
                            if (max < histogram_r[greyValue])
                            {
                                max = histogram_r[greyValue];
                            }
                        }
                    }

                    int histHeight = 128;
                    Bitmap img = new Bitmap(256, histHeight + 10);
                    using (Graphics g = Graphics.FromImage(img))
                    {
                        for (int i = 0; i < histogram_r.Length; i++)
                        {
                            float pct = histogram_r[i] / max;   
                            g.DrawLine(Pens.Black,
                                new Point(i, img.Height - 5),
                                new Point(i, img.Height - 5 - (int)(pct * histHeight))  
                                );
                        }
                    }

                    Form2 settingsForm = new Form2(img);

                    settingsForm.Show();
                }
                islem3();
            }

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UsedBitmap != null)
            {
                SaveFileDialog save = new SaveFileDialog();
                save.Filter = "Jpg Image|*.jpg|JPeg Image|*.jpeg|Bitmap Image|*.bmp|Gif Image|*.gif|PNG Image|*.png";
                ImageFormat format = ImageFormat.Png;
                if (save.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string ext = System.IO.Path.GetExtension(save.FileName);
                    switch (ext)
                    {
                        case ".jpg":
                            format = ImageFormat.Jpeg;
                            break;
                        case ".jpeg":
                            format = ImageFormat.Jpeg;
                            break;
                        case ".gif":
                            format = ImageFormat.Gif;
                            break;
                        case ".bmp":
                            format = ImageFormat.Bmp;
                            break;
                        case ".png":
                            format = ImageFormat.Png;
                            break;
                    }
                    UsedBitmap.Save(save.FileName, format);
                }
            }
        }
        public void ChannelControl(Bitmap bmp , object sender , EventArgs e){
            if(redStatus == true)
            {
                redToolStripMenuItem_Click(sender , e);
            }
            if(greenStatus == true)
            {
                greenToolStripMenuItem_Click(sender, e);
            }
            if(blueStatus == true)
            {
                blueToolStripMenuItem_Click(sender , e);
            }
        }

        private void backToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (UsedBitmap != null && stack.Count != 0)
            {
                backToolStripMenuItem.Enabled = true;
                String adim = (String)stack.Pop();

                if (adim == "invert")
                {
                    ınvertToolStripMenuItem_Click(UsedBitmap, e);
                    ChannelControl(UsedBitmap, sender, e);
                    stack.Pop();
                }
                if (adim == "mirror")
                {
                    mirroringToolStripMenuItem_Click(UsedBitmap, e);
                    ChannelControl(UsedBitmap, sender, e);
                    stack.Pop();
                }
                if (adim == "right")
                {
                    solaDöndürmeToolStripMenuItem_Click(UsedBitmap, e);
                    ChannelControl(UsedBitmap, sender, e);
                    stack.Pop();
                }
                if (adim == "left")
                {
                    sağaDöndürmeToolStripMenuItem_Click(UsedBitmap, e);
                    ChannelControl(UsedBitmap, sender, e);
                    stack.Pop();
                }
                if (adim == "grey")
                {
                    greyback(sender, e);
                    ChannelControl(UsedBitmap, sender, e);
                    stack.Pop();
                }
                if (adim == "greyback")
                {
                    greyscaleToolStripMenuItem_Click(sender, e);
                    ChannelControl(UsedBitmap, sender, e);
                    stack.Pop();
                }
                if (adim == "open")
                {
                    pictureBox1.Image = null;
                    UsedBitmap = null;
                    orjBitmap = null;
                    processingBitmap = null;
                    pictureBox1.BackgroundImage = backGraund;

                    reopenToolStripMenuItem.Enabled = false;
                    saveToolStripMenuItem.Enabled = false;
                    backToolStripMenuItem.Enabled = false;
                    ınvertToolStripMenuItem.Enabled = false;
                    mirroringToolStripMenuItem.Enabled = false;
                    sağaDöndürmeToolStripMenuItem.Enabled = false;
                    solaDöndürmeToolStripMenuItem.Enabled = false;
                    greyscaleToolStripMenuItem.Enabled = false;

                    rGBToolStripMenuItem2.Enabled = false;
                    redToolStripMenuItem.Enabled = false;
                    greenToolStripMenuItem.Enabled = false;
                    blueToolStripMenuItem.Enabled = false;

                    rGBToolStripMenuItem1.Enabled = false;
                    redToolStripMenuItem1.Enabled = false;
                    greenToolStripMenuItem1.Enabled = false;
                    blueToolStripMenuItem1.Enabled = false;
                    greyToolStripMenuItem1.Enabled = false;

                }
            }
            if (stack.Count == 0)
            {
                backToolStripMenuItem.Enabled = false;
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lineShape1.X2 = this.Width;
            lineShape1.X1 = 0;
            pictureBox1.Width = this.Width;
            pictureBox1.Height = this.Height;
            pictureBox2.Width = this.Width;
            pictureBox2.Height = this.Height;
            this.MinimumSize = new Size(this.Width, this.Height);
            this.MaximumSize = new Size(this.Width, this.Height);
            pictureBox2.Visible = false;
            button1.Location = new Point(this.Width-45, 0);
            numericUpDown1.Value = 50;
            numericUpDown1.Enabled = false;
            Channel = rGBToolStripMenuItem2.Image;
            rGBToolStripMenuItem2.Image = null;
            Zoom = blueToolStripMenuItem.Image;
            Bitmap newZoom = new Bitmap(Zoom,pictureBox3.Width,pictureBox3.Height);
            blueToolStripMenuItem.Image = null;
            pictureBox3.Image = newZoom;

            reopenToolStripMenuItem.Enabled = false;
            saveToolStripMenuItem.Enabled = false;
            backToolStripMenuItem.Enabled = false;
            ınvertToolStripMenuItem.Enabled = false;
            mirroringToolStripMenuItem.Enabled = false;
            sağaDöndürmeToolStripMenuItem.Enabled = false;
            solaDöndürmeToolStripMenuItem.Enabled = false;
            greyscaleToolStripMenuItem.Enabled = false;

            rGBToolStripMenuItem2.Enabled = false;
            redToolStripMenuItem.Enabled = false;
            greenToolStripMenuItem.Enabled = false;
            blueToolStripMenuItem.Enabled = false;

            rGBToolStripMenuItem1.Enabled = false;
            redToolStripMenuItem1.Enabled = false;
            greenToolStripMenuItem1.Enabled = false;
            blueToolStripMenuItem1.Enabled = false;
            greyToolStripMenuItem1.Enabled = false;
            
            
        }

        private void rGBToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = UsedBitmap;
            redStatus = false;
            greenStatus = false;
            blueStatus = false;

            rGBToolStripMenuItem2.Image = Channel;
            redToolStripMenuItem.Image = null;
            greenToolStripMenuItem.Image = null;
            blueToolStripMenuItem.Image = null;
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            numericValue = (int)numericUpDown1.Value;
            deltaCount = (numericValue - 50);
            Resize(deltaCount);

        }

        private void effectsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Color Corn = Color.FromArgb(255, 248, 220);

                effectsToolStripMenuItem.BackColor = Corn;
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

     }
}
