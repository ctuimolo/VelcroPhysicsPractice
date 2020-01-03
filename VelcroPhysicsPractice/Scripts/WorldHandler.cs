using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

using VelcroPhysics.Dynamics;
using VelcroPhysics.Factories;
using VelcroPhysics.Utilities;

using System.Collections.Generic;

namespace VelcroPhysicsPractice.Scripts
{

    public class WorldHandler
    {
        private readonly World          _world;             // Physics bodies only, no collision flags
        private readonly List<Hitbox>   _worldHitboxes;     // Non-physics hitboxes, collision flags

        public List<Hitbox> WorldHitboxes   { get; private set; }

        public WorldHandler(ContentManager rootContentManager, SpriteBatch rootSpriteBatch, Vector2 gravity)
        {
            _world          = new World(gravity);
            _worldHitboxes  = new List<Hitbox>();
        }

        public Body AddBody(GameObject owner, Vector2 position, Vector2 size, float gravityScale = 1)
        {
            Body body = BodyFactory.CreateRectangle
            (
                _world,
                ConvertUnits.ToSimUnits(size.X),
                ConvertUnits.ToSimUnits(size.Y),
                1f,
                ConvertUnits.ToSimUnits(position + new Vector2(size.X / 2, size.Y /2)),
                0,
                BodyType.Dynamic,
                owner
            );

            body.FixtureList[0].UserData = new Rectangle(0, 0, (int)size.X, (int)size.Y);
            body.FixedRotation = true;
            body.GravityScale = gravityScale;
            body.Friction = 0;

            return body;
        }

        public Body AddKinematicBody(GameObject Owner, Vector2 position, Vector2 size)
        {
            Body body = BodyFactory.CreateRectangle
            (
                _world,
                ConvertUnits.ToSimUnits(size.X),
                ConvertUnits.ToSimUnits(size.Y),
                1f,
                ConvertUnits.ToSimUnits(position + new Vector2(size.X / 2, size.Y / 2)),
                0,
                BodyType.Kinematic,
                Owner
            );

            body.FixtureList[0].UserData = new Rectangle(0, 0, (int)size.X, (int)size.Y);
            body.FixedRotation = true;
            body.Friction = 0;

            return body;
        }

        public Hitbox AddHitbox(Body Owner, Vector2 offset, Vector2 size, string color, CollisionType type, string value)
        {
            Hitbox hitbox = new Hitbox
            (
                Owner,
                offset,
                size,
                color,
                type,
                value
            );
            _worldHitboxes.Add(hitbox);
            return hitbox;
        }

        private bool AABBoverlapping(Hitbox self, Hitbox other)
        {
            // if self.left >= other.right && self.right <= other.left
            if ((self.Position.X - self.Size.X / 2) <= (other.Position.X + other.Size.X / 2) &&
               (self.Position.X + self.Size.X / 2) >= (other.Position.X - other.Size.X / 2) &&
               (self.Position.Y - self.Size.Y / 2) <= (other.Position.Y + other.Size.Y / 2) &&
               (self.Position.Y + self.Size.Y / 2) >= (other.Position.Y - other.Size.Y / 2))
            {
                return true;
            }

            return false;
        }

        public void CheckHitboxCollision(Hitbox self, List<CollisionPackage> collisions)
        {
            foreach (Hitbox other in _worldHitboxes)
            {
                if (!ReferenceEquals(self.Owner, other.Owner))
                {
                    if (other.Enabled == true)
                    {
                        if (AABBoverlapping(self, other))
                        {
                            collisions.Add(other.CollisionPackage);
                        }
                    }
                }
            }
        }

        public void PhysicsStep(GameTime gameTime)
        {
            _world.Step((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f);
            foreach (Hitbox hitbox in _worldHitboxes)
            {
                hitbox.Update();
            }
        }

        public void DrawDebug()
        {
            foreach (Hitbox hitbox in _worldHitboxes)
            {
                hitbox.DrawDebug();
            }
        }
    }
}
