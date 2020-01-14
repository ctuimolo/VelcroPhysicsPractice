
using Microsoft.Xna.Framework;

namespace VelcroPhysicsPractice.Scripts
{

    public abstract class GameObject
    {
        public PhysicsBody      Body               { get; protected set; }
        public AnimationHandler AnimationHandler   { get; protected set; }
        public virtual void Initialize()        { }
        public virtual void LoadContent()       { }
        public virtual void Update()            { }
        public virtual void ResolveCollisions() { }
        public virtual void Draw()              { }
        public virtual void DrawDebug()         { }
    }
}
