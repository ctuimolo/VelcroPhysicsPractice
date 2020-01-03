using VelcroPhysics.Dynamics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace VelcroPhysicsPractice.Scripts
{

    public abstract class GameObject
    {
        public BodyCollisionHandler  CollisionHandler { get; protected set; }
        public AnimationHandler      AnimationHandler { get; protected set; }
        public abstract void Initialize();
        public abstract void Draw(GameTime gameTime);
        public abstract void LoadContent();
        public abstract void Update();
        public abstract void ResolveCollisions();
        public abstract void DrawDebug();
    }
}
