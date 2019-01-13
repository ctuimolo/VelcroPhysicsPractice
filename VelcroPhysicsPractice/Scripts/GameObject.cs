using VelcroPhysics.Dynamics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace VelcroPhysicsPractice.Scripts
{
    public enum CollisionType {
        none,
        wall,
        invoker,
    };

    public abstract class GameObject
    {
        public Body body = null;
        public Vector2 size;
        public Vector2 position;
        public List<Hitbox> Hitboxes;
        public CollisionType collisionType = CollisionType.none;
        public abstract void Initialize();
        public abstract void Draw();
        public abstract void LoadContent();
        public abstract void SetCollision(GameObject other);
        public abstract void Update();
    }
}
