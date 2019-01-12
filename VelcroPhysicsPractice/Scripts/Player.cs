using System.Collections.Generic;

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
        private Vector2 playerSize = new Vector2(32,32);
        private Vector2 playerOrigin;
        private readonly float gravityScale = 1;

        // Game logic misc. fields
        private KeyboardState _oldKeyState;

        // Debugs and self hitboxes
        bool drawDebug = true;
        private string positionDebugString;
        private string isTouchingWallString;
        private bool isTouchingWall;
        private List<HitBox> collisions;
        private readonly SpriteFont font;
        public HitBox bodyHitbox;
        public HitBox feetHitbox;

        public Player(ContentManager rootContent, World rootWorld, SpriteBatch rootSpriteBatch, Vector2 setPosition)
        {
            // Player fields
            spriteBatch = rootSpriteBatch;
            playerSprite = rootContent.Load<Texture2D>("white");
            playerOrigin = new Vector2(playerSize.X/2, playerSize.Y/2);
            font = rootContent.Load<SpriteFont>("font");

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

            //////////////////////////////////
            // Debug fields
            feetHitbox = new HitBox(rootWorld, rootSpriteBatch, rootContent, this, new Rectangle(0, (int)playerSize.Y / 2, (int)playerSize.X, 2), "red");
            bodyHitbox = new HitBox(rootWorld, rootSpriteBatch, rootContent, this, new Rectangle(0, 0, (int)playerSize.X, (int)playerSize.Y), "yellow");

            hitBoxes = new List<HitBox> {
                feetHitbox,
                bodyHitbox
            };

            collisions = new List<HitBox>();
        }

        public override void LoadContent()
        {
        }

        public override void Initialize()
        {
        }

        public override void SetCollision(HitBox other)
        {
            collisions.Add(other);
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
            foreach(HitBox hitbox in hitBoxes)
            {
                hitbox.Update();
            }
        }

        public override void Update()
        {


            HandleKeyboard();
            if(body.LinearVelocity.Y > 40)
            {
                body.LinearVelocity = new Vector2(body.LinearVelocity.X, 40);
            }
            isTouchingWall = false;

            ////////////////////////////////
            ///// Setup debug string
            positionDebugString = "Position: \n" +
                                  "X: " + (int)(ConvertUnits.ToDisplayUnits(body.Position.X) - playerSize.X / 2) + "\n" +
                                  "Y: " + (int)(ConvertUnits.ToDisplayUnits(body.Position.Y) - playerSize.Y / 2) + "\n";
             
            isTouchingWallString = "                  Grounded:    " + (isTouchingWall ? " true" : " false");


            UpdateHitboxes();
            collisions.Clear();
        }

        public override void Draw()
        {
            spriteBatch.DrawString(font, positionDebugString, new Vector2(10, 10), Color.CornflowerBlue);
            spriteBatch.DrawString(font, "Toggle hitbox view:   [F1]", new Vector2(60, 10), Color.White);
            spriteBatch.DrawString(font, isTouchingWallString, new Vector2(60, 24), Color.Gray);

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
            ////////////////////
            if (drawDebug)
            {
                foreach (HitBox hitbox in hitBoxes)
                {
                    hitbox.Draw();
                }
            }
        }
    }
}