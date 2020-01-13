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
        private ICollisionResponse          _simulatorBox;

        public float Gravity        { get; set; } = 0.6f;
        public float MaxFallSpeed   { get; set; } = 12f;

        public WorldHandler()
        {
            _world          = new World(Game.Graphics.PreferredBackBufferWidth, Game.Graphics.PreferredBackBufferHeight);
            _dynamicBodies  = new List<PhysicsBody>();
        }

        public PhysicsBody AddBody(GameObject owner, Vector2 position, Vector2 size, bool isDynamic = true)
        {
            PhysicsBody newBody = new PhysicsBody(owner, _world.Create(position.X, position.Y, size.X, size.Y));
            if (isDynamic)
            {
                _dynamicBodies.Add(newBody);
            }
            return newBody;
        }

        public void PhysicsStep()
        {
            foreach (PhysicsBody body in _dynamicBodies)
            {
                //body.ClearCollisions();
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

                body.BoxCollider.Simulate(
                    body.BoxCollider.X + body.Velocity.X,
                    body.BoxCollider.Y + body.Velocity.Y, 
                    (collision) =>
                    {
                        if (collision.Other.HasTag(PhysicsType.Hitbox))
                        {
                            if (collision.Other.Data != null)
                                body.CurrentCollisions.Add(collision);
                            return CollisionResponse.Create(collision, CollisionResponses.Cross);
                        } 
                        if (collision.Other.HasTag(PhysicsType.Wall))
                        {
                            if (body.Velocity.Y > 0 && collision.Hit.Normal.Y < 0)
                            {
                                body.IsFloored = true;
                            }
                            return CollisionResponse.Create(collision, CollisionResponses.Slide);
                        }
                        return CollisionResponse.Create(collision, CollisionResponses.None);
                    });

                body.BoxCollider.Move(
                    body.BoxCollider.X + body.Velocity.X,
                    body.BoxCollider.Y + body.Velocity.Y,
                    (collision) =>
                    {
                        if (collision.Other.HasTag(PhysicsType.Hitbox))
                        {
                            return CollisionResponses.None;
                        }
                        if (collision.Other.HasTag(PhysicsType.Wall))
                        {
                            if (body.Velocity.Y > 0 && collision.Hit.Normal.Y < 0)
                            {
                                body.Velocity.Y = 0;
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
                        }

                        return CollisionResponses.None;
                    });
            }
        }
    }
}
