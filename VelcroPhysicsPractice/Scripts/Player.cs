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

using VelcroPhysicsPractice.Scripts;

namespace VelcroPhysicsPractice.Scripts
{
    class Player : GameObject
    {
        // MonoGame Drawing Fields
        private readonly Texture2D playerSprite;
        private SpriteBatch spriteBatch;
        private Rectangle drawRect;

        // BodyCollisionHandler and physics world fields
        BodyCollisionHandler collisionHandler;

        // Player coordinate fields
        private Vector2 origin;
        public Vector2 size = new Vector2(32,32);
        public Vector2 position;

        // Game logic misc. fields
        private KeyboardState _oldKeyState;

        public Player(ContentManager rootContent, World rootWorld, SpriteBatch rootSpriteBatch, Vector2 setPosition)
        {
            // Initialize MonoGame drawing fields
            spriteBatch = rootSpriteBatch;
            playerSprite = rootContent.Load<Texture2D>("green");
            drawRect = new Rectangle(0,0,32,32);

            // Initialize player coordinates
            origin = new Vector2(size.X/2,size.Y/2);
            position = setPosition;

            // Initialize body physics handler
            collisionHandler = new BodyCollisionHandler(rootContent, rootWorld, rootSpriteBatch, setPosition, size);
        }

        public override void Initialize()
        {
        }

        public override void LoadContent()
        {
        }

        private void HandleKeyboard()
        {
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.A))
                collisionHandler.body.LinearVelocity = new Vector2(-10f, collisionHandler.body.LinearVelocity.Y);

            if (state.IsKeyDown(Keys.D))
                collisionHandler.body.LinearVelocity = new Vector2(10f, collisionHandler.body.LinearVelocity.Y);

            if (!state.IsKeyDown(Keys.D) && !state.IsKeyDown(Keys.A))
                collisionHandler.body.LinearVelocity = new Vector2(0, collisionHandler.body.LinearVelocity.Y);

            if (state.IsKeyDown(Keys.Space) && _oldKeyState.IsKeyUp(Keys.Space))
            {
                collisionHandler.body.LinearVelocity = new Vector2(collisionHandler.body.LinearVelocity.X, -20f);
            }
            _oldKeyState = state;
        }

        public override void Update()
        {
            // Update player position from Physics body fields
            position = collisionHandler.getDisplayPosition();
            HandleKeyboard();
        }

        public override void Draw()
        {
            spriteBatch.Draw(
                playerSprite,
                position,
                drawRect,
                Color.White,
                0,
                origin,
                1f,
                SpriteEffects.None,
                0f
            );
        }

        public override void DrawDebug()
        {
            collisionHandler.DebugDraw(spriteBatch);
        }
    }
}
