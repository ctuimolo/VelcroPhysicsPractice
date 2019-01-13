using VelcroPhysics.Dynamics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace VelcroPhysicsPractice.Scripts
{
    public class WorldHandler
    {
        private List<GameObject> _worldObjects;
        private List<Hitbox>     _worldHitboxes;
        private World _world;

        public WorldHandler(Vector2 gravity)
        {
            _world = new World(gravity);
        }

        private bool AABBoverlapping(Hitbox self, Hitbox other)
        {
            // if self.left >= other.right && self.right <= other.left
            if ((self.position.X - self.size.X / 2) <= (other.position.X + other.size.X / 2) &&
               (self.position.X + self.size.X / 2) >= (other.position.X - other.size.X / 2) &&
               (self.position.Y - self.size.Y / 2) <= (other.position.Y + other.size.Y / 2) &&
               (self.position.Y + self.size.Y / 2) >= (other.position.Y - other.size.Y / 2))
            {
                return true;
            }

            return false;
        }

        public void Update()
        {

        }
    }
}
