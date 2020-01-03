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
        // Player Monogame drawing fields
        private readonly Texture2D sprite;

        // Player VelcroPhysics fields
        private Vector2 origin;
        private Vector2 size;
        private Vector2 position;
        private Body body;

        // Debugs and self Hitboxes
        private bool drawDebug = true;
        public Hitbox hitbox;
        private KeyboardState _oldKeyState;

        public Wall(Rectangle coordinates)
        {
            // Object fields
            size = new Vector2(coordinates.Width, coordinates.Height);
            sprite = Game.Assets.Load<Texture2D>("grey");
            origin = new Vector2(size.X / 2, size.Y / 2);
            position = new Vector2(coordinates.X, coordinates.Y);

            body = Game.World.AddKinematicBody
            (
                this,
                position,
                size
            );

            hitbox = Game.World.AddHitbox
            (
                body,
                Vector2.Zero,
                size,
                "blue",
                CollisionType.wall,
                "wall"
            );

            hitbox.CollisionPackage.type = CollisionType.wall;
            hitbox.CollisionPackage.Value = "wall";
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
                drawDebug = !drawDebug;
            _oldKeyState = state;
        }

        public override void Draw(GameTime gameTime)
        {
            Game.SpriteBatch.Draw(
                sprite,
                ConvertUnits.ToDisplayUnits(body.Position),
                new Rectangle(0, 0, (int)size.X, (int)size.Y), 
                Color.White,
                body.Rotation,
                origin,
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