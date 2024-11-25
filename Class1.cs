using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIP_Coins_Activity
{
    public class Coins
    {
        public const float PESO_5 = 5f;
        public const float PESO_1 = 1f;
        public const float CENT_25 = .25f;
        public const float CENT_10 = .10f;
        public const float CENT_5 = .05f;

        //public static void CountCoin(Bitmap bmp, ref Label countLabel, ref Label valueLabel)
        public static void CountCoin(Bitmap bmp, ref Label valueLabel)
        {
            int count = 0; //Expected 64
            float value = 0; //Expected 46.45

            int height = bmp.Height;
            int width = bmp.Width;

            bool[,] visited = new bool[height, width];

            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                {
                    Color pixel = bmp.GetPixel(j, i);
                    if (!visited[i, j] && pixel.R == 0)
                    {
                        count++;
                        value += Classify(BFS(bmp, ref visited, j, i), ref count);
                    }
                }

            //countLabel.Text = count.ToString();
            valueLabel.Text = value.ToString("F2");
        }

        private static float Classify(int pixel, ref int count)
        {
            if (pixel >= 18000)
                return PESO_5;
            else if (pixel >= 15000)
                return PESO_1;
            else if (pixel >= 11000)
                return CENT_25;
            else if (pixel >= 8000)
                return CENT_10;
            else if (pixel >= 6500)
                return CENT_5;

            count--;
            return 0;
        }

        private static int BFS(Bitmap bmp, ref bool[,] visited, int x, int y)
        {
            int height = bmp.Height;
            int width = bmp.Width;
            int count = 0;

            Queue<Point> queue = new();
            queue.Enqueue(new Point(x, y));

            while (queue.Count > 0)
            {
                Point point = queue.Dequeue();
                int dx = point.X, dy = point.Y;

                if (visited[dy, dx])
                    continue;

                count++;

                visited[dy, dx] = true;

                if (dx - 1 >= 0 && bmp.GetPixel(dx - 1, dy).R == 0 && !visited[dy, dx - 1])
                    queue.Enqueue(new Point(dx - 1, dy));
                if (dx + 1 < width && bmp.GetPixel(dx + 1, dy).R == 0 && !visited[dy, dx + 1])
                    queue.Enqueue(new Point(dx + 1, dy));
                if (dy - 1 >= 0 && bmp.GetPixel(dx, dy - 1).R == 0 && !visited[dy - 1, dx])
                    queue.Enqueue(new Point(dx, dy - 1));
                if (dy + 1 < height && bmp.GetPixel(dx, dy + 1).R == 0 && !visited[dy + 1, dx])
                    queue.Enqueue(new Point(dx, dy + 1));
            }

            return count;
        }
    }

    public class Filter
    {
        public static bool Binary(Bitmap src, int threshold)
        {
            if (threshold < 0 || threshold > 255)
                return false;

            int srcHeight = src.Height;
            int srcWidth = src.Width;

            BitmapData bmLoaded = src.LockBits(
                new Rectangle(0, 0, srcWidth, srcHeight),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb
            );

            unsafe
            {
                int padding = bmLoaded.Stride - srcWidth * 3;

                byte* pLoaded = (byte*)bmLoaded.Scan0;

                for (int i = 0; i < srcHeight; i++, pLoaded += padding)
                {
                    for (int j = 0; j < srcWidth; j++, pLoaded += 3)
                    {
                        byte gray = (byte)((pLoaded[0] + pLoaded[1] + pLoaded[2]) / 3);
                        byte binaryValue = (byte)(gray < threshold ? 0 : 255);
                        pLoaded[0] = pLoaded[1] = pLoaded[2] = binaryValue;
                    }
                }
            }

            src.UnlockBits(bmLoaded);

            return true;
        }
    }
}
