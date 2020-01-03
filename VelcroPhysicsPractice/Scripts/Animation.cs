using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VelcroPhysicsPractice.Scripts
{
    public class Animation
    {
        private readonly SpriteFont         _font;

        public Texture2D SpriteSheet { get; set; }
        public Rectangle DrawRect;
        public Vector2   Offset      { get; set; }
        public int       FrameCount  { get; set; }
        public int       FrameDelay  { get; set; }
        public int       StartIndex  { get; set; }
        public int       LoopIndex   { get; set; }
        public bool      Loop        { get; set; } = true;
        public bool      Play        { get; set; } = true;

        public Animation()
        {
            _font       = Game.Assets.Load<SpriteFont>("font");
        }

        public Rectangle GetDrawRect(int drawIndex)
        {
            DrawRect.X = (DrawRect.Width * drawIndex) % SpriteSheet.Width;
            DrawRect.Y = DrawRect.Height * ((DrawRect.Width * drawIndex) / SpriteSheet.Width);
            return DrawRect;
        }

        public void DrawDebug()
        {
            Game.SpriteBatch.DrawString(_font, "DrawRect.X : " + DrawRect.X, new Vector2(10, 76), Color.Pink);
            Game.SpriteBatch.DrawString(_font, "DrawRect.Y : " + DrawRect.Y, new Vector2(10, 88), Color.Pink);
        }
    }
}
