namespace FlappyBird
{
    public enum BirdColor
    {
        Yellow,
        Red,
        Blue,
        Pink,
        Purple,
        Green,
        Lilac,
        Navy,
        Patricia,
        Cyan,
        Grey,
        Brown,
        White,
        Black,
        Lime,
        Orange
    }
    public class Bird
    {
        public const float Gravity = 0.5f;
        public const float JumpStrength = -8f;

        public float Y { get; private set; }
        public float Velocity { get; private set; }

        public Bird(float startY)
        {
            Y = startY;
            Velocity = 0;
        }

        public void Jump()
        {
            Velocity = JumpStrength;
        }

        public void Update()
        {
            Velocity += Gravity;
            Y += Velocity;
        }

        public BirdColor CurrentColor { get; private set; } = BirdColor.Yellow;

        public void NextColor()
        {
            int count = Enum.GetValues(typeof(BirdColor)).Length;
            CurrentColor = (BirdColor)(((int)CurrentColor + 1) % count);
        }

        public System.Drawing.Color GetDrawColor()
        {
            switch (CurrentColor)
            {
                case BirdColor.Red: return System.Drawing.Color.Red;
                case BirdColor.Blue: return System.Drawing.Color.Blue;
                case BirdColor.Green: return System.Drawing.Color.Green;
                case BirdColor.Purple: return System.Drawing.Color.Purple;
                case BirdColor.Pink: return System.Drawing.Color.Pink;
                case BirdColor.Lilac: return System.Drawing.Color.FromArgb(200, 128, 255);
                case BirdColor.Navy: return System.Drawing.Color.FromArgb(0, 0, 128);
                case BirdColor.Cyan: return System.Drawing.Color.Cyan;
                case BirdColor.Grey: return System.Drawing.Color.FromArgb(128, 128, 128);
                case BirdColor.Brown: return System.Drawing.Color.Brown;
                case BirdColor.White: return System.Drawing.Color.White;
                case BirdColor.Black: return System.Drawing.Color.Black;
                case BirdColor.Lime: return System.Drawing.Color.LimeGreen;
                case BirdColor.Orange: return System.Drawing.Color.Orange;
                case BirdColor.Patricia: return System.Drawing.Color.FromArgb(255, 0, 128, 128);
                default: return System.Drawing.Color.Yellow;
            }
        }
    }
}
