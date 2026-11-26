using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Text;

namespace FlappyBird
{
    public enum TimeOfDay
    {
        Day,
        Evening,
        Night
    }
    internal class Background
    {
        public static TimeOfDay GetTimeOfDay()
        {
            int hour = DateTime.Now.Hour;

            if (hour >= 6 && hour < 17) return TimeOfDay.Day;
            if (hour >= 17 && hour < 20) return TimeOfDay.Evening;
            return TimeOfDay.Night;
        }
        public static void Draw(Graphics g, int width, int height, float cloudOffset)
        {
            switch (GetTimeOfDay())
            {
                case TimeOfDay.Day:
                    DrawDay(g, width, height, cloudOffset);
                    break;
                case TimeOfDay.Evening:
                    DrawEvening(g, width, height);
                    break;
                default:
                    DrawNight(g, width, height);
                    break;
            }
        }
        private static void DrawDay(Graphics g, int width, int height, float cloudOffset)
        {
            // Açık mavi gradyan gökyüzü (üstten alta)
            using (LinearGradientBrush sky = new LinearGradientBrush(
                new Rectangle(0, 0, width, height),
                Color.FromArgb(120, 190, 255),
                Color.FromArgb(200, 235, 255),
                90f))
            {
                g.FillRectangle(sky, 0, 0, width, height);
            }

            // Güneş (sağ üst köşede, sabit)
            using (Brush sun = new SolidBrush(Color.FromArgb(255, 220, 80)))
            {
                g.FillEllipse(sun, width - 110, 30, 60, 60);
            }

            // Kayan bulutlar
            for (int i = 0; i < 3; i++)
            {
                float baseX = (i * width / 3 + cloudOffset) % (width + 120) - 60;
                float baseY = 50 + i * 40;
                DrawCloud(g, baseX, baseY);
            }
        }
        private static void DrawEvening(Graphics g, int width, int height)
        {
            // Turuncu-mor gradyan (üstte mor, altta turuncu)
            using (LinearGradientBrush sky = new LinearGradientBrush(
                new Rectangle(0, 0, width, height),
                Color.FromArgb(90, 60, 130),
                Color.FromArgb(255, 150, 80),
                90f))
            {
                g.FillRectangle(sky, 0, 0, width, height);
            }

            // Batan güneş (ufukta yarım daire)
            using (Brush sun = new SolidBrush(Color.FromArgb(255, 200, 90)))
            {
                g.FillEllipse(sun, width - 130, height - 170, 80, 80);
            }
        }
        private static void DrawNight(Graphics g, int width, int height)
        {
            // Koyu lacivert gradyan
            using (LinearGradientBrush sky = new LinearGradientBrush(
                new Rectangle(0, 0, width, height),
                Color.FromArgb(10, 15, 40),
                Color.FromArgb(40, 50, 100),
                90f))
            {
                g.FillRectangle(sky, 0, 0, width, height);
            }

            // Ay (sağ üst köşede)
            using (Brush moon = new SolidBrush(Color.FromArgb(245, 245, 220)))
            {
                g.FillEllipse(moon, width - 110, 30, 55, 55);
            }

            // Yıldızlar (sabit konumlar, rastgele değil ki her karede yer değiştirmesin)
            Point[] stars = {
        new Point(60, 40), new Point(180, 90), new Point(320, 30),
        new Point(120, 160), new Point(260, 200), new Point(40, 230)
    };
            using (Brush star = new SolidBrush(Color.White))
            {
                foreach (Point p in stars)
                {
                    g.FillEllipse(star, p.X, p.Y, 3, 3);
                }
            }
        }

        private static void DrawCloud(Graphics g, float x, float y)
        {
            using (Brush cloud = new SolidBrush(Color.White))
            {
                g.FillEllipse(cloud, x, y, 60, 30);
                g.FillEllipse(cloud, x + 20, y - 12, 50, 40);
                g.FillEllipse(cloud, x + 45, y, 50, 30);
            }
        }
    }
}
