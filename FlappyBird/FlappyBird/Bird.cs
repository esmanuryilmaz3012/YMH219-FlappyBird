namespace FlappyBird
{
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
    }
}
