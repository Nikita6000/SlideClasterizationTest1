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

        // function to create a cluster by moving one pixel at a time on the Map
        public void CreateCluster(int xStart, int yStart)
        {
            List<Point> Junctions = new List<Point>();

            bool IsThereAnywhereToGo = true;
            int CurentX = xStart, CurentY = yStart;

            while(IsThereAnywhereToGo)
            {
                if (ByteCoordinates.Contains((CurentY * Map.Length + CurentX) * 4) == false)
                {
                    ByteCoordinates.Add((yStart * Map.Length + xStart) * 4);
                    Size++;
                }

                Map[CurentX][CurentY] = false;

                if (CurentX > 0 && Map[CurentX - 1][CurentY]) 
                {
                    if ((CurentY > 0 && Map[CurentX][CurentY - 1]) ||
                       (CurentY < Map[0].Length - 1 && Map[CurentX][CurentY + 1]) ||
                       (CurentX < Map.Length - 1 && Map[CurentX + 1][CurentY]))
                        Junctions.Add(new Point(CurentX, CurentY));

                    CurentX--;
                    continue;
                }

                if (CurentX < Map.Length - 1 && Map[CurentX + 1][CurentY])
                {
                    if ((CurentY > 0 && Map[CurentX][CurentY - 1]) ||
                       (CurentY < Map[0].Length - 1 && Map[CurentX][CurentY + 1]) ||
                       (CurentX > 0 && Map[CurentX - 1][CurentY]))
                        Junctions.Add(new Point(CurentX, CurentY));

                    CurentX++;
                    continue;
                }

                if (CurentY > 0 && Map[CurentX][CurentY - 1])
                {
                    if ((CurentX > 0 && Map[CurentX - 1][CurentY]) ||
                       (CurentY < Map[0].Length - 1 && Map[CurentX][CurentY + 1]) ||
                       (CurentX < Map.Length - 1 && Map[CurentX + 1][CurentY]))
                        Junctions.Add(new Point(CurentX, CurentY));

                    CurentY--;
                    continue;
                }

                if (CurentY < Map[0].Length - 1 && Map[CurentX][CurentY + 1])
                {
                    if ((CurentY > 0 && Map[CurentX][CurentY - 1]) ||
                       (CurentX < Map.Length - 1 && Map[CurentX + 1][CurentY]) ||
                       (CurentX > 0 && Map[CurentX - 1][CurentY]))
                        Junctions.Add(new Point(CurentX, CurentY));

                    CurentY++;
                    continue;
                }

                if (Junctions.Count > 0)
                {
                    CurentX = Junctions.First().X;
                    CurentY = Junctions.First().Y;
                    Junctions.RemoveAt(0);
                }
                else IsThereAnywhereToGo = false;
            }
        }

        // creates a map of pixels, which show where we can (true) or can't (false) go
        public static bool[][] CreatMap(byte[] PixelBuffer, int Stride, Color BackgroundColor)
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
                    (A.B - B.B) * (A.B - B.B) < ColorSimilarityMargin);
        }
    }
}
