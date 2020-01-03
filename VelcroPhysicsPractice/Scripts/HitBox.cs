using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using VelcroPhysics.Dynamics;

using VelcroPhysics.Utilities;
using VelcroPhysics.Factories;

using System;
using System.Collections.Generic;

namespace VelcroPhysicsPractice.Scripts
{
    public enum CollisionType
    {
        none,
        wall,
        invoker,
    };

    public class CollisionPackage
    {
        public CollisionType type = CollisionType.none;
        public string Value = "";
    }

    public class Hitbox
    {
        // Monogame drawing fields
        private readonly Texture2D  _sprite;
        private readonly Vector2    _origin;

        public Body     Owner    { get; private set; }
        public Vector2  Offset   { get; set; }
        public Vector2  Size     { get; set; }
        public Vector2  Position { get; set; }

        public CollisionPackage CollisionPackage    { get; set; }
        public bool     Enabled { get; set; } = true;
        public bool     Delete  { get; set; } = false;

        public delegate void enact();

        public Hitbox(Body owner, Vector2 offset, Vector2 size, string color, CollisionType type, string value)
        {
            _origin = new Vector2(size.X / 2, size.Y / 2);

            Size    = size;
            Owner   = owner;
            Offset  = offset;

            if (Owner != null)
            {
                Position = ConvertUnits.ToDisplayUnits(Owner.Position) + Offset;
            }
            else
            {
                Position = Offset;
            }

            _sprite = Game.Assets.Load<Texture2D>(color);

            CollisionPackage = new CollisionPackage 
            {
                Value = value,
                type = type
            };
        }

        public void LoadContent()
        {
        }

        public void Initialize()
        {
        }

        public void Update()
        {
            if(Owner != null) { 
                Position = ConvertUnits.ToDisplayUnits(Owner.Position) + Offset;
            }
        }

        public void Draw()
        {

        }

        public void DrawDebug()
        {
            Game.SpriteBatch.Draw(
                _sprite,
                Position,
                new Rectangle(0, 0, (int)Size.X, (int)Size.Y),
                new Color(255,255,255,120),
                0f,
                _origin,
                1f,
                SpriteEffects.None,
                0f
            );
        }
    }
}
