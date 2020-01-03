using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using VelcroPhysics.Dynamics;
using VelcroPhysics.Factories;
using VelcroPhysics.Utilities;

using Microsoft.Xna.Framework.Input;


namespace VelcroPhysicsPractice.Scripts
{
    public class Wall : GameObject
    {
        private readonly Vector2    _origin;
        private readonly Vector2    _size;
        private readonly Vector2    _position;
        private readonly Texture2D  _sprite;

        private bool _drawDebug = true;
        private KeyboardState _oldKeyState;

        public Body     Body    { get; private set; }
        public Hitbox   Hitbox  { get; private set; }

        public Wall(Rectangle coordinates)
        {
            // Object fields
            _size = new Vector2(coordinates.Width, coordinates.Height);
            _sprite = Game.Assets.Load<Texture2D>("grey");
            _origin = new Vector2(_size.X / 2, _size.Y / 2);
            _position = new Vector2(coordinates.X, coordinates.Y);

            Body = Game.World.AddKinematicBody
            (
                this,
                _position,
                _size
            );

            Hitbox = Game.World.AddHitbox
            (
                Body,
                Vector2.Zero,
                _size,
                "blue",
                CollisionType.wall,
                "wall"
            );

            Hitbox.CollisionPackage.type = CollisionType.wall;
            Hitbox.CollisionPackage.Value = "wall";
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

        public override void Draw(GameTime gameTime)
        {
            Game.SpriteBatch.Draw(
                _sprite,
                ConvertUnits.ToDisplayUnits(Body.Position),
                new Rectangle(0, 0, (int)_size.X, (int)_size.Y), 
                Color.White,
                Body.Rotation,
                _origin,
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