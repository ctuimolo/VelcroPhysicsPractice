using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;

namespace VelcroPhysicsPractice.Scripts
{
    class AnimationHandler
    {
        private double timer;
        private double frameCount_debug;
        private readonly double frameFactor;
        private readonly double frameRange = 60; // Animation Frames 1/60 FPS
        private readonly SpriteFont font;

        private Dictionary<int, Animation> AnimationDic { get; }

        public SpriteBatch      SpriteBatch     { get; }
        public ContentManager   ContentManager  { get; }
        public int              State           { get; set; }

        public AnimationHandler(WorldHandler rootWorldHandler)
        {
            ContentManager  = rootWorldHandler.ContentManager;
            SpriteBatch     = rootWorldHandler.SpriteBatch;
            frameFactor     = frameRange / Game.CurrentTargetFPS();
            font            = ContentManager.Load<SpriteFont>("font");
            AnimationDic    = new Dictionary<int, Animation>();
        }

        public void AddAnimation(int key, string spriteSheetDirectory, Vector2 size, Vector2 origin, int frames)
        {
            AnimationDic.Add(key, new Animation(this, spriteSheetDirectory));
        }

        public Texture2D GetFrame(GameTime gameTime)
        {
            timer += frameFactor;
            if (timer >= 1)
            {
                timer = timer % frameFactor;
                frameCount_debug++;
                if (frameCount_debug > frameRange - 1)
                    frameCount_debug = 0;
            }

            SpriteBatch.DrawString(font, "gameTime.ElapsedGameTime (ms)    : " + gameTime.ElapsedGameTime.Milliseconds, new Vector2(10, 70), Color.Pink);
            SpriteBatch.DrawString(font, "gameTime.ElapsedGameTime (ticks) : " + gameTime.ElapsedGameTime.Ticks, new Vector2(10, 82), Color.Pink);
            SpriteBatch.DrawString(font, "Player() : frameCount_debug : " + frameCount_debug, new Vector2(10, 94), Color.Pink);

            return AnimationDic[State].GetFrame();
        }
    }
}
