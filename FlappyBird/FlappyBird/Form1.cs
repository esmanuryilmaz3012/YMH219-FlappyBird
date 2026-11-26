using System;
using System.Drawing;
using System.Windows.Forms;

namespace FlappyBird
{
    public partial class Form1 : Form
    {
        private Bird bird;
        private System.Windows.Forms.Timer gameTimer;
        private bool isGameOver = false;
        private const int BirdSize = 30;

        public Form1()
        {
            InitializeComponent();

            bird = new Bird(this.ClientSize.Height / 2f);

            gameTimer = new System.Windows.Forms.Timer();
            gameTimer.Interval = 20;
            gameTimer.Tick += GameTimer_Tick;
            gameTimer.Start();
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            if (isGameOver) return;

            bird.Update();

            if (bird.Y < 0 || bird.Y + BirdSize >= this.ClientSize.Height)
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
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.FillEllipse(Brushes.Yellow, 100, bird.Y, 30, 30);
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
        }


    }
}