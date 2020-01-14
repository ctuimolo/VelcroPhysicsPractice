using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;

using Humper;

namespace VelcroPhysicsPractice.Scripts
{
    public class PhysicsBody
    {
        private readonly Texture2D  _sprite;
        private readonly SpriteFont _font;

        public Dictionary<int, Hitbox>  ChildHitboxes       { get; private set; }
        public List<Hitbox>             CurrentCollisions   { get; private set; }
        public bool IsFloored   { get; set; }

        public readonly IBox        BoxCollider;
        public readonly GameObject  Owner;

        public bool     GravityEnabled { get; set; } = true;
        public Vector2  Velocity = new Vector2(0, 0);

        public PhysicsBody(GameObject owner, IBox body)
        {
            _sprite     = Game.Assets.Load<Texture2D>("red");
            _font       = Game.Assets.Load<SpriteFont>("font"); 

            Owner       = owner;
            BoxCollider = body;

            ChildHitboxes       = new Dictionary<int, Hitbox>();
            CurrentCollisions   = new List<Hitbox>();
        }

        public void AddChildHitbox(int key, Vector2 offset, Point size)
        {
            ChildHitboxes[key] = Game.World.AddHitbox(Owner, offset, size, "red");
        }
        
        public void DrawDebug()
        {
            foreach(Hitbox hitbox in ChildHitboxes.Values)
            {
                Game.SpriteBatch.Draw(
                    _sprite,
                    new Vector2(hitbox.Position.X, hitbox.Position.Y),
                    new Rectangle(0, 0, hitbox.Size.X, hitbox.Size.Y),
                    new Color(Color.White, 0.5f),
                    0,
                    Vector2.Zero,
                    1f,
                    SpriteEffects.None,
                    0f);
            }
        }
    }
}
