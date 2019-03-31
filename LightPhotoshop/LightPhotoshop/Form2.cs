using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LightPhotoshop
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        public Form2(Image img2)
        {
            InitializeComponent();

            Bitmap newBmp = new Bitmap(img2);
            Size newSize = new Size(pictureBox2.Width, pictureBox2.Height);
            newBmp = new Bitmap(img2, newSize);
            this.pictureBox2.Image = newBmp;

        }

    }
}
