using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using VelcroPhysics.Dynamics;
using VelcroPhysics.Collision.ContactSystem;
using VelcroPhysics.Dynamics.Solver;
using VelcroPhysics.Factories;
using VelcroPhysics.Utilities;

using System;

namespace VelcroPhysicsPractice.Scripts
{
    public class BodyCollisionHandler
    {
        // _bodyCollisionHandler Monogame drawing fields
        private readonly Texture2D  _bodyCollisionHandlerSprite;
        private readonly Vector2    _bodyCollisionHandlerOrigin;
        private readonly SpriteFont _font;

        // hold all collisions 
        private readonly Vector2 _size;

        private Hitbox  _bodyHitbox;
        private Hitbox  _feetHitbox;

        public List<CollisionPackage> CurrentCollisions         { get; private set; }
        public List<CollisionPackage> CurrentFloorCollisions    { get; private set; }
        public Body    Body         { get; private set; }
        public bool    IsFloored    { get; private set; }

        public BodyCollisionHandler(GameObject owner, Vector2 startPosition, Vector2 size)
        {
            _bodyCollisionHandlerSprite = Game.Assets.Load<Texture2D>("white");
            _size                       = size;
            _bodyCollisionHandlerOrigin = new Vector2(_size.X/2, _size.Y/2);
            _font = Game.Assets.Load<SpriteFont>("font");

            CurrentCollisions = new List<CollisionPackage>();
            CurrentFloorCollisions = new List<CollisionPackage>();

            Body = Game.World.AddBody(
                owner,
                startPosition,
                size
            );

            _bodyHitbox = AddHitbox(
                Body,
                Vector2.Zero,
                size,
                "red",
                CollisionType.invoker,
                "Player _body"
            );

            _feetHitbox = AddHitbox(
                Body,
                new Vector2(0, _size.Y / 2),
                new Vector2(_size.X, 2),
                "red",
                CollisionType.invoker,
                "Player feet"
            );
        }

        public Hitbox AddHitbox(Body owner, Vector2 offset, Vector2 size, string color, CollisionType type, string value)
        {
            return Game.World.AddHitbox(
                Body,
                offset,
                size,
                color,
                type,
                value
            );
        }

        public void ClearCollisions()
        {
            CurrentCollisions.Clear();
            CurrentFloorCollisions.Clear();
        }

        public void CheckWorldCollisions()
        {
            ClearCollisions();
            Game.World.CheckHitboxCollision(_bodyHitbox, CurrentCollisions);
            Game.World.CheckHitboxCollision(_feetHitbox, CurrentFloorCollisions);
            CheckFloored();
        }

        public void CheckFloored()
        {
            if (CurrentFloorCollisions.Count > 0)
            {
                foreach (CollisionPackage collision in CurrentFloorCollisions)
                {
                    if(collision.type == CollisionType.wall)
                    {
                        IsFloored = true;
                    }
                }
            } else
            {
                IsFloored = false;
            }
        }

        public Vector2 GetDisplayPosition()
        {
            return ConvertUnits.ToDisplayUnits(Body.Position);
        }

        public void DebugDraw(SpriteBatch spriteBatch)
        {
        }
    }
}