using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MarriedMoles
{
    public partial class Form1 : Form
    {
        Simulator simulator;
        Bitmap bitmap;
        int width = 150;
        int height = 150;

        public Form1()
        {
            InitializeComponent(); // write all code afterwards

            simulator = new Simulator(width, height); // choose size depending on pc power (grows exponentially)

            simulator.StepDone += Simulator_StepDone; // when you finish the step - call stepdone method below;

            bitmap = new Bitmap(width, height); // one bitmap pixel equals one simulation field square
        }
        private void Simulator_StepDone(Field field)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    var actor = field.GetActorAt(i, j);
                    var color = Color.White; //or Color.FromArgb(255,255,255)

                    //if (i % 2 ==0 ^ j % 2 ==0) color = Color.Black; // grieztai ARBA = ^

                    
                    
                     if (actor is Bomb)
                    {
                        color = Color.Red;
                    }
                    else if (actor is MaleMole)
                    {
                        var male = actor as MaleMole;
                        if (male.cheater)
                        {
                            color = Color.YellowGreen;
                        }
                        else if (male.isMarried)
                        {
                            color = Color.Blue;
                        }
                        else
                        {
                            color = Color.Cyan;
                        }
                    }
                    else if (actor is FemaleMole)
                    {
                        var female = actor as FemaleMole;
                        if (female.isMarried)
                        {
                            color = Color.HotPink;
                        }
                        else
                        {
                            color = Color.Pink;
                        }

                    }
                    
                    else if (actor is Fox)
                    {
                        color = Color.DarkGray;
                    }
                    else if (actor is Rabbit)
                    {
                        color = Color.Orange;
                    }

                    bitmap.SetPixel(i, j, color);
                }
            }
            Application.DoEvents(); // makes sure that you can click stuff while program is running
            this.panel1.CreateGraphics().DrawImage(ResizeImage(bitmap, panel1.Width, panel1.Height), new Point(0, 0));

        }
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                graphics.SmoothingMode = SmoothingMode.None;
                graphics.PixelOffsetMode = PixelOffsetMode.None;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            simulator.RunOneStep();
        }

        

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (int.TryParse(textBox1.Text, out var skaicius))
                simulator.Run(skaicius);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            simulator.Reset();
        }

        
    }
}
