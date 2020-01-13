
using Microsoft.Xna.Framework;

namespace VelcroPhysicsPractice.Scripts
{

    public abstract class GameObject
    {
        public PhysicsBody      Body               { get; protected set; }
        public AnimationHandler AnimationHandler   { get; protected set; }
        public abstract void Initialize();
        public abstract void LoadContent();
        public abstract void Update();
        public abstract void ResolveCollisions();
        public abstract void Draw();
        public abstract void DrawDebug();
    }
}
