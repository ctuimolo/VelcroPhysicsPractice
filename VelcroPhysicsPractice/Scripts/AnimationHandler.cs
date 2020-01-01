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
        private GameObject owner;
        private double timer;
        private double frameCount_debug;

        public PlayerOrientation facing;

        private readonly double frameFactor;
        private readonly double frameRange = 60; // Animation Frames 1/60 FPS

        private Dictionary<int, Animation> AnimationDic { get; }

        public SpriteBatch      SpriteBatch     { get; }
        public ContentManager   ContentManager  { get; }
        public int              State           { get; set; }

        public AnimationHandler(WorldHandler rootWorldHandler, GameObject rootOwner)
        {
            worldHandler = rootWorldHandler;
            owner = rootOwner;
            ContentManager  = rootWorldHandler.ContentManager;
            SpriteBatch     = rootWorldHandler.SpriteBatch;
            frameFactor     = frameRange / Game.CurrentTargetFPS();
            AnimationDic    = new Dictionary<int, Animation>();
        }

        public void AddAnimation(int key, string spriteSheetDirectory, Vector2 size, Vector2 origin, int frames, int setFrameDelay)
        {
            AnimationDic.Add(key, new Animation(worldHandler, this, spriteSheetDirectory, setFrameDelay, new Vector2(owner.CollisionHandler.size.X / 2, owner.CollisionHandler.size.Y / 2)));
        }

        public void DrawFrame()
        {
            AnimationDic[State].DrawFrame(owner.CollisionHandler.getDisplayPosition() - new Vector2(60, 94), facing);
        }
    }
}
