using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Microsoft.Xna.Framework.Input;

namespace VelcroPhysicsPractice.Scripts
{
    public class Wall : GameObject
    {
        private readonly Point      _size;
        private readonly Vector2    _position;
        private readonly Texture2D  _sprite;

        private bool _drawDebug = true;
        private KeyboardState _oldKeyState;

        public Hitbox Hitbox  { get; private set; }

        public Wall(Rectangle coordinates)
        {
            // Object fields
            _size     = new Point(coordinates.Width, coordinates.Height);
            _sprite   = Game.Assets.Load<Texture2D>("grey");
            _position = new Vector2(coordinates.X, coordinates.Y);

            Body = Game.World.AddBody(this, _position, _size, false);
        }

        public override void LoadContent()
        {
        }

        public override void Initialize()
        {
        }

        public override void ResolveCollisions()
        {
        }

        public override void Update()
        {
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.F1) && _oldKeyState.IsKeyUp(Keys.F1))
                _drawDebug = !_drawDebug;
            _oldKeyState = state;
        }

        public override void Draw()
        {
            Game.SpriteBatch.Draw(
                _sprite,
                new Vector2(Body.BoxCollider.X, Body.BoxCollider.Y),
                new Rectangle(0, 0, (int)_size.X, (int)_size.Y), 
                Color.White,
                0,
                Vector2.Zero,
                1f,
                SpriteEffects.None,
                0f
            );
        }

        public override void DrawDebug()
        {
        }
    }
}