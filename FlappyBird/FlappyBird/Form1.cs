using System;
using System.Drawing;
using System.Windows.Forms;

namespace FlappyBird
{
    public enum GameState
    {
        Menu,
        Playing,
        GameOver
    }
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
        private float cloudOffset = 0f;
        private System.Windows.Forms.Timer gameTimer;
        private Random random = new Random();
        private GameState state = GameState.Menu;
        private Button startButton;
        private Button colorButton;

        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.OptimizedDoubleBuffer |
                          ControlStyles.UserPaint, true);

            bird = new Bird(this.ClientSize.Height / 2f);

            pipes = new Pipe[4];
            for (int i = 0; i < pipes.Length; i++)
            {
                pipes[i] = new Pipe(this.ClientSize.Width + i * PipeSpacing, PipeWidth, RandomGapY(), PipeGap);
            }

            gameTimer = new System.Windows.Forms.Timer();
            gameTimer.Interval = 20;
            gameTimer.Tick += GameTimer_Tick;
            CreateMenuButtons();
            gameTimer.Start();
        }
        private float GetRightmostPipeX()
        {
            float rightmost = pipes[0].X;
            for (int i = 1; i < pipes.Length; i++)
            {
                if (pipes[i].X > rightmost)
                {
                    rightmost = pipes[i].X;
                }
            }
            return rightmost;
        }
        private void ResetGame()
            {
            bird.Reset(this.ClientSize.Height / 2f);

            score = 0;

            for (int i = 0; i < pipes.Length; i++)
            {
                float startX = this.ClientSize.Width + i * PipeSpacing;
                pipes[i].Reset(startX, RandomGapY());
            }

            state = GameState.Playing;
            ShowMenuButtons(false);
            gameTimer.Start();
            this.Invalidate();
            }
      
        private int RandomGapY()
        {
            return random.Next(80, this.ClientSize.Height - PipeGap - 80);
        }
        private void CreateMenuButtons()
        {
            startButton = new Button();
            startButton.Text = "BAŞLA";
            startButton.Size = new Size(160, 45);
            startButton.Font = new Font("Arial", 12, FontStyle.Bold);
            startButton.TabStop = false;
            startButton.Click += StartButton_Click;

            colorButton = new Button();
            colorButton.Text = "RENK DEĞİŞTİR";
            colorButton.Size = new Size(160, 45);
            colorButton.Font = new Font("Arial", 10, FontStyle.Bold);
            colorButton.TabStop = false;
            colorButton.Click += ColorButton_Click;

            this.Controls.Add(startButton);
            this.Controls.Add(colorButton);

            LayoutMenuButtons();
        }
        private void StartButton_Click(object sender, EventArgs e)
        {
            ResetGame();
            this.ActiveControl = null;
        }

        private void ColorButton_Click(object sender, EventArgs e)
        {
            bird.NextColor();
            this.Invalidate();
            this.ActiveControl = null;
        }
        private void LayoutMenuButtons()
        {
            int centerX = this.ClientSize.Width / 2;
            int centerY = this.ClientSize.Height / 2;

            startButton.Location = new Point(centerX - startButton.Width / 2, centerY + 70);
            colorButton.Location = new Point(centerX - colorButton.Width / 2, centerY + 125);
        }

        private void ShowMenuButtons(bool show)
        {
            startButton.Visible = show;
            colorButton.Visible = show;
        }
        private void GameTimer_Tick(object sender, EventArgs e)
        {
            cloudOffset += 0.5f;

            if (state != GameState.Playing)
            {
                this.Invalidate();
                return;
            }
            bird.Update();

            for (int i = 0; i < pipes.Length; i++)
            {
                pipes[i].Move(PipeSpeed);

                if (pipes[i].IsOffScreen)
                {
                    pipes[i].Reset(GetRightmostPipeX() + PipeSpacing, RandomGapY());
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
                state = GameState.GameOver;
                gameTimer.Stop();
            }

            this.Invalidate();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (state)
            {
                case GameState.Menu:
                    if (e.KeyCode == Keys.Space)
                    {
                        ResetGame();
                    }
                    else if (e.KeyCode == Keys.C)
                    {
                        bird.NextColor();
                        this.Invalidate();
                    }
                    break;

                case GameState.Playing:
                    if (e.KeyCode == Keys.Space)
                    {
                        bird.Jump();
                    }
                    break;

                case GameState.GameOver:
                    if (e.KeyCode == Keys.Space || e.KeyCode == Keys.R)
                    {
                        ResetGame();
                    }
                    break;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Background.Draw(e.Graphics, this.ClientSize.Width, this.ClientSize.Height, cloudOffset);
            BirdRenderer.Draw(e.Graphics, new Rectangle(BirdX, (int)bird.Y, BirdSize, BirdSize), bird.GetDrawColor());

            foreach (Pipe p in pipes)
            {
                p.Draw(e.Graphics, this.ClientSize.Height);
            }

            using (Font scoreFont = new Font("Arial", 16, FontStyle.Bold))
            {
                e.Graphics.DrawString("Skor: " + score, scoreFont, Brushes.FloralWhite, 10, 10);
            }

            if (state == GameState.GameOver)
            {
                using (Font font = new Font("Arial", 24, FontStyle.Bold))
                using (Font hintFont = new Font("Arial", 12, FontStyle.Regular))
                {
                    string text = "KAYBETTİNİZ";
                    string hint = "Yeniden kaybetmek için Space veya R tuşuna basınız";

                    SizeF size = e.Graphics.MeasureString(text, font);
                    SizeF hintSize = e.Graphics.MeasureString(hint, hintFont);

                    float centerY = this.ClientSize.Height / 2f;

                    e.Graphics.DrawString(text, font, Brushes.Crimson,
                        (this.ClientSize.Width - size.Width) / 2,
                        centerY - size.Height);

                    e.Graphics.DrawString(hint, hintFont, Brushes.FloralWhite,
                        (this.ClientSize.Width - hintSize.Width) / 2,
                        centerY + 5);
                }
            }
            if (state == GameState.Menu)
            {
                using (Font titleFont = new Font("Arial", 35, FontStyle.Bold))
                using (Font textFont = new Font("Arial", 14, FontStyle.Regular))
                {
                    string title = "FLAPPY BIRD";
                    string line1 = "Başlamak için Space tuşuna da basabilirsiniz";

                    SizeF titleSize = e.Graphics.MeasureString(title, titleFont);
                    float centerY = this.ClientSize.Height / 2f;

                    e.Graphics.DrawString(title, titleFont, Brushes.AntiqueWhite,
                        (this.ClientSize.Width - titleSize.Width) / 2,
                        centerY - titleSize.Height - 20);

                    SizeF l1 = e.Graphics.MeasureString(line1, textFont);
                    e.Graphics.DrawString(line1, textFont, Brushes.PaleGoldenrod,
                        (this.ClientSize.Width - l1.Width) / 2, centerY + 10);
                }
            }
        }
    }
}