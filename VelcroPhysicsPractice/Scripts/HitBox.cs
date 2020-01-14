using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace VelcroPhysicsPractice.Scripts
{
    public class CollisionPackage
    {
        public string Value;
    }

    public class Hitbox : GameObject
    {
        // Monogame drawing fields
        private readonly Texture2D  _sprite;

        public GameObject   Owner   { get; private set; } = null;

        public Vector2 Offset   { get; set; }
        public Vector2 Position { get; set; }

        public delegate void enact();

        public Hitbox(GameObject owner, Vector2 offset, Vector2 size, string color, string value)
        {
            _sprite = Game.Assets.Load<Texture2D>(color);

            Offset  = offset;

            if (owner != null )
            {
                Owner = owner;
                Position = new Vector2(owner.Body.BoxCollider.X + offset.X, owner.Body.BoxCollider.Y + offset.Y);
            } else {
                Position = Offset;
            }

            Body = Game.World.AddBody(this, Position, size, false);

            Body.BoxCollider.Data = new CollisionPackage()
            {
                Value = color
            };
        }

        public override void Initialize()
        {
        }

        public override void LoadContent()
        {
        } 

        public override void Update()
        {
        }

        public override void ResolveCollisions()
        {
        }

        public override void Draw()
        {
        }

        public override void DrawDebug()
        {
            Game.SpriteBatch.Draw(
                _sprite,
                new Vector2(Body.BoxCollider.X, Body.BoxCollider.Y),
                new Rectangle(0, 0, (int)Body.BoxCollider.Width, (int)Body.BoxCollider.Height),
                new Color(Color.White, 0.25f),
                0f,
                Vector2.Zero,
                1f,
                SpriteEffects.None,
                0f
            );
        }
    }
}
