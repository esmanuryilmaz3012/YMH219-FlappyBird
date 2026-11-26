using System;
using System.Drawing;
using System.Windows.Forms;

namespace FlappyBird
{
    public partial class Form1 : Form
    {
        private const int BirdX = 100;
        private const int BirdSize = 30;
        private const int PipeWidth = 60;
        private const int PipeGap = 150;
        private const int PipeSpacing = 250;
        private const float PipeSpeed = 3f;
        private int score = 0;

        private Bird bird;
        private Pipe[] pipes;
        private System.Windows.Forms.Timer gameTimer;
        private Random random = new Random();
        private bool isGameOver = false;

        public Form1()
        {
            InitializeComponent();

            bird = new Bird(this.ClientSize.Height / 2f);

            pipes = new Pipe[2];
            for (int i = 0; i < pipes.Length; i++)
            {
                pipes[i] = new Pipe(this.ClientSize.Width + i * PipeSpacing, PipeWidth, RandomGapY(), PipeGap);
            }

            gameTimer = new System.Windows.Forms.Timer();
            gameTimer.Interval = 20;
            gameTimer.Tick += GameTimer_Tick;
            gameTimer.Start();
        }

        private int RandomGapY()
        {
            return random.Next(80, this.ClientSize.Height - PipeGap - 80);
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            if (isGameOver) return;

            bird.Update();

            for (int i = 0; i < pipes.Length; i++)
            {
                pipes[i].Move(PipeSpeed);

                if (pipes[i].IsOffScreen)
                {
                    Pipe other = pipes[(i + 1) % pipes.Length];
                    pipes[i].Reset(other.X + PipeSpacing, RandomGapY());
                }

                // Kuşun sol kenarı borunun sağ kenarını geçtiyse ve bu boru henüz sayılmadıysa
                if (!pipes[i].Passed && BirdX > pipes[i].X + pipes[i].Width)
                {
                    pipes[i].Passed = true;
                    score++;
                }
            }

            Rectangle birdRect = new Rectangle(BirdX, (int)bird.Y, BirdSize, BirdSize);
            bool hitPipe = false;
            foreach (Pipe p in pipes)
            {
                if (p.CollidesWith(birdRect, this.ClientSize.Height))
                {
                    hitPipe = true;
                }
            }

            if (bird.Y < 0 || bird.Y + BirdSize >= this.ClientSize.Height || hitPipe)
            {
                isGameOver = true;
                gameTimer.Stop();
            }

            this.Invalidate();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                bird.Jump();
            }
            else if (e.KeyCode == Keys.C)
            {
                bird.NextColor();
                this.Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            foreach (Pipe p in pipes)
            {
                p.Draw(e.Graphics, this.ClientSize.Height);
            }

            using (Brush birdBrush = new SolidBrush(bird.GetDrawColor()))
            {
                e.Graphics.FillEllipse(birdBrush, BirdX, bird.Y, BirdSize, BirdSize);
            }

            if (isGameOver)
            {
                using (Font font = new Font("Arial", 24, FontStyle.Bold))
                {
                    string text = "GAME OVER";
                    SizeF size = e.Graphics.MeasureString(text, font);
                    e.Graphics.DrawString(text, font, Brushes.Red,
                        (this.ClientSize.Width - size.Width) / 2,
                        (this.ClientSize.Height - size.Height) / 2);
                }
            }

            using (Font scoreFont = new Font("Arial", 16, FontStyle.Bold))
            {
                e.Graphics.DrawString("Skor: " + score, scoreFont, Brushes.Black, 10, 10);
            }
        }
    }
}