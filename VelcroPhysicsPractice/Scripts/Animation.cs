using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VelcroPhysicsPractice.Scripts
{
    class Animation
    {
        private readonly AnimationHandler owner;
        private readonly SpriteBatch spriteBatch;
        private readonly Texture2D spriteSheet;

        public Animation(AnimationHandler rootAnimationHandler, string spriteSheetDirectory)
        {
            owner = rootAnimationHandler;
            spriteBatch  = owner.SpriteBatch;
            spriteSheet  = owner.ContentManager.Load<Texture2D>(spriteSheetDirectory);
        }

        public Texture2D GetFrame()
        {
            return spriteSheet;
        }
    }
}
