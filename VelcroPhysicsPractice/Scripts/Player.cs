using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace VelcroPhysicsPractice.Scripts
{
    enum AnimationStates
    {
        Idle,
        Walking,
        Rising,
        Falling
    }

    class Player : GameObject
    {
        // Player physics engine params
        private readonly Vector2    _origin;
        private readonly Vector2    _size      = new Vector2(14,56);
        private readonly int        _walkSpeed = 6;
        private readonly SpriteFont _font;

        private Vector2 _velocity;

        // Input handler eventually
        private KeyboardState _oldKeyState;
        private bool _inputLeft;
        private bool _inputRight;
        private bool _inputJump;
        private bool _inputAction1;
        private bool _inputAction2;

        // Debug fields and strings
        private bool    _afterCollision = false;
        private bool    _isOverlappingOrange;
        private bool    _isOverlappingPink;
        private string  _afterCollisionString;
        private string  _PositionDebugString;
        private string  _isFlooredString;
        private string  _isOverlappingOrangeString;
        private string  _isOverlappingPinkString;

        public Vector2 Position     { get; private set; }
        public bool    IsFloored    { get; private set; }

        public Player(Vector2 setPosition)
        {
            _font       = Game.Assets.Load<SpriteFont>("font");
            _origin     = new Vector2(_size.X / 2, _size.Y / 2);
            _velocity   = new Vector2(0,0);

            CollisionHandler    = new BodyCollisionHandler(this, setPosition, _size);
            AnimationHandler    = new AnimationHandler(this);
            Position            = setPosition;

            Hitbox.enact hitboxCollision = footCollision;

            AnimationHandler.AddAnimation(
                (int)AnimationStates.Idle, 
                "suika_idle_padded",
                152, 152,
                new Vector2 (76,152 - _size.Y / 2 - 1), 
                18, 
                6
            );

            AnimationHandler.AddAnimation(
                (int)AnimationStates.Walking,
                "suika_walk",
                96, 96,
                new Vector2(48, 96 - _size.Y / 2 - 1),
                8,
                4
            );

            AnimationHandler.AddAnimation(
                (int)AnimationStates.Falling,
                "suika_fall",
                126, 102,
                new Vector2(63, 102 - _size.Y / 2 - 1),
                3,
                6,
                0, 1
            );

            AnimationHandler.AddAnimation(
                (int)AnimationStates.Rising,
                "suika_rise",
                110, 110,
                new Vector2(110, 55 - _size.Y / 2 - 1),
                2,
                4
            );

            AnimationHandler.ChangeAnimation((int)AnimationStates.Idle);
            AnimationHandler.Facing = PlayerOrientation.Right;
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

            _inputLeft       = state.IsKeyDown(Keys.A);
            _inputRight       = state.IsKeyDown(Keys.D);
            _inputJump       = state.IsKeyDown(Keys.Space);
            _inputAction1    = state.IsKeyDown(Keys.J);

            if (_inputLeft && !_inputRight )
            {
                _velocity.X = -_walkSpeed;
                AnimationHandler.Facing = PlayerOrientation.Left;
                if(CollisionHandler.IsFloored)
                {
                    AnimationHandler.ChangeAnimation((int)AnimationStates.Walking);
                }
            }

            if (_inputRight  && !_inputLeft)
            {
                _velocity.X = _walkSpeed;
                AnimationHandler.Facing = PlayerOrientation.Right;
                if (CollisionHandler.IsFloored)
                {
                    AnimationHandler.ChangeAnimation((int)AnimationStates.Walking);
                }
            }

            if (!_inputRight  && !_inputLeft)
            {
                _velocity.X = 0f;
                if (CollisionHandler.IsFloored)
                {
                    AnimationHandler.ChangeAnimation((int)AnimationStates.Idle);
                }
            }

            if (_inputRight  && _inputLeft)
            {
                _velocity.X = 0f;
                if (CollisionHandler.IsFloored)
                {
                    AnimationHandler.ChangeAnimation((int)AnimationStates.Idle);
                }
            }

            if (state.IsKeyDown(Keys.Space) && _oldKeyState.IsKeyUp(Keys.Space))
            {
                _velocity.Y = -20f;
            }

            if (_inputAction1)
            {
                Game.World.AddHitbox(CollisionHandler.Body, new Vector2(10, 10), new Vector2(50, 50), "purple", CollisionType.invoker, "purple");
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
            IsFloored = CollisionHandler.IsFloored;
            _isOverlappingOrange = false;
            _isOverlappingPink   = false;
            if(!CollisionHandler.IsFloored)
            {
                AnimationHandler.ChangeAnimation((int)AnimationStates.Falling);
            }
            _velocity = CollisionHandler.Body.LinearVelocity;

            if (CollisionHandler.CurrentCollisions.Count > 0)
            {
                _afterCollision = true;
                foreach (CollisionPackage collision in CollisionHandler.CurrentCollisions)
                {
                    if (collision.Value == "orange")
                    {
                        _isOverlappingOrange = true;
                    }
                    if (collision.Value == "purple")
                    {
                        _isOverlappingPink = true;
                    }
                }
            } else
            {
                _afterCollision = false;
            }

            // Take keyboard input
            HandleKeyboard();
            CollisionHandler.Body.LinearVelocity = _velocity;
        }

        public override void Draw(GameTime gameTime)
        {
            AnimationHandler.DrawFrame();
            DrawDebug();
        }

        public override void DrawDebug()
        {
            _PositionDebugString = "Position: \n" +
                                  "X: " + (int)(CollisionHandler.GetDisplayPosition().X - _size.X / 2) + "\n" +
                                  "Y: " + (int)(CollisionHandler.GetDisplayPosition().Y - _size.Y / 2) + "\n";

            _isFlooredString             = "Grounded:            " + (IsFloored           ? "true" : "false");
            _isOverlappingOrangeString   = "Hitbox Collisions:   " + (_isOverlappingOrange ? "true" : "false");
            _isOverlappingPinkString     = "Hitbox Collisions:   " + (_isOverlappingPink   ? "true" : "false");
            _afterCollisionString        = "Collisions present:  " + (_afterCollision      ? "true" : "false");

            Game.SpriteBatch.DrawString(_font, _PositionDebugString,   CollisionHandler.GetDisplayPosition() + new Vector2(-60, -88), Color.CornflowerBlue);
            Game.SpriteBatch.DrawString(_font, _isFlooredString,       CollisionHandler.GetDisplayPosition() + new Vector2(-60, -102), Color.Gray);
            Game.SpriteBatch.DrawString(_font, _afterCollisionString,  CollisionHandler.GetDisplayPosition() + new Vector2(-60, -116), Color.Gray);

            Game.SpriteBatch.DrawString(_font, "A " + _isOverlappingPinkString,   CollisionHandler.GetDisplayPosition() + new Vector2(10, -54), Color.Violet);
            Game.SpriteBatch.DrawString(_font, "B " + _isOverlappingOrangeString, CollisionHandler.GetDisplayPosition() + new Vector2(10, -40), Color.Orange);
        }
    }
}
