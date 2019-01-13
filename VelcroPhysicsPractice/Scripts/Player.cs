using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using VelcroPhysics.Dynamics;
using VelcroPhysics.Factories;
using VelcroPhysics.Utilities;

using System;

namespace VelcroPhysicsPractice.Scripts
{
    public class Player : GameObject
    {
        // Player Monogame drawing fields
        private readonly Texture2D playerSprite;
        private SpriteBatch spriteBatch;

        // Player VelcroPhysics fields
        private Vector2 playerOrigin;
        private readonly float gravityScale = 1;

        // Game logic misc. fields
        private KeyboardState _oldKeyState;

        // Debugs and self Hitboxes
        bool drawDebug = true;
        private string positionDebugString;
        private string isTouchingWallString;
        private bool isTouchingWall;
        private List<Hitbox> collisions;
        private readonly SpriteFont font;
        public Hitbox bodyHitbox;
        public Hitbox feetHitbox;

        public Player(ContentManager rootContent, World rootWorld, List<Hitbox> rootWorldHitboxes, SpriteBatch rootSpriteBatch, Vector2 setPosition)
        {
            // Player fields
            spriteBatch = rootSpriteBatch;
            playerSprite = rootContent.Load<Texture2D>("white");
            size = new Vector2(32, 32);
            playerOrigin = new Vector2(size.X/2, size.Y/2);
            font = rootContent.Load<SpriteFont>("font");

            // VelcroPhysics body configuration
            body = BodyFactory.CreateRectangle(
                rootWorld, 
                ConvertUnits.ToSimUnits(size.X), 
                ConvertUnits.ToSimUnits(size.Y), 
                1f, 
                ConvertUnits.ToSimUnits(setPosition + new Vector2(size.X/2, size.Y/2)), 
                0, 
                BodyType.Dynamic, 
                this
            );
            body.FixedRotation = true;
            body.GravityScale = gravityScale;
            /*body.FixtureList[0].CollisionCategories = VelcroPhysics.Dynamics.Contacts;
            body.FixtureList[0].CollisionCategories = VelcroPhysics.Collision.Filtering.Category.Cat1;
            body.FixtureList[0].CollidesWith = VelcroPhysics.Collision.Filtering.Category.Cat1 & VelcroPhysics.Collision.Filtering.Category.Cat1;*/
            body.Friction = 0;

            //////////////////////////////////
            // Debug fields
            feetHitbox = new Hitbox(rootWorld, rootSpriteBatch, rootContent, this, new Rectangle(0, (int)size.Y / 2, (int)size.X, 2), "red", CollisionType.wall);
            bodyHitbox = new Hitbox(rootWorld, rootSpriteBatch, rootContent, this, new Rectangle(0, 0, (int)size.X, (int)size.Y), "yellow");
            rootWorldHitboxes.Add(feetHitbox);
            rootWorldHitboxes.Add(bodyHitbox);

            Hitboxes = new List<Hitbox> {
                feetHitbox,
                bodyHitbox
            };

            collisions = new List<Hitbox>();
        }

        public override void LoadContent()
        {
        }

        public override void Initialize()
        {
        }

        public override void SetCollision(GameObject other)
        {
        }

        private void HandleKeyboard()
        {
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.A))
                body.LinearVelocity = new Vector2(-10f, body.LinearVelocity.Y);

            if (state.IsKeyDown(Keys.D))
                body.LinearVelocity = new Vector2(10f, body.LinearVelocity.Y);

            if (!state.IsKeyDown(Keys.D) && !state.IsKeyDown(Keys.A))
                body.LinearVelocity = new Vector2(0, body.LinearVelocity.Y);

            if (state.IsKeyDown(Keys.Space) && _oldKeyState.IsKeyUp(Keys.Space))
                body.LinearVelocity = new Vector2(body.LinearVelocity.X, -20f);

            if (state.IsKeyDown(Keys.F1) && _oldKeyState.IsKeyUp(Keys.F1))
                drawDebug = !drawDebug;

            _oldKeyState = state;
        }

        private void UpdateHitboxes()
        {
            foreach(Hitbox Hitbox in Hitboxes)
            {
                Hitbox.UpdatePosition();
            }
        }

        public override void Update()
        {
            position = ConvertUnits.ToDisplayUnits(body.Position);

            HandleKeyboard();
            if(body.LinearVelocity.Y > 40)
            {
                body.LinearVelocity = new Vector2(body.LinearVelocity.X, 40);
            }
            isTouchingWall = false;

            ////////////////////////////////
            ///// Setup debug string
            positionDebugString = "Position: \n" +
                                  "X: " + (int)(ConvertUnits.ToDisplayUnits(body.Position.X) - size.X / 2) + "\n" +
                                  "Y: " + (int)(ConvertUnits.ToDisplayUnits(body.Position.Y) - size.Y / 2) + "\n";
             
            isTouchingWallString = "                  Grounded:    " + (isTouchingWall ? " true" : " false");


            UpdateHitboxes();
            collisions.Clear();
        }

        public override void Draw()
        {
            spriteBatch.DrawString(font, positionDebugString, new Vector2(10, 10), Color.CornflowerBlue);
            spriteBatch.DrawString(font, "Toggle Hitbox view:   [F1]", new Vector2(60, 10), Color.White);
            spriteBatch.DrawString(font, isTouchingWallString, new Vector2(60, 24), Color.Gray);

            spriteBatch.Draw(
                playerSprite,
                ConvertUnits.ToDisplayUnits(body.Position),
                new Rectangle(0, 0, (int)size.X, (int)size.Y), 
                Color.White, 
                body.Rotation,
                playerOrigin, 
                1f, 
                SpriteEffects.None, 
                0f
            );
            /*////////////////////
            if (drawDebug)
            {
                foreach (Hitbox Hitbox in Hitboxes)
                {
                    Hitbox.Draw();
                }
            }*/
        }
    }
}