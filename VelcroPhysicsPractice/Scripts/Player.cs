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

        // Debug fields and strings
        private bool afterCollision = false;
        private string afterCollisionString;
        private string positionDebugString;
        public bool isFloored;
        private string isFlooredString;
        private bool isOverlappingOrange;
        private string isOverlappingOrangeString;
        private bool isOverlappingPink;
        private string isOverlappingPinkString;
        private readonly SpriteFont font;

        public Player(WorldHandler worldHandler, ContentManager rootContent, SpriteBatch rootSpriteBatch, Vector2 setPosition)
        {
            // Initialize MonoGame drawing fields
            spriteBatch = rootSpriteBatch;
            playerSprite = rootContent.Load<Texture2D>("white");
            font = rootContent.Load<SpriteFont>("font");
            drawRect = new Rectangle(0,0,32,32);

            // Initialize player coordinates
            origin = new Vector2(size.X/2,size.Y/2);
            position = setPosition;

            // Initialize body physics handler
            collisionHandler = new BodyCollisionHandler(this, worldHandler, rootContent, rootSpriteBatch, setPosition, size);
            Hitbox.enact hitboxCollision = footCollision;
        }

        public static void footCollision()
        {

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

        public override void ResolveCollisions()
        {
            collisionHandler.CheckWorldCollisions();
        }

        public override void Update()
        {
            // World collisions are set, do update here
            isFloored = collisionHandler.isFloored;
            isOverlappingOrange = false;
            isOverlappingPink = false;
            if (collisionHandler.currentCollisions.Count > 0)
            {
                afterCollision = true;
                foreach (CollisionPackage collision in collisionHandler.currentCollisions)
                {
                    if (collision.value == "orange")
                    {
                        isOverlappingOrange = true;
                    }
                    if (collision.value == "purple")
                    {
                        isOverlappingPink = true;
                    }
                }
            } else
            {
                afterCollision = false;
            }

            // Take keyboard input
            HandleKeyboard();
        }

        public override void Draw()
        {
            spriteBatch.Draw(
                playerSprite,
                collisionHandler.getDisplayPosition(),
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
            positionDebugString = "Position: \n" +
                                 "X: " + (int)(collisionHandler.getDisplayPosition().X - size.X / 2) + "\n" +
                                 "Y: " + (int)(collisionHandler.getDisplayPosition().Y - size.Y / 2) + "\n";

            isFlooredString = "                   Grounded:    " + (isFloored ? "true" : "false");
            isOverlappingOrangeString = "   Hitbox Collisions:    " + (isOverlappingOrange ? "true" : "false");
            isOverlappingPinkString = "   Hitbox Collisions:    " + (isOverlappingPink ? "true" : "false");
            afterCollisionString = "Collisions present:    " + (afterCollision ? "true" : "false");

            spriteBatch.DrawString(font, positionDebugString, collisionHandler.getDisplayPosition() + new Vector2(-60, -68), Color.CornflowerBlue);
            spriteBatch.DrawString(font, isFlooredString, collisionHandler.getDisplayPosition() + new Vector2(-60, -82), Color.Gray);
            spriteBatch.DrawString(font, afterCollisionString, collisionHandler.getDisplayPosition() + new Vector2(-60, -96), Color.Gray);
            spriteBatch.DrawString(font, "A " + isOverlappingPinkString, collisionHandler.getDisplayPosition() + new Vector2(-20, -54), Color.Violet);
            spriteBatch.DrawString(font, "B " + isOverlappingOrangeString, collisionHandler.getDisplayPosition() + new Vector2(-20, -40), Color.Orange);
        }
    }
}
