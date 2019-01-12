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
        private SpriteBatch spriteBatch;

        // Player VelcroPhysics fields
        private Vector2 size;
        private Vector2 origin;
        private Vector2 position;

        // Debugs and self hitboxes
        private bool drawDebug = true;
        public HitBox hitbox;
        private KeyboardState _oldKeyState;

        public Wall(ContentManager rootContent, World rootWorld, SpriteBatch rootSpriteBatch, Rectangle coordinates)
        {
            // Object fields
            spriteBatch = rootSpriteBatch;
            size = new Vector2(coordinates.Width, coordinates.Height);
            sprite = rootContent.Load<Texture2D>("grey");
            origin = new Vector2(size.X / 2, size.Y / 2);
            position = new Vector2(coordinates.X, coordinates.Y);
            collisionType = CollisionType.wall;

            // VelcroPhysics body configuration
            body = BodyFactory.CreateRectangle(
                rootWorld,
                ConvertUnits.ToSimUnits(size.X),
                ConvertUnits.ToSimUnits(size.Y),
                1f,
                ConvertUnits.ToSimUnits(position + new Vector2(size.X/2, size.Y/2)),
                0,
                BodyType.Kinematic,
                this
            );
            body.FixedRotation = true;
            body.FixtureList[0].CollisionCategories = VelcroPhysics.Collision.Filtering.Category.Cat1;
            body.Friction = 0;

            /////////////
            /// Debug hitbox
            hitbox = new HitBox(rootWorld, rootSpriteBatch, rootContent, this, new Rectangle(0, 0, (int)size.X, (int)size.Y), "blue");
        }

        public override void LoadContent()
        {
        }

        public override void Initialize()
        {
        }

        public override void SetCollision(HitBox other)
        {
        }

        public override void Update()
        {
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.F1) && _oldKeyState.IsKeyUp(Keys.F1))
                drawDebug = !drawDebug;
            _oldKeyState = state;
        }

        public override void Draw()
        {
            spriteBatch.Draw(
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

            if(drawDebug)
            {
                //hitbox.Draw();
            }
        }
    }
}