using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;

using Humper;

namespace VelcroPhysicsPractice.Scripts
{
    public class PhysicsBody
    {
        private readonly Texture2D  _bodySprite;
        private readonly SpriteFont _font;

        public List<ICollision> CurrentCollisions      { get; private set; }
        public bool IsFloored { get; set; }

        public readonly IBox        BoxCollider;
        public readonly GameObject  Owner;

        public bool     GravityEnabled { get; set; } = true;
        public Vector2  Velocity = new Vector2(0, 0);

        public PhysicsBody(GameObject owner, IBox body)
        {
            _bodySprite = Game.Assets.Load<Texture2D>("white");
            _font       = Game.Assets.Load<SpriteFont>("font");

            Owner       = owner;
            BoxCollider = body;

            CurrentCollisions       = new List<ICollision>();
        }

        public void ClearCollisions()
        {
            CurrentCollisions.Clear();
            IsFloored = false;
        }
    }
}
