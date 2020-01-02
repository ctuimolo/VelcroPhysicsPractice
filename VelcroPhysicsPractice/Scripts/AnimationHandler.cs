using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;

namespace VelcroPhysicsPractice.Scripts
{
    public enum PlayerOrientation
    {
        Right,
        Left
    }

    public class AnimationHandler
    {
        private readonly WorldHandler worldHandler;
        private readonly GameObject owner;
        private Animation currentAnimation;

        public PlayerOrientation facing;

        private readonly double frameFactor;
        private readonly double frameRange = 60; // Animation Frames 1/60 FPS
        private int drawIndex       = 0;
        private int animationTimer  = 0;
        private int _state          = 0;

        private readonly SpriteFont font;

        private Dictionary<int, Animation> AnimationDic { get; }
        public SpriteBatch      SpriteBatch     { get; }
        public ContentManager   ContentManager  { get; }

        public AnimationHandler(WorldHandler rootWorldHandler, GameObject rootOwner)
        {
            worldHandler = rootWorldHandler;
            owner = rootOwner;
            ContentManager  = rootWorldHandler.ContentManager;
            SpriteBatch     = rootWorldHandler.SpriteBatch;
            frameFactor     = frameRange / Game.CurrentTargetFPS();
            AnimationDic    = new Dictionary<int, Animation>();
            font = rootWorldHandler.ContentManager.Load<SpriteFont>("font");

            //test = Game.SpriteSheetLoader.Load("Sprites/Suika_idle/suika_idle.png");
        }

        public void AddAnimation(int key, string spriteSheetDirectory, int frameWidth, int frameHeight, Vector2 offset, int frameCount, int setFrameDelay, int startIndex = 0, int loopIndex = 0)
        {
            AnimationDic.Add(key, new Animation(worldHandler, 
                this, 
                spriteSheetDirectory,
                new Rectangle(0,0,frameWidth,frameHeight),
                frameCount,
                setFrameDelay, 
                offset,
                startIndex,
                loopIndex
            ));
        }

        public void ChangeAnimation(int state)
        {
            if(_state != state)
            {
                _state = state;
                drawIndex = AnimationDic[_state].StartIndex;
            }
        }

        public void SetDrawIndex(int setDrawIndex)
        {
            if (setDrawIndex >= currentAnimation.FrameCount)
                drawIndex = 0;
            else
                drawIndex = setDrawIndex;
        }

        public void DrawFrame()
        {
            currentAnimation = AnimationDic[_state];
            SpriteBatch.Draw(
                currentAnimation.SpriteSheet,
                owner.CollisionHandler.getDisplayPosition() - currentAnimation.Offset,
                currentAnimation.GetDrawRect(drawIndex),
                Color.White,
                0,
                Vector2.Zero,
                1f,
                facing == PlayerOrientation.Right ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                0f
            );

            animationTimer++;
            if(animationTimer >= currentAnimation.FrameDelay)
            {
                animationTimer = 0;
                drawIndex++;
                if (drawIndex >= currentAnimation.FrameCount)
                    drawIndex = currentAnimation.LoopIndex;
            }


            SpriteBatch.DrawString(font, "drawIndex  : " + drawIndex, new Vector2(10, 100), Color.Pink);
            currentAnimation.DrawDebug();
        }
    }
}
