using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace SlideClasterizationTest1
{
    public class PixelCluster
    {
        // bytes of pixels, which belong to this cluster
        public List<int> ByteCoordinates;

        // map of pixels, where we can (true) or can't (false) go
        public bool[][] Map;

        public int Size;
        public float x, y;

        // look "IsTheSameColors" method. 
        static float ColorSimilarityMargin;

        static PixelCluster()
        {
            // this realy shoold be in app.config....
            ColorSimilarityMargin = 30;
        }

        public PixelCluster()
        {
            ByteCoordinates = new List<int>();
        }

        public PixelCluster(bool[][] map)
        {
            ByteCoordinates = new List<int>();
            Map = map;
        }

        // recursive function to create a cluster by moving one pixel at a time on the Map
        public void CreateCluster(int xStart, int yStart)
        {
            if(Map[xStart][yStart])
            {
                ByteCoordinates.Add((yStart * Map.Length + xStart) * 4);
                Map[xStart][yStart] = false;

                if (xStart > 0 && Map[xStart - 1][yStart])
                {
                    CreateCluster(xStart - 1, yStart);
                }

                if (yStart > 0 && Map[xStart][yStart - 1])
                {
                    CreateCluster(xStart, yStart - 1);
                }

                if (xStart < Map.Length - 1 && Map[xStart + 1][yStart])
                {
                    CreateCluster(xStart + 1, yStart);
                }

                if (yStart < Map[0].Length - 1 && Map[xStart][yStart + 1])
                {
                    CreateCluster(xStart, yStart + 1);
                }
            }
        }

        // creates a map of pixels, which show where we can (true) or can't (false) go
        static bool[][] CreatMap(byte[] PixelBuffer, int Stride, Color BackgroundColor)
        {
            bool[][] Map = new bool[Stride / 4][];

            for (int i = 0; i < Map.Length; i++)
            {
                Map[i] = new bool[PixelBuffer.Length / Stride];
            }

            int Offset = 0;

            for (int x = 0; x < Stride / 4; x++)
            {
                for (int y = 0; y < PixelBuffer.Length / Stride; y++)
                {
                    Offset = x * 4 + y * Stride;
                    if (IsTheSameColors(Color.FromArgb(PixelBuffer[Offset + 3], PixelBuffer[Offset + 2], PixelBuffer[Offset + 1], PixelBuffer[Offset]),
                                       BackgroundColor) == false)
                        Map[x][y] = true;
                    else Map[x][y] = false;
                }
            }

            return Map;
        }

        // right now this is true only if colors are exactly the same
        static bool IsTheSameColors(Color A, Color B)
        {
            return ((A.A - B.A) * (A.A - B.A) +
                    (A.R - B.R) * (A.R - B.R) +
                    (A.G - B.G) * (A.G - B.G) +
                    (A.B - B.B) * (A.B - B.B) == 0);
        }
    }
}
