using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VelcroPhysicsPractice.Scripts
{
    class Animation
    {
        private readonly AnimationHandler   _owner;
        private readonly SpriteFont         _font;
        private readonly int                _drawWidth;
        private readonly int                _drawHeight;

        private Rectangle _drawRect;

        public Texture2D SpriteSheet { get; private set; }
        public Vector2   Offset      { get; set; }
        public int       FrameCount  { get; set; }
        public int       FrameDelay  { get; set; }
        public int       StartIndex  { get; set; }
        public int       LoopIndex   { get; set; }

        public Animation( AnimationHandler  owner, 
                          string    spriteSheetDirectory, 
                          Rectangle drawRect, 
                          int       setFrameCount, 
                          int       setframeDelay, 
                          Vector2   setOffset,
                          int       startIndex = 0,
                          int       loopIndex  = 0 )
        {
            _owner        = owner;
            SpriteSheet  = Game.Assets.Load<Texture2D>(spriteSheetDirectory);
            _drawRect     = new Rectangle(drawRect.X, drawRect.Y, drawRect.Width, drawRect.Height);
            _drawWidth    = drawRect.Width;
            _drawHeight   = drawRect.Height;
            _font         = Game.Assets.Load<SpriteFont>("font");

            FrameDelay  = setframeDelay;
            Offset      = setOffset;
            FrameCount  = setFrameCount;
            StartIndex  = startIndex;
            LoopIndex   = loopIndex;
        }

        public Rectangle GetDrawRect(int drawIndex)
        {
            _drawRect.X = (_drawWidth * drawIndex) % SpriteSheet.Width;
            _drawRect.Y = _drawHeight * ((_drawWidth * drawIndex) / SpriteSheet.Width);
            return _drawRect;
        }

        public void DrawDebug()
        {
            Game.SpriteBatch.DrawString(_font, "_drawRect.X : " + _drawRect.X, new Vector2(10, 76), Color.Pink);
            Game.SpriteBatch.DrawString(_font, "_drawRect.Y : " + _drawRect.Y, new Vector2(10, 88), Color.Pink);
        }
    }
}
