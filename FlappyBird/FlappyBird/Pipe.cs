using System;
using System.Drawing;

namespace FlappyBird
{
    public class Pipe
    {
        private const int CapHeight = 24;    // Sütun başlığının (capital) yüksekliği
        private const int CapOverhang = 6;   // Başlığın gövdeden her yana taşan kısmı

        public float X { get; private set; }
        public int Width { get; }
        public int GapY { get; private set; }   // Boşluğun üst kenarı
        public int GapHeight { get; }           // Boşluğun yüksekliği

        public bool Passed { get; set; } = false; // "Oyuncu bu boruyu geçti mi?" diye kontrol etmek için

        public Pipe(float x, int width, int gapY, int gapHeight)
        {
            X = x;
            Width = width;
            GapY = gapY;
            GapHeight = gapHeight;
        }

        public bool IsOffScreen => X + Width < 0;

        public void Move(float speed)
        {
            X -= speed;
        }

        public void Reset(float newX, int newGapY)
        {
            X = newX;
            GapY = newGapY;
            Passed = false;   // Yeni boru henüz geçilmedi
        }

        public bool CollidesWith(Rectangle bird, int formHeight)
        {
            Rectangle top = new Rectangle((int)X, 0, Width, GapY);
            int bottomY = GapY + GapHeight;
            Rectangle bottom = new Rectangle((int)X, bottomY, Width, formHeight - bottomY);

            return bird.IntersectsWith(top) || bird.IntersectsWith(bottom);
        }

        public void Draw(Graphics g, int formHeight)
        {
            int bottomY = GapY + GapHeight;

            Rectangle topBody = new Rectangle((int)X, 0, Width, GapY - CapHeight);
            Rectangle topCap = new Rectangle((int)X - CapOverhang, GapY - CapHeight, Width + CapOverhang * 2, CapHeight);
            Rectangle bottomCap = new Rectangle((int)X - CapOverhang, bottomY, Width + CapOverhang * 2, CapHeight);
            Rectangle bottomBody = new Rectangle((int)X, bottomY + CapHeight, Width, formHeight - bottomY - CapHeight);

            using (Brush bodyBrush = new SolidBrush(Color.FromArgb(235, 228, 205)))
            using (Brush capBrush = new SolidBrush(Color.FromArgb(250, 245, 230)))
            using (Pen linePen = new Pen(Color.FromArgb(180, 170, 145), 2))
            {
                g.FillRectangle(bodyBrush, topBody);
                g.FillRectangle(bodyBrush, bottomBody);
                g.FillRectangle(capBrush, topCap);
                g.FillRectangle(capBrush, bottomCap);

                // Sütun oluklarını (fluting) dikey çizgilerle çiziyoruz
                for (int fx = (int)X + 12; fx < (int)X + Width; fx += 14)
                {
                    g.DrawLine(linePen, fx, 0, fx, GapY - CapHeight);
                    g.DrawLine(linePen, fx, bottomY + CapHeight, fx, formHeight);
                }

                g.DrawRectangle(linePen, topCap);
                g.DrawRectangle(linePen, bottomCap);
            }
        }
    }
}
