using VelcroPhysics.Dynamics;
using System.Collections.Generic;

namespace VelcroPhysicsPractice.Scripts
{
    public enum CollisionType {
        none,
        wall,
        actor,
    };

    public abstract class GameObject
    {
        public Body body = null;
        public List<HitBox> hitBoxes;
        public CollisionType collisionType = CollisionType.none;
        public abstract void Initialize();
        public abstract void Draw();
        public abstract void LoadContent();
        public abstract void SetCollision(HitBox other);
        public abstract void Update();
    }
}
