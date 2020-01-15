using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Humper;

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
        private readonly Point      _size = new Point(14,56);
        private readonly float      _walkSpeed    = 3;
        private readonly float      _jumpStrength = 8;

        // Input handler eventually
        private KeyboardState _oldKeyState;
        private bool _inputLeft;
        private bool _inputRight;
        private bool _inputJump;
        private bool _inputAction1;

        // Debug fields and strings
        private bool    _isOverlappingOrange;
        private bool    _isOverlappingPink;
        private string  _afterCollisionString;
        private string  _PositionDebugString;
        private string  _isFlooredString;
        private string  _isOverlappingOrangeString;
        private string  _isOverlappingPinkString;
        private string  _listOfCollisions;

        public Player(WorldHandler setWorld, Vector2 setPosition)
        {
            CurrentWorld            = setWorld;
            Body                    = setWorld.AddBody(this, setPosition, _size);
            Body.ChildHitboxes[0]   = new Hitbox(this, new Vector2(0,0), _size);
            AnimationHandler        = new AnimationHandler(this);
            Position                = setPosition;
            Size                    = _size;

            AnimationHandler.AddAnimation(
                (int)AnimationStates.Idle,
                new Animation(new Rectangle(0, 0, 152, 152), _size)
                {
                    SpriteSheet = Game.Assets.Load<Texture2D>("suika_idle_padded"),
                    FrameCount  = 18,
                    FrameDelay  = 6,
                });

            AnimationHandler.AddAnimation(
                (int)AnimationStates.Walking,
                new Animation(new Rectangle(0, 0, 96, 96), _size)
                {
                    SpriteSheet = Game.Assets.Load<Texture2D>("suika_walk"),
                    FrameCount  = 8,
                    FrameDelay  = 4
                });

            AnimationHandler.AddAnimation(
                (int)AnimationStates.Falling,
                new Animation(new Rectangle(0, 0, 126, 102), _size)
                {
                    SpriteSheet = Game.Assets.Load<Texture2D>("suika_fall"),
                    FrameCount  = 3,
                    FrameDelay  = 6,
                    LoopIndex   = 1
                });

            AnimationHandler.AddAnimation(
               (int)AnimationStates.Rising,
                new Animation(new Rectangle(0, 0, 110, 110), _size)
                {
                    SpriteSheet = Game.Assets.Load<Texture2D>("suika_rise"),
                    Offset      = new Vector2(48, 53),
                    FrameCount  = 2,
                    FrameDelay  = 4,
                    Loop        = false
                });

            AnimationHandler.ChangeAnimation((int)AnimationStates.Idle);
            AnimationHandler.Facing = PlayerOrientation.Right;
        } 

        private void HandleKeyboard()
        {
            KeyboardState state = Keyboard.GetState();

            _inputLeft       = state.IsKeyDown(Keys.A);
            _inputRight      = state.IsKeyDown(Keys.D);
            _inputJump       = state.IsKeyDown(Keys.Space);
            _inputAction1    = state.IsKeyDown(Keys.J);

            if (_inputLeft && !_inputRight )
            {
                Body.Velocity.X = -_walkSpeed;
                AnimationHandler.Facing = PlayerOrientation.Left;
                if(Body.IsFloored)
                {
                    AnimationHandler.ChangeAnimation((int)AnimationStates.Walking);
                }
            }

            if (_inputRight  && !_inputLeft)
            {
                Body.Velocity.X = _walkSpeed;
                AnimationHandler.Facing = PlayerOrientation.Right;
                if (Body.IsFloored)
                {
                    AnimationHandler.ChangeAnimation((int)AnimationStates.Walking);
                }
            }

            if (!_inputRight  && !_inputLeft)
            {
                Body.Velocity.X = 0;
                if (Body.IsFloored)
                {
                    AnimationHandler.ChangeAnimation((int)AnimationStates.Idle);
                }
            }

            if (_inputRight  && _inputLeft)
            {
                Body.Velocity.X = 0;
                if (Body.IsFloored)
                {
                    AnimationHandler.ChangeAnimation((int)AnimationStates.Idle);
                }
            }

            if (state.IsKeyDown(Keys.Space) && _oldKeyState.IsKeyUp(Keys.Space))
            {
                Body.Velocity.Y = -_jumpStrength;
            }

            if (_inputAction1)
            {
                //Game.World.AddHitbox(CollisionHandler.Body, new Vector2(10, 10), new Vector2(50, 50), "purple", CollisionType.invoker, "purple");
            }

            _oldKeyState = state;
        }

        public override void ResolveCollisions()
        {
        }

        public override void Update()
        {
            // Take keyboard input
            HandleKeyboard();

            // World collisions are set, do update here
            _isOverlappingOrange = false;
            _isOverlappingPink   = false;

            if (!Body.IsFloored)
            {
                if (Body.Velocity.Y <= 0)
                {
                    AnimationHandler.ChangeAnimation((int)AnimationStates.Rising);
                }
                else
                {
                    AnimationHandler.ChangeAnimation((int)AnimationStates.Falling);
                }
            }

            foreach (Hitbox collision in Body.CurrentCollisions)
            {
                if (collision.Data.Value == "orange")
                {
                    _isOverlappingOrange = true;
                }
                if (collision.Data.Value == "purple")
                {
                    _isOverlappingPink = true;
                }
            }
        }

        public override void Draw()
        {
            AnimationHandler.DrawFrame();

            _PositionDebugString = "Position: \n" +
                                  "X: " + (int)(Body.BoxCollider.X - _size.X / 2) + "\n" +
                                  "Y: " + (int)(Body.BoxCollider.Y - _size.Y / 2) + "\n";

            _isFlooredString = "Grounded:            " + (Body.IsFloored ? "true" : "false");
            _isOverlappingOrangeString = "Hitbox Collisions:   " + (_isOverlappingOrange ? "true" : "false");
            _isOverlappingPinkString = "Hitbox Collisions:   " + (_isOverlappingPink ? "true" : "false");
            _afterCollisionString = "Collisions present:  " + (Body.CurrentCollisions.Count > 0 ? "true" : "false");

            Game.SpriteBatch.DrawString(
                Debug.Assets.DebugFont,
                "collision packages: " + Body.CurrentCollisions.Count,
                new Vector2(10, 124),
                Color.GreenYellow);
            
            _listOfCollisions = "";
            foreach (Hitbox collision in Body.CurrentCollisions)
            {
                _listOfCollisions += "<" + collision.Position.X + "," + collision.Position.Y + " : " + collision.Data.String + ">\n";
            }

            if (Body.CurrentCollisions.Count > 0 )
            {
                Game.SpriteBatch.DrawString(
                    Debug.Assets.DebugFont,
                    _listOfCollisions,
                    new Vector2(10, 136),
                    Color.GreenYellow);
            }

            Game.SpriteBatch.DrawString(
                Debug.Assets.DebugFont,
                _PositionDebugString,
                new Vector2(Body.BoxCollider.X, Body.BoxCollider.Y) + new Vector2(-60, -88),
                Color.CornflowerBlue);

            Game.SpriteBatch.DrawString(
                Debug.Assets.DebugFont,
                _isFlooredString,
                new Vector2(Body.BoxCollider.X, Body.BoxCollider.Y) + new Vector2(-60, -102),
                Color.Gray);

            Game.SpriteBatch.DrawString(
                Debug.Assets.DebugFont,
                _afterCollisionString,
                new Vector2(Body.BoxCollider.X, Body.BoxCollider.Y) + new Vector2(-60, -116),
                Color.Gray);

            Game.SpriteBatch.DrawString(
                Debug.Assets.DebugFont,
                "A " + _isOverlappingPinkString,
                new Vector2(Body.BoxCollider.X, Body.BoxCollider.Y) + new Vector2(20, -64),
                Color.Violet);

            Game.SpriteBatch.DrawString(
                Debug.Assets.DebugFont,
                "B " + _isOverlappingOrangeString,
                new Vector2(Body.BoxCollider.X, Body.BoxCollider.Y) + new Vector2(20, -50),
                Color.Orange);
        }

        public override void DrawDebug()
        {
            Body.DrawDebug();
        }
    }
}
