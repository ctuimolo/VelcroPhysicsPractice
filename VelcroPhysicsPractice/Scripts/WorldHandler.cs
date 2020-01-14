using System;

using Microsoft.Xna.Framework;

using System.Collections.Generic;

using Humper;
using Humper.Responses;

namespace VelcroPhysicsPractice.Scripts
{
    public enum PhysicsType
    {
        Wall,
        Hitbox
    }

    public class WorldHandler
    {
        private readonly World              _world;
        private readonly List<PhysicsBody>  _dynamicBodies;
        private readonly List<Hitbox>       _worldHitboxes;

        public float Gravity        { get; set; } = 0.6f;
        public float MaxFallSpeed   { get; set; } = 12f;

        public WorldHandler()
        {
            _world          = new World(Game.Graphics.PreferredBackBufferWidth, Game.Graphics.PreferredBackBufferHeight);
            _dynamicBodies  = new List<PhysicsBody>();
            _worldHitboxes  = new List<Hitbox>();
        }

        public PhysicsBody AddBody(GameObject owner, Vector2 position, Point size, bool isDynamic = true)
        {
            PhysicsBody newBody = new PhysicsBody(owner, _world.Create(position.X, position.Y, size.X, size.Y));
            if (isDynamic)
            {
                _dynamicBodies.Add(newBody);
            }
            return newBody;
        }

        public Hitbox AddHitbox(GameObject owner, Vector2 offset, Point size, string color)
        {
            Hitbox newHitbox = new Hitbox(owner, offset, size, color);
            _worldHitboxes.Add(newHitbox);
            return newHitbox;
        }

        private bool IsAABBOverlap(Hitbox A, Hitbox B)
        {
            return (A.Position.X <= B.Position.X + B.Size.X && A.Position.X + A.Size.X >= B.Position.X) &&
                   (A.Position.Y <= B.Position.Y + B.Size.Y && A.Position.Y + A.Size.Y >= B.Position.Y);
        }

        public void PhysicsStep()
        {
            foreach (PhysicsBody body in _dynamicBodies)
            {
                body.IsFloored = false;
                body.CurrentCollisions.Clear();

                if (body.GravityEnabled && body.Velocity.Y < MaxFallSpeed)
                {
                    if(body.Velocity.Y + Gravity <= MaxFallSpeed)
                    {
                        body.Velocity.Y += Gravity;
                    } else
                    {
                        body.Velocity.Y = MaxFallSpeed;
                    }
                }

                body.BoxCollider.Move(
                    body.BoxCollider.X + body.Velocity.X,
                    body.BoxCollider.Y + body.Velocity.Y, 
                    (collision) =>
                    {
                        if (body.Velocity.Y > 0 && collision.Hit.Normal.Y < 0)
                        {
                            body.Velocity.Y = 0;
                            body.IsFloored = true;
                        }
                        else if (body.Velocity.Y < 0 && collision.Hit.Normal.Y > 0)
                        {
                            body.Velocity.Y = 0;
                        }
                        if (body.Velocity.X > 0 && collision.Hit.Normal.X < 0)
                        {
                            body.Velocity.X = 0;
                        }
                        else if (body.Velocity.X < 0 && collision.Hit.Normal.X > 0)
                        {
                            body.Velocity.X = 0;
                        }
                        return CollisionResponses.Slide;
                    });

                foreach (Hitbox hitbox in body.ChildHitboxes.Values)
                {
                    hitbox.Position.X = body.BoxCollider.X + hitbox.Offset.X;
                    hitbox.Position.Y = body.BoxCollider.Y + hitbox.Offset.Y;

                    foreach (Hitbox other in _worldHitboxes)
                    {
                        if (!body.CurrentCollisions.Contains(other)     &&
                            !ReferenceEquals(body.Owner, other.Owner)   &&
                            IsAABBOverlap(hitbox, other))
                        {
                            body.CurrentCollisions.Add(other);
                        }
                    }
                }
            }
        }

        public void DrawDebug()
        {
            foreach(Hitbox hitbox in _worldHitboxes)
            {
                hitbox.DrawDebug();
            }
        }
    }
}
