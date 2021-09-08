using System;
using System.Drawing;

namespace Equirectangular
{
    class Program
    {
        static void Main(string[] args)
        {
            var outImgWidth = 4096;
            var outImgHeight = 2048;
            var cubeFaceWidth = 512;
            var cubeFaceHeight = 512;

            var outImg = new Bitmap(outImgWidth, outImgHeight);

            var xFace = new Bitmap("right.jpg");
            var nxFace = new Bitmap("left.jpg");
            var yFace = new Bitmap("bottom.jpg");
            var nyFace = new Bitmap("top.jpg");
            var zFace = new Bitmap("front.jpg");
            var nzFace = new Bitmap("back.jpg");

            double u, v;
            double phi, theta;

            for (int j = 0; j < outImg.Height; j++)
            {
                v = 1 - ((float)j / outImg.Height);
                theta = v * Math.PI;
                for (int i = 0; i < outImg.Width; i++)
                {
                    u = ((float)i / outImg.Width);
                    phi = u * 2 * Math.PI;

                    double x, y, z; 
                    x = Math.Sin(phi) * Math.Sin(theta) * -1;
                    y = Math.Cos(theta);
                    z = Math.Cos(phi) * Math.Sin(theta) * -1;

                    double xa, ya, za;
                    double a;

                    a = Math.Max(Math.Max(Math.Abs(x), Math.Abs(y)), Math.Abs(z));

                    xa = x / a;
                    ya = y / a;
                    za = z / a;

                    Color color;
                    int xPixel, yPixel;
                    Bitmap face;
                    if (xa == 1)
                    {
                        xPixel = (int)((((za + 1f) / 2f) - 1f) * cubeFaceWidth);
                        yPixel = (int)((((ya + 1f) / 2f)) * cubeFaceHeight);
                        face = xFace;
                    }
                    else if (xa == -1)
                    {
                        xPixel = (int)((((za + 1f) / 2f)) * cubeFaceWidth);
                        yPixel = (int)((((ya + 1f) / 2f)) * cubeFaceHeight);
                        face = nxFace;
                    }
                    else if (ya == 1)
                    {
                        xPixel = (int)((((xa + 1f) / 2f)) * cubeFaceWidth);
                        yPixel = (int)((((za + 1f) / 2f) - 1f) * cubeFaceHeight);
                        face = yFace;
                    }
                    else if (ya == -1)
                    {
                        xPixel = (int)((((xa + 1f) / 2f)) * cubeFaceWidth);
                        yPixel = (int)((((za + 1f) / 2f)) * cubeFaceHeight);
                        face = nyFace;
                    }
                    else if (za == 1)
                    {
                        xPixel = (int)((((xa + 1f) / 2f)) * cubeFaceWidth);
                        yPixel = (int)((((ya + 1f) / 2f)) * cubeFaceHeight);
                        face = zFace;
                    }
                    else if (za == -1)
                    {
                        xPixel = (int)((((xa + 1f) / 2f) - 1f) * cubeFaceWidth);
                        yPixel = (int)((((ya + 1f) / 2f)) * cubeFaceHeight);
                        face = nzFace;
                    }
                    else
                    {
                        throw new Exception("Unknown face, something went wrong");
                    }

                    color = face.GetPixel(Math.Clamp(Math.Abs(xPixel), 0, cubeFaceWidth - 1), Math.Clamp(Math.Abs(yPixel), 0, cubeFaceHeight - 1));
                    outImg.SetPixel(i, j, color);
                }
            }
            outImg.Save("out.jpg", System.Drawing.Imaging.ImageFormat.Png);
        }
    }
}
