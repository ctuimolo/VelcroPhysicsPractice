using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace VelcroPhysicsPractice.Scripts
{
    enum AnimationStates
    {
        Idle,
        Walking
    }

    class Player : GameObject
    {
        // MonoGame Drawing Fields
        private SpriteBatch spriteBatch;

        // BodyCollisionHandler and physics world fields
        private WorldHandler         worldHandler;

        // Player coordinate fields
        private Vector2 origin;
        public  Vector2 size = new Vector2(28,56);
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
        private readonly SpriteFont font;

        public Player(WorldHandler rootWorldHandler, Vector2 setPosition)
        {
            // Initialize MonoGame drawing fields
            worldHandler = rootWorldHandler;
            spriteBatch = rootWorldHandler.SpriteBatch;
            font = rootWorldHandler.ContentManager.Load<SpriteFont>("font");

            // Initialize player coordinates
            origin = new Vector2(size.X / 2, size.Y / 2);
            position = setPosition;

            // Initialize body physics handler
            CollisionHandler = new BodyCollisionHandler(this, worldHandler, setPosition, size);
            Hitbox.enact hitboxCollision = footCollision;

            // Initialize Animation handler fields
            AnimationHandler = new AnimationHandler(worldHandler, this);
            AnimationHandler.AddAnimation((int)AnimationStates.Idle, "suika_idle", new Vector2 (150,150), new Vector2 (60,150), 18, 2);
            AnimationHandler.State = (int)AnimationStates.Idle;
            AnimationHandler.facing = PlayerOrientation.Right;
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

            if (inputLeft && !inputRight)
            {
                velocity.X = -10f;
                AnimationHandler.facing = PlayerOrientation.Left;
            }

            if (inputRight && !inputLeft)
            {
                velocity.X = 10f;
                AnimationHandler.facing = PlayerOrientation.Right;
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
                worldHandler.AddHitbox(CollisionHandler.body, new Vector2(10, 10), new Vector2(50, 50), "purple", CollisionType.invoker, "purple");
            }

            _oldKeyState = state;
        }

        public override void ResolveCollisions()
        {
            CollisionHandler.CheckWorldCollisions();
        }

        public override void Update()
        {
            // World collisions are set, do update here
            isFloored = CollisionHandler.isFloored;
            isOverlappingOrange = false;
            isOverlappingPink   = false;
            velocity = CollisionHandler.body.LinearVelocity;

            if (CollisionHandler.currentCollisions.Count > 0)
            {
                afterCollision = true;
                foreach (CollisionPackage collision in CollisionHandler.currentCollisions)
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
            CollisionHandler.body.LinearVelocity = velocity;
        }

        public override void Draw(GameTime gameTime)
        {
            AnimationHandler.DrawFrame();
        }

        public override void DrawDebug()
        {
            positionDebugString = "Position: \n" +
                                  "X: " + (int)(CollisionHandler.getDisplayPosition().X - size.X / 2) + "\n" +
                                  "Y: " + (int)(CollisionHandler.getDisplayPosition().Y - size.Y / 2) + "\n";

            isFlooredString             = "Grounded:            " + (isFloored           ? "true" : "false");
            isOverlappingOrangeString   = "Hitbox Collisions:   " + (isOverlappingOrange ? "true" : "false");
            isOverlappingPinkString     = "Hitbox Collisions:   " + (isOverlappingPink   ? "true" : "false");
            afterCollisionString        = "Collisions present:  " + (afterCollision      ? "true" : "false");

            spriteBatch.DrawString(font, positionDebugString,   CollisionHandler.getDisplayPosition() + new Vector2(-60, -68), Color.CornflowerBlue);
            spriteBatch.DrawString(font, isFlooredString,       CollisionHandler.getDisplayPosition() + new Vector2(-60, -82), Color.Gray);
            spriteBatch.DrawString(font, afterCollisionString,  CollisionHandler.getDisplayPosition() + new Vector2(-60, -96), Color.Gray);

            spriteBatch.DrawString(font, "A " + isOverlappingPinkString,   CollisionHandler.getDisplayPosition() + new Vector2(-10, -54), Color.Violet);
            spriteBatch.DrawString(font, "B " + isOverlappingOrangeString, CollisionHandler.getDisplayPosition() + new Vector2(-10, -40), Color.Orange);
        }
    }
}
