using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VelcroPhysicsPractice.Scripts
{
    class Animation
    {
        private readonly WorldHandler worldHandler;
        private readonly AnimationHandler owner;
        private readonly SpriteBatch spriteBatch;
        private readonly SpriteFont font;
        private readonly int drawWidth;
        private readonly int drawHeight;

        private Rectangle drawRect;

        public Texture2D SpriteSheet { get; set; }
        public Rectangle DrawRect    { get; set; }
        public Vector2   Offset      { get; set; }
        public int       FrameCount  { get; set; }
        public int       FrameDelay  { get; set; }
        public int       StartIndex  { get; set; }

        public Animation( WorldHandler rootWorldHandler, 
                          AnimationHandler rootAnimationHandler, 
                          string spriteSheetDirectory, 
                          Rectangle setDrawRect, 
                          int setFrameCount, 
                          int setframeDelay, 
                          Vector2 setOffset,
                          int startIndex = 0)
        {
            worldHandler = rootWorldHandler;
            owner = rootAnimationHandler;
            spriteBatch  = owner.SpriteBatch;
            SpriteSheet  = owner.ContentManager.Load<Texture2D>(spriteSheetDirectory);
            FrameDelay   = setframeDelay;
            drawRect = setDrawRect;
            drawWidth = setDrawRect.Width;
            drawHeight = setDrawRect.Height;
            Offset = setOffset;
            font = rootWorldHandler.ContentManager.Load<SpriteFont>("font");
            FrameCount = setFrameCount;
            StartIndex = startIndex;
        }

        public Texture2D GetFrame()
        {
            return SpriteSheet;
        }

        public Rectangle GetDrawRect(int drawIndex)
        {
            drawRect.X = (drawWidth * drawIndex) % SpriteSheet.Width;
            drawRect.Y = drawHeight * ((drawWidth * drawIndex) / SpriteSheet.Width);
            return drawRect;
        }

        public void DrawDebug()
        {
            spriteBatch.DrawString(font, "drawRect.X : " + drawRect.X, new Vector2(10, 76), Color.Pink);
            spriteBatch.DrawString(font, "drawRect.Y : " + drawRect.Y, new Vector2(10, 88), Color.Pink);
        }
    }
}
