using System.Collections.Generic;
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using VelcroPhysics.Dynamics;
using VelcroPhysics.Factories;
using VelcroPhysics.Utilities;

namespace VelcroPhysicsPractice.Scripts
{
    public class Player : GameObject
    {
        // Player Monogame drawing fields
        private readonly Texture2D playerSprite;
        private SpriteBatch spriteBatch;

        // Player VelcroPhysics fields
        public Body body;
        private Vector2 playerSize = new Vector2(32,32);
        private Vector2 playerOrigin;
        private readonly float gravityScale = 1;

        // Game logic misc. fields
        private KeyboardState _oldKeyState;

        public Player(ContentManager rootContent, World rootWorld, SpriteBatch rootSpriteBatch, Vector2 setPosition)
        {
            // Player fields
            spriteBatch = rootSpriteBatch;
            playerSprite = rootContent.Load<Texture2D>("white");
            playerOrigin = new Vector2(playerSize.X/2, playerSize.Y/2);

            // VelcroPhysics body configuration
            body = BodyFactory.CreateRectangle(
                rootWorld, 
                ConvertUnits.ToSimUnits(playerSize.X), 
                ConvertUnits.ToSimUnits(playerSize.Y), 
                1f, 
                ConvertUnits.ToSimUnits(setPosition + new Vector2(playerSize.X/2, playerSize.Y/2)), 
                0, 
                BodyType.Dynamic, 
                this
            );
            body.FixedRotation = true;
            body.GravityScale = gravityScale;
            body.FixtureList[0].CollisionCategories = VelcroPhysics.Collision.Filtering.Category.Cat1;
            body.FixtureList[0].CollidesWith = VelcroPhysics.Collision.Filtering.Category.Cat1 & VelcroPhysics.Collision.Filtering.Category.Cat1;
            body.Friction = 0;
        }

        public override void LoadContent()
        {
        }

        public override void Initialize()
        {
        }

        private void HandleKeyboard()
        {
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.A))
                body.LinearVelocity = new Vector2(-4f, body.LinearVelocity.Y);

            if (state.IsKeyDown(Keys.D))
                body.LinearVelocity = new Vector2(4f, body.LinearVelocity.Y);

            if (!state.IsKeyDown(Keys.D) && !state.IsKeyDown(Keys.A))
                body.LinearVelocity = new Vector2(0, body.LinearVelocity.Y);

            if (state.IsKeyDown(Keys.Space) && _oldKeyState.IsKeyUp(Keys.Space))
                body.LinearVelocity = new Vector2(body.LinearVelocity.X, -9f);

            _oldKeyState = state;
        }

        public override void Update()
        {
            HandleKeyboard();
            if(body.LinearVelocity.Y > 10)
            {
                body.LinearVelocity = new Vector2(body.LinearVelocity.X, 10);
            }
        }

        public override void Draw()
        {
            spriteBatch.Draw(
                playerSprite, 
                ConvertUnits.ToDisplayUnits(body.Position), 
                new Rectangle(0, 0, (int)playerSize.X, (int)playerSize.Y), 
                Color.White, 
                body.Rotation,
                playerOrigin, 
                1f, 
                SpriteEffects.None, 
                0f
            );
        }
    }
}