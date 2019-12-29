using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace VelcroPhysicsPractice.Scripts
{
    class Player : GameObject
    {
        // MonoGame Drawing Fields
        private Texture2D   playerSprite;
        private SpriteBatch spriteBatch;
        private Rectangle   drawRect;

        // BodyCollisionHandler and physics world fields
        private WorldHandler         worldHandler;
        private BodyCollisionHandler collisionHandler;

        // Player coordinate fields
        private Vector2 origin;
        public  Vector2 size = new Vector2(32,32);
        public  Vector2 position;
        private Vector2 velocity = new Vector2(0, 0);

        // Game logic misc. fields
        private KeyboardState _oldKeyState;

        // Input handler eventually
        private bool inputLeft;
        private bool inputRight;
        private bool inputJump;
        private bool inputAction1;
        private bool inputAction2;

        // Animation handler things
        private double AnimationTimer   { get; set; }
        private double AnimationStep    { get; } = 60; // in seconds
        private double AnimationDeltaTime   { get; set; }
        private double AnimationTimeStart   { get; set; }

        // Debug fields and strings
        private bool    afterCollision = false;
        private bool    isOverlappingOrange;
        private bool    isOverlappingPink;
        public  bool    isFloored;
        private string  afterCollisionString;
        private string  positionDebugString;
        private string  isFlooredString;
        private string  isOverlappingOrangeString;
        private string  isOverlappingPinkString;
        private SpriteFont font;

        public Player(WorldHandler rootWorldHandler, Vector2 setPosition)
        {
            // Initialize MonoGame drawing fields
            worldHandler    = rootWorldHandler;
            spriteBatch     = rootWorldHandler.SpriteBatch;
            playerSprite    = worldHandler.ContentManager.Load<Texture2D>("white");
            font            = worldHandler.ContentManager.Load<SpriteFont>("font");
            drawRect        = new Rectangle(0,0,32,32);

            // Initialize player coordinates
            origin      = new Vector2(size.X/2,size.Y/2);
            position    = setPosition;

            // Initialize body physics handler
            collisionHandler = new BodyCollisionHandler(this, worldHandler, setPosition, size);
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

            inputLeft       = state.IsKeyDown(Keys.A);
            inputRight      = state.IsKeyDown(Keys.D);
            inputJump       = state.IsKeyDown(Keys.Space);
            inputAction1    = state.IsKeyDown(Keys.J);

            if (inputLeft)
            {
                velocity.X = -10f;
            }

            if (inputRight)
            {
                velocity.X = 10f;
            }

            if (!inputRight && !inputLeft)
            {
                velocity.X = 0f;
            }

            if (inputRight && inputLeft)
            {
                velocity.X = 0f;
            }

            if (state.IsKeyDown(Keys.Space) && _oldKeyState.IsKeyUp(Keys.Space))
            {
                velocity.Y = -20f;
            }

            if (inputAction1)
            {
                worldHandler.AddHitbox(collisionHandler.body, new Vector2(10, 10), new Vector2(50, 50), "purple", CollisionType.invoker, "purple");
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
            isOverlappingPink   = false;
            velocity = collisionHandler.body.LinearVelocity;

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
            collisionHandler.body.LinearVelocity = velocity;
        }

        public override void Draw(GameTime gameTime)
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

            AnimationTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if (AnimationTimer >=  1f / AnimationStep)
            {
                AnimationTimer = 0f;
                if (AnimationDeltaTime > AnimationStep)
                    AnimationDeltaTime = 0;
                else
                    AnimationDeltaTime++;
            }

            spriteBatch.DrawString(font, "Player() : DeltaTime : " + AnimationDeltaTime, new Vector2(10, 70), Color.Pink);
        }

        public override void DrawDebug()
        {
            positionDebugString = "Position: \n" +
                                  "X: " + (int)(collisionHandler.getDisplayPosition().X - size.X / 2) + "\n" +
                                  "Y: " + (int)(collisionHandler.getDisplayPosition().Y - size.Y / 2) + "\n";

            isFlooredString             = "Grounded:            " + (isFloored           ? "true" : "false");
            isOverlappingOrangeString   = "Hitbox Collisions:   " + (isOverlappingOrange ? "true" : "false");
            isOverlappingPinkString     = "Hitbox Collisions:   " + (isOverlappingPink   ? "true" : "false");
            afterCollisionString        = "Collisions present:  " + (afterCollision      ? "true" : "false");

            spriteBatch.DrawString(font, positionDebugString,   collisionHandler.getDisplayPosition() + new Vector2(-60, -68), Color.CornflowerBlue);
            spriteBatch.DrawString(font, isFlooredString,       collisionHandler.getDisplayPosition() + new Vector2(-60, -82), Color.Gray);
            spriteBatch.DrawString(font, afterCollisionString,  collisionHandler.getDisplayPosition() + new Vector2(-60, -96), Color.Gray);

            spriteBatch.DrawString(font, "A " + isOverlappingPinkString,   collisionHandler.getDisplayPosition() + new Vector2(-10, -54), Color.Violet);
            spriteBatch.DrawString(font, "B " + isOverlappingOrangeString, collisionHandler.getDisplayPosition() + new Vector2(-10, -40), Color.Orange);
        }
    }
}
