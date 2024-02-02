using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;

namespace Grafika_lab_1_TK
{
    public class CanvasExtensions
    {
        private Canvas _canvas;

        public CanvasExtensions(Canvas canvas)
        {
            _canvas = canvas ?? throw new ArgumentNullException(nameof(canvas));
        }

        public BitmapSource ToBitmapSource()
        {
            Rect bounds = VisualTreeHelper.GetDescendantBounds(_canvas);
            RenderTargetBitmap rtb = new RenderTargetBitmap((int)bounds.Width, (int)bounds.Height, 96, 96, PixelFormats.Pbgra32);

            DrawingVisual dv = new DrawingVisual();
            using (DrawingContext dc = dv.RenderOpen())
            {
                VisualBrush vb = new VisualBrush(_canvas);
                dc.DrawRectangle(vb, null, new Rect(new Point(), bounds.Size));
            }

            rtb.Render(dv);

            return rtb;
        }

        public static void ApplyConvolutionFilter(WriteableBitmap bitmap, double[,] kernel)
        {
            if (bitmap == null || kernel.GetLength(0) != 3 || kernel.GetLength(1) != 3)
                throw new ArgumentException("Invalid bitmap or kernel matrix");

            int width = bitmap.PixelWidth;
            int height = bitmap.PixelHeight;

            int stride = width * (bitmap.Format.BitsPerPixel / 8);
            byte[] pixels = new byte[height * stride];
            bitmap.CopyPixels(pixels, stride, 0);

            byte[] resultPixels = new byte[height * stride];

            int kernelSize = 3;
            int kernelRadius = kernelSize / 2;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    double red = 0, green = 0, blue = 0;

                    for (int i = 0; i < kernelSize; i++)
                    {
                        for (int j = 0; j < kernelSize; j++)
                        {
                            int offsetX = x + i - kernelRadius;
                            int offsetY = y + j - kernelRadius;

                            offsetX = Math.Max(0, Math.Min(width - 1, offsetX));
                            offsetY = Math.Max(0, Math.Min(height - 1, offsetY));

                            int pixelIndex = offsetY * stride + offsetX * 4;

                            red += pixels[pixelIndex + 2] * kernel[i, j];
                            green += pixels[pixelIndex + 1] * kernel[i, j];
                            blue += pixels[pixelIndex] * kernel[i, j];
                        }
                    }

                    int resultIndex = y * stride + x * 4;
                    resultPixels[resultIndex + 2] = (byte)Math.Max(0, Math.Min(255, red));
                    resultPixels[resultIndex + 1] = (byte)Math.Max(0, Math.Min(255, green));
                    resultPixels[resultIndex] = (byte)Math.Max(0, Math.Min(255, blue));
                    resultPixels[resultIndex + 3] = 255; // Alpha channel
                }
            }

            bitmap.WritePixels(new Int32Rect(0, 0, width, height), resultPixels, stride, 0);
        }
    }
}
