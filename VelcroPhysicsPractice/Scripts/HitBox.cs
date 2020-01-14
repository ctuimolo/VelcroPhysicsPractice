using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace VelcroPhysicsPractice.Scripts
{
    public class CollisionPackage
    {
        public string Value;
        public string String;
    }

    public class Hitbox
    {
        // Monogame drawing fields
        private readonly Texture2D  _sprite;

        public GameObject   Owner   { get; private set; } = null;

        public Vector2 Offset;
        public Vector2 Position;
        public Point   Size;
        public CollisionPackage Data { get; set; }

        public delegate void enact();

        public Hitbox(GameObject owner, Vector2 offset, Point size, string color)
        {
            _sprite = Game.Assets.Load<Texture2D>(color);

            Offset  = offset;
            Size    = size;

            if (owner != null )
            {
                Owner = owner;
                Position = new Vector2(owner.Body.BoxCollider.X + offset.X, owner.Body.BoxCollider.Y + offset.Y);
            } else {
                Position = Offset;
            }

            Data = new CollisionPackage();
        }

        public void DrawDebug()
        {
            Game.SpriteBatch.Draw(
                _sprite,
                new Vector2(Position.X, Position.Y),
                new Rectangle(0, 0, Size.X, Size.Y),
                new Color(Color.White, 0.1f),
                0f,
                Vector2.Zero,
                1f,
                SpriteEffects.None,
                0f
            );
        }
    }
}
