using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VelcroPhysicsPractice.Scripts
{
    public enum OffsetType
    {
        CenterBottom,
        Center
    }

    public class Animation
    {
        private readonly SpriteFont         _font;

        public Texture2D SpriteSheet { get; set; }
        public Rectangle DrawRect;
        
        public Vector2  Offset     { get; set; }
        public Vector2  OwnerSize  { get; set; }
        public int      FrameCount { get; set; }
        public int      FrameDelay { get; set; }
        public int      StartIndex { get; set; }
        public int      LoopIndex  { get; set; }
        public bool     Loop       { get; set; } = true;
        public bool     Play       { get; set; } = true;

        public OffsetType OffsetType = OffsetType.CenterBottom;

        public Animation(Rectangle drawRect, Vector2 ownerSize)
        {
            _font = Game.Assets.Load<SpriteFont>("font");
            DrawRect = drawRect;
            if(OffsetType == OffsetType.CenterBottom)
            {
                Offset = new Vector2(DrawRect.Width / 2 - ownerSize.X / 2 - 1, drawRect.Height - ownerSize.Y - 1);
            }
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
