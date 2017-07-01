using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SlideClasterizationTest1
{
    public partial class Form1 : Form
    {
        List<ImageClusterizer> Images;
        int CurentImage = 0;
        int CurentProcessedImage = 0;

        public Form1()
        {
            InitializeComponent();
            Images = new List<ImageClusterizer>();
        }

        private async void buttonAddImage_Click(object sender, EventArgs e)
        {
            await AddImage();

            pictureBox1.Image = Images.Last().OriginalImage;
            pictureBox2.Image = Images.Last().ProcesedImages[CurentProcessedImage];

            chart1.Series[0].Points.Clear();
            for (int i = 0; i < Images[CurentImage].Clusters.Count; i++)
            {
                chart1.Series[0].Points.AddXY(i, Images[CurentImage].Clusters[i].Count);
            }
            chart1.Update();
        }

        private async Task AddImage()
        {
            openFileDialog1.InitialDirectory = System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName; ;
            openFileDialog1.Title = "Open Image";
            openFileDialog1.Filter = "Image files (*.png)|*.png";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                ImageClusterizer NewImage = new ImageClusterizer(openFileDialog1.FileName);
                await NewImage.GenerateImagesAsync();
                Images.Add(NewImage);
                await NewImage.GenerateClustersAsync();
            }
        }

        private void buttonNextProcessedImage_Click(object sender, EventArgs e)
        {
            if (Images.Count != 0 && Images[CurentImage].ProcesedImages.Count != 0)
            {
                if(CurentProcessedImage + 1 != Images[CurentImage].ProcesedImages.Count)
                {
                    CurentProcessedImage++;
                    pictureBox2.Image = Images[CurentImage].ProcesedImages[CurentProcessedImage];
                }
                else
                {
                    CurentProcessedImage = 0;
                    pictureBox2.Image = Images[CurentImage].ProcesedImages[CurentProcessedImage];
                }
            }
        }
        
        private void buttonPreviousProcessedImage_Click(object sender, EventArgs e)
        {
            if (Images.Count != 0 && Images[CurentImage].ProcesedImages.Count != 0)
            {
                if (CurentProcessedImage != 0)
                {
                    CurentProcessedImage--;
                    pictureBox2.Image = Images[CurentImage].ProcesedImages[CurentProcessedImage];
                }
                else
                {
                    CurentProcessedImage = Images[CurentImage].ProcesedImages.Count - 1;
                    pictureBox2.Image = Images[CurentImage].ProcesedImages[CurentProcessedImage];
                }
            }
        }

        private void buttonNextImage_Click(object sender, EventArgs e)
        {
            if (Images.Count != 0 && CurentImage < Images.Count - 1)
            {
                CurentImage++;
                pictureBox1.Image = Images[CurentImage].OriginalImage;

                chart1.Series[0].Points.Clear();
                for (int i = 0; i < Images[CurentImage].Clusters.Count; i++)
                {
                    chart1.Series[0].Points.AddXY(i, Images[CurentImage].Clusters[i].Count);
                }
                chart1.Update();

                if (Images[CurentImage].ProcesedImages.Count != 0 && CurentProcessedImage < Images[CurentImage].ProcesedImages.Count)
                {
                    pictureBox2.Image = Images[CurentImage].ProcesedImages[CurentProcessedImage];
                }
                else if (Images[CurentImage].ProcesedImages.Count != 0)
                {
                    CurentProcessedImage = 0;
                    pictureBox2.Image = Images[CurentImage].ProcesedImages[CurentProcessedImage];
                }
            }
            else if (Images.Count != 0 && CurentImage == Images.Count - 1)
            {
                CurentImage = 0;
                pictureBox1.Image = Images[CurentImage].OriginalImage;

                chart1.Series[0].Points.Clear();
                for (int i = 0; i < Images[CurentImage].Clusters.Count; i++)
                {
                    chart1.Series[0].Points.AddXY(i, Images[CurentImage].Clusters[i].Count);
                }
                chart1.Update();

                if (Images[CurentImage].ProcesedImages.Count != 0 && CurentProcessedImage < Images[CurentImage].ProcesedImages.Count)
                {
                    pictureBox2.Image = Images[CurentImage].ProcesedImages[CurentProcessedImage];
                }
                else if (Images[CurentImage].ProcesedImages.Count != 0)
                {
                    CurentProcessedImage = 0;
                    pictureBox2.Image = Images[CurentImage].ProcesedImages[CurentProcessedImage];
                }
            }
        }

        private void buttonPreviousImage_Click(object sender, EventArgs e)
        {
            if (Images.Count != 0 && CurentImage > 0)
            {
                CurentImage--;
                pictureBox1.Image = Images[CurentImage].OriginalImage;

                chart1.Series[0].Points.Clear();
                for (int i = 0; i < Images[CurentImage].Clusters.Count; i++)
                {
                    chart1.Series[0].Points.AddXY(i, Images[CurentImage].Clusters[i].Count);
                }
                chart1.Update();

                if (Images[CurentImage].ProcesedImages.Count != 0 && CurentProcessedImage < Images[CurentImage].ProcesedImages.Count)
                {
                    pictureBox2.Image = Images[CurentImage].ProcesedImages[CurentProcessedImage];
                }
                else if (Images[CurentImage].ProcesedImages.Count != 0)
                {
                    CurentProcessedImage = 0;
                    pictureBox2.Image = Images[CurentImage].ProcesedImages[CurentProcessedImage];
                }
            }
            else if (Images.Count != 0 && CurentImage == 0)
            {
                CurentImage = Images.Count - 1;
                pictureBox1.Image = Images[CurentImage].OriginalImage;

                chart1.Series[0].Points.Clear();
                for (int i = 0; i < Images[CurentImage].Clusters.Count; i++)
                {
                    chart1.Series[0].Points.AddXY(i, Images[CurentImage].Clusters[i].Count);
                }
                chart1.Update();

                if (Images[CurentImage].ProcesedImages.Count != 0 && CurentProcessedImage < Images[CurentImage].ProcesedImages.Count)
                {
                    pictureBox2.Image = Images[CurentImage].ProcesedImages[CurentProcessedImage];
                }
                else if (Images[CurentImage].ProcesedImages.Count != 0)
                {
                    CurentProcessedImage = 0;
                    pictureBox2.Image = Images[CurentImage].ProcesedImages[CurentProcessedImage];
                }
            }
        }
        
    }
}
