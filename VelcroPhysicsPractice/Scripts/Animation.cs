using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VelcroPhysicsPractice.Scripts
{
    class Animation
    {
        private readonly WorldHandler worldHandler;
        private readonly AnimationHandler owner;
        private readonly SpriteBatch spriteBatch;
        private readonly Texture2D spriteSheet;
        private readonly int frameDelay;
        private SpriteFont font;

        private int step;
        private Rectangle drawRect;
        private Vector2 origin;
        private int drawIndex;


        public Animation(WorldHandler rootWorldHandler, AnimationHandler rootAnimationHandler, string spriteSheetDirectory, int setframeDelay, Vector2 setOrigin)
        {
            worldHandler = rootWorldHandler;
            owner = rootAnimationHandler;
            spriteBatch  = owner.SpriteBatch;
            spriteSheet  = owner.ContentManager.Load<Texture2D>(spriteSheetDirectory);
            frameDelay   = setframeDelay;
            step = 1;
            drawRect = new Rectangle(0, 0, 150, 150);
            origin = setOrigin;
            font = rootWorldHandler.ContentManager.Load<SpriteFont>("font");
        }

        public Texture2D GetFrame()
        {
            return spriteSheet;
        }

        public void DrawFrame(Vector2 position, PlayerOrientation facing)
        {
            spriteBatch.Draw(
               spriteSheet,
               position,
               drawRect,
               Color.White,
               0,
               origin,
               1f,
               facing == PlayerOrientation.Right ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
               0f
            );

            drawIndex += 1;
            if (drawIndex >= 18)
                drawIndex = 0;

            drawRect.X = (150 * drawIndex) % 900;
            drawRect.Y = 150 * ((150 * drawIndex) / 900);

            spriteBatch.DrawString(font, "drawRect.X : " + drawRect.X, new Vector2(10, 106), Color.Pink);
            spriteBatch.DrawString(font, "drawRect.Y : " + drawRect.Y, new Vector2(10, 118), Color.Pink);
            spriteBatch.DrawString(font, "drawIndex  : " + drawIndex, new Vector2(10, 130), Color.Pink);
        }
    }
}
