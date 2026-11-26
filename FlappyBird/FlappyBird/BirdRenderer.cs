using System.Drawing;

namespace FlappyBird
{
    public static class BirdRenderer
    {
        public static void Draw(Graphics g, Rectangle body, System.Drawing.Color color)
        {
            int x = body.X;
            int y = body.Y;
            int w = body.Width;
            int h = body.Height;

            // Gövde
            using (Brush bodyBrush = new SolidBrush(color))
            {
                g.FillEllipse(bodyBrush, x, y, w, h);
            }

            System.Drawing.Color wingColor = Brightness(color) > 128
     ? System.Drawing.Color.FromArgb(
         (int)(color.R * 0.7), (int)(color.G * 0.7), (int)(color.B * 0.7))   // açık gövde: kanat koyu
     : System.Drawing.Color.FromArgb(
         Math.Min(255, (int)(color.R + (255 - color.R) * 0.5)),
         Math.Min(255, (int)(color.G + (255 - color.G) * 0.5)),
         Math.Min(255, (int)(color.B + (255 - color.B) * 0.5)));  // koyu gövde: kanat açık

            using (Brush wingBrush = new SolidBrush(wingColor))
            {
                g.FillEllipse(wingBrush, x + w / 6, y + h / 2, w / 2, h / 4);
            }

            // Göz (beyaz zemin + siyah bebek)
            int eyeSize = w / 4;
            int eyeX = x + w * 2 / 3;
            int eyeY = y + h / 4;
            g.FillEllipse(Brushes.White, eyeX, eyeY, eyeSize, eyeSize);
            g.FillEllipse(Brushes.Black, eyeX + eyeSize / 3, eyeY + eyeSize / 3, eyeSize / 2, eyeSize / 2);

            // Gaga (sağa bakan üçgen)
            Point[] beak =
            {
                new Point(x + w - 2, y + h / 2 - 3),
                new Point(x + w + 8, y + h / 2 + 2),
                new Point(x + w - 2, y + h / 2 + 7)
            };
            g.FillPolygon(Brushes.Orange, beak);
        }
        private static double Brightness(Color c)
        {
            return 0.299 * c.R + 0.587 * c.G + 0.114 * c.B;
        }
    }
}