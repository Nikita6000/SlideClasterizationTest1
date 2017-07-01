using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace SlideClasterizationTest1
{
    delegate Color PixelCalculationDelegate(List<Color> Pixels, Color ReferenceColor);

    public class ImageClusterizer
    {
        public Bitmap OriginalImage { get; set; }

        private byte[] OriginalImagePixelBuffer;
        private int OriginalImageStride;

        private float ImageProportions;
        bool IsWidthLonger;
        Color AverageColor;
        Color BackgroundColor;

        public List<Bitmap> ProcesedImages { get; set; }
        private List<Byte[]> ProcesedImagesPixelBuffer;
        private List<int> ProcesedImagesStride;

        public ImageClusterizer()
        {
            ProcesedImages = new List<Bitmap>();
            ProcesedImagesPixelBuffer = new List<byte[]>();
            ProcesedImagesStride = new List<int>();
            AverageColor = new Color();
        }

        public ImageClusterizer(string pathToImage)
        {
            ProcesedImages = new List<Bitmap>();
            ProcesedImagesPixelBuffer = new List<byte[]>();
            ProcesedImagesStride = new List<int>();

            OriginalImage = new Bitmap(pathToImage);
            ImageProportions = (float)Math.Max(OriginalImage.Width, OriginalImage.Height) / (float)Math.Min(OriginalImage.Width, OriginalImage.Height);

            AverageColor = GetAverageColor(OriginalImage);
            BackgroundColor = GetMostCommonColor(OriginalImage);

            System.Drawing.Imaging.BitmapData OriginalImageData =
                       OriginalImage.LockBits(new Rectangle(0, 0,
                       OriginalImage.Width, OriginalImage.Height),
                       System.Drawing.Imaging.ImageLockMode.ReadOnly,
                       System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            OriginalImagePixelBuffer = new byte[OriginalImageData.Stride * OriginalImageData.Height];

            System.Runtime.InteropServices.Marshal.Copy(OriginalImageData.Scan0, OriginalImagePixelBuffer, 0,
                                                        OriginalImagePixelBuffer.Length);

            OriginalImageStride = OriginalImageData.Stride;

            OriginalImage.UnlockBits(OriginalImageData);


            if (OriginalImage.Width > OriginalImage.Height)
            {
                IsWidthLonger = true;
            }
            else
            {
                IsWidthLonger = false;
            }
        }

        public ImageClusterizer(Bitmap originalImage)
        {
            ProcesedImages = new List<Bitmap>();
            ProcesedImagesPixelBuffer = new List<byte[]>();
            ProcesedImagesStride = new List<int>();

            OriginalImage = originalImage;
            ImageProportions = (float)Math.Max(OriginalImage.Width, OriginalImage.Height) / (float)Math.Min(OriginalImage.Width, OriginalImage.Height);

            AverageColor = GetAverageColor(OriginalImage);
            BackgroundColor = GetMostCommonColor(OriginalImage);

            System.Drawing.Imaging.BitmapData OriginalImageData =
                       OriginalImage.LockBits(new Rectangle(0, 0,
                       OriginalImage.Width, OriginalImage.Height),
                       System.Drawing.Imaging.ImageLockMode.ReadOnly,
                       System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            OriginalImagePixelBuffer = new byte[OriginalImageData.Stride * OriginalImageData.Height];

            System.Runtime.InteropServices.Marshal.Copy(OriginalImageData.Scan0, OriginalImagePixelBuffer, 0,
                                                        OriginalImagePixelBuffer.Length);

            OriginalImageStride = OriginalImageData.Stride;

            OriginalImage.UnlockBits(OriginalImageData);
        }

        public async Task GenerateImagesAsync()
        {
            await Task.Factory.StartNew(() => GenerateImages());
        }
        
        public void GenerateImages()
        {
            if(OriginalImage != null)
            {
                for (int i = 3; i < Math.Min(OriginalImage.Width, OriginalImage.Height) / 4.0; i = (int)(i * 1.4))
                {
                    CompressImage(i, GetTheFurthestColor);
                }
            }
        }

        // resize image without blurry distortions on pixels (for correct visualization in winforms)
        public static Image ResizeImage(Image img, Size size)
        {
            var bmp = new Bitmap(size.Width, size.Height);
            using (var gr = Graphics.FromImage(bmp))
            {
                gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                gr.DrawImage(img, new Rectangle(Point.Empty, size));
            }
            return bmp;
        }

        // fast filter on lockbits
        private void CompressImage(int PixelsOnShorterEdge, PixelCalculationDelegate CalculateColor)
        {
            Bitmap NewImage;
            
            if(OriginalImage.Width < OriginalImage.Height)
                NewImage = new Bitmap(PixelsOnShorterEdge, (int)(((float)OriginalImage.Height / (float)OriginalImage.Width) * PixelsOnShorterEdge + 0.5f));
            else
                NewImage = new Bitmap((int)(((float)OriginalImage.Width / (float)OriginalImage.Height) * PixelsOnShorterEdge + 0.5f), PixelsOnShorterEdge);

            
            System.Drawing.Imaging.BitmapData NewImageData =
                       NewImage.LockBits(new Rectangle(0, 0,
                       NewImage.Width, NewImage.Height),
                       System.Drawing.Imaging.ImageLockMode.WriteOnly,
                       System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            

            byte[] NewImagePixelBuffer = new byte[NewImageData.Stride * NewImageData.Height];
            
            int filterOffset = (int)(((float)Math.Min(OriginalImage.Height, OriginalImage.Width) / (float)PixelsOnShorterEdge) / 2.0 + 0.5f);
            int calcOffset = 0;
            int OriginalImageByteOffset = 0;
            int NewImageByteOffset = 0;

            Color NewColor = new Color();
            List<Color> neighbourPixels = new List<Color>();

            for (int offsetY = filterOffset; offsetY < OriginalImage.Height - 2 * filterOffset + 1; offsetY += 2 * filterOffset + 1)
            {
                for (int offsetX = filterOffset; offsetX < OriginalImage.Width - 2 * filterOffset + 1; offsetX += 2 * filterOffset + 1)
                {
                    // find a byte where "the center" of calculated pixel will be
                    OriginalImageByteOffset = offsetY * OriginalImageStride + offsetX * 4;

                    neighbourPixels.Clear();

                    // iterate over all pixels in original image, which are "covered" by a larger pixel of new image
                    for (int filterY = -filterOffset; filterY <= filterOffset; filterY++)
                    {
                        for (int filterX = -filterOffset; filterX <= filterOffset; filterX++)
                        {
                            // find needed pixel in array of bytes
                            calcOffset = (offsetX + filterX) * 4 + (offsetY + filterY) * OriginalImageStride;

                            // save its value in neighbourPixels list
                            neighbourPixels.Add(Color.FromArgb(OriginalImagePixelBuffer[calcOffset + 3],
                                                               OriginalImagePixelBuffer[calcOffset + 2],
                                                               OriginalImagePixelBuffer[calcOffset + 1],
                                                               OriginalImagePixelBuffer[calcOffset]));
                        }
                    }

                    // call a given method of color calculation
                    NewColor = CalculateColor(neighbourPixels, BackgroundColor);

                    NewImageByteOffset = (int)(((float)(offsetY - filterOffset) / (2.0 * filterOffset + 1.0)) * NewImageData.Stride + 
                                               ((float)(offsetX - filterOffset) / (2.0 * filterOffset + 1.0)) * 4.0);

                    // save founded color in new image's pixel byte array
                    NewImagePixelBuffer[NewImageByteOffset] = NewColor.B;
                    NewImagePixelBuffer[NewImageByteOffset + 1] = NewColor.G;
                    NewImagePixelBuffer[NewImageByteOffset + 2] = NewColor.R;
                    NewImagePixelBuffer[NewImageByteOffset + 3] = NewColor.A;
                }
            }

            // copy created byte array into an actual new image
            System.Runtime.InteropServices.Marshal.Copy(NewImagePixelBuffer, 0, NewImageData.Scan0,
                                       NewImagePixelBuffer.Length);
            
            NewImage.UnlockBits(NewImageData);

            // save procesed image
            ProcesedImages.Add(new Bitmap(ResizeImage(NewImage, new Size(OriginalImage.Width, OriginalImage.Height))));
            ProcesedImagesPixelBuffer.Add(NewImagePixelBuffer);
            ProcesedImagesStride.Add(NewImageData.Stride);
        }

        private static Color GetTheFurthestColor(List<Color> neighbourPixels, Color ReferenceColor)
        {
            Color TheFurthestColor = new Color();

            int MaxValue = -1;
            int TempSquare = 0;

            // find the furthest from average colored pixel in neigbourPixels
            for (int i = 0; i < neighbourPixels.Count; i++)
            {
                TempSquare = (neighbourPixels[i].R - ReferenceColor.R) *
                             (neighbourPixels[i].R - ReferenceColor.R) +
                             (neighbourPixels[i].G - ReferenceColor.G) *
                             (neighbourPixels[i].G - ReferenceColor.G) +
                             (neighbourPixels[i].B - ReferenceColor.B) *
                             (neighbourPixels[i].B - ReferenceColor.B);

                if (TempSquare > MaxValue)
                {
                    MaxValue = TempSquare;
                    TheFurthestColor = neighbourPixels[i];
                }
            }

            return TheFurthestColor;
        }

        private static Color GetAverageColor(List<Color> neighbourPixels, Color ReferenceColor)
        {
            Color AverageColor;

            int R = 0, G = 0, B = 0, A = 0, Counter = 0;

            for (int i = 0; i < neighbourPixels.Count; i++)
            {
                A += neighbourPixels[i].A;
                R += neighbourPixels[i].R;
                G += neighbourPixels[i].G;
                B += neighbourPixels[i].B;
                Counter++;               
            }

            A = A / Counter;
            R = R / Counter;
            G = G / Counter;
            B = B / Counter;

            AverageColor = Color.FromArgb(A, R, G, B);

            return AverageColor;
        }

        private static Color GetAverageColor(List<Color> neighbourPixels)
        {
            Color AverageColor;

            int R = 0, G = 0, B = 0, A = 0, Counter = 0;

            for (int i = 0; i < neighbourPixels.Count; i++)
            {
                A += neighbourPixels[i].A;
                R += neighbourPixels[i].R;
                G += neighbourPixels[i].G;
                B += neighbourPixels[i].B;
                Counter++;
            }

            A = A / Counter;
            R = R / Counter;
            G = G / Counter;
            B = B / Counter;

            AverageColor = Color.FromArgb(A, R, G, B);

            return AverageColor;
        }

        private static Color GetAverageColor(Bitmap Image)
        {
            Color AverageColor;

            System.Drawing.Imaging.BitmapData ImageData =
                       Image.LockBits(new Rectangle(0, 0,
                       Image.Width, Image.Height),
                       System.Drawing.Imaging.ImageLockMode.ReadOnly,
                       System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            byte[] PixelBuffer = new byte[ImageData.Stride * ImageData.Height];

            System.Runtime.InteropServices.Marshal.Copy(ImageData.Scan0, PixelBuffer, 0, PixelBuffer.Length);

            Image.UnlockBits(ImageData);

            int R = 0, G = 0, B = 0, A = 0, Counter = 0;

            for (int i = 0; i < PixelBuffer.Length - 3; i += 3)
            {
                A += PixelBuffer[i + 3];
                R += PixelBuffer[i + 2];
                G += PixelBuffer[i + 1];
                B += PixelBuffer[i];
                Counter++;
            }

            A = A / Counter;
            R = R / Counter;
            G = G / Counter;
            B = B / Counter;

            AverageColor = Color.FromArgb(A, R, G, B);

            return AverageColor;
        }
        
        // most frequently used color in image
        private static Color GetMostCommonColor(Bitmap Image)
        {
            Color CurentColor = new Color();
            Dictionary<Color, int> Colors = new Dictionary<Color, int>();

            System.Drawing.Imaging.BitmapData ImageData =
                       Image.LockBits(new Rectangle(0, 0,
                       Image.Width, Image.Height),
                       System.Drawing.Imaging.ImageLockMode.ReadOnly,
                       System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            byte[] PixelBuffer = new byte[ImageData.Stride * ImageData.Height];

            System.Runtime.InteropServices.Marshal.Copy(ImageData.Scan0, PixelBuffer, 0, PixelBuffer.Length);

            Image.UnlockBits(ImageData);
            
            for (int i = 0; i < PixelBuffer.Length - 3; i += 3)
            {
                CurentColor = Color.FromArgb(PixelBuffer[i + 3], PixelBuffer[i + 2], PixelBuffer[i + 1], PixelBuffer[i]);

                if (Colors.ContainsKey(CurentColor))
                    Colors[CurentColor]++;
                else
                    Colors.Add(CurentColor, 1);                
            }        

            return Colors.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
        }
    }
}
