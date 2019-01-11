using VelcroPhysics.Dynamics;

namespace VelcroPhysicsPractice.Scripts
{
    public enum CollisionType {
        none,
        wall,
        sensor,
    };

    public abstract class GameObject
    {
        public Body body = null;
        public CollisionType collisionType = CollisionType.none;
        public abstract void Initialize();
        public abstract void Draw();
        public abstract void LoadContent();
        public abstract void Collide(GameObject other);
        public abstract void Update();
    }
}
