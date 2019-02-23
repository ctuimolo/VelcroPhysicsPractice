﻿using VelcroPhysics.Dynamics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using System.Collections.Generic;
using Microsoft.Xna.Framework;

using VelcroPhysics.Factories;
using VelcroPhysics.Utilities;

namespace VelcroPhysicsPractice.Scripts
{

    public class WorldHandler
    {
        private World _world;
        private List<Hitbox> _worldHitboxes;
        private SpriteBatch _spriteBatch;
        private ContentManager _contentManager;

        public WorldHandler(ContentManager rootContentManager, SpriteBatch rootSpriteBatch, Vector2 gravity)
        {
            _world = new World(gravity);
            _worldHitboxes = new List<Hitbox>();
            _spriteBatch = rootSpriteBatch;
            _contentManager = rootContentManager;
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

        public Body AddKinematicBody(GameObject owner, Vector2 position, Vector2 size)
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
                owner
            );

            body.FixtureList[0].UserData = new Rectangle(0, 0, (int)size.X, (int)size.Y);
            body.FixedRotation = true;
            body.Friction = 0;

            return body;
        }

        public Hitbox AddHitbox(Body owner, Vector2 offset, Vector2 size, string color)
        {
            Hitbox hitbox = new Hitbox
            (
                _contentManager,
                _spriteBatch,
                owner,
                offset,
                size,
                color
            );
            _worldHitboxes.Add(hitbox);
            return hitbox;
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

        public void checkHitboxCollision(Hitbox self)
        {
            foreach (Hitbox other in _worldHitboxes)
            {
                if(!ReferenceEquals(self, other))
                {
                    if(AABBoverlapping(self, other))
                    {
                        // enact collision on self, use delegates?
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
