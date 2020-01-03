using System.Collections.Generic;
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using VelcroPhysics.Dynamics;
using VelcroPhysics.Utilities;

using VelcroPhysicsPractice.Scripts;

namespace VelcroPhysicsPractice
{

    public class Game : Microsoft.Xna.Framework.Game
    {
        // Engine IO and Physics inits
        private static int  _targetFPS = 60;

        // Graphics and World physics
        public static GraphicsDeviceManager Graphics    { get; set; }
        public static SpriteBatch           SpriteBatch { get; set; }
        public static WorldHandler          World       { get; set; }
        public static ContentManager        Assets    { get; set; }

        // Game camera and view
        private Matrix              _view;
        private List<GameObject>    _renderedGameObjects;

        // Debug
        private bool            _drawDebug = false; 
        private SpriteFont      _font;
        private KeyboardState   _oldKeyState;
        private int             _frameCount;  
        private double          _frameRate;

        public Game()
        {
            Graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 800,
                PreferredBackBufferHeight = 480,
                SynchronizeWithVerticalRetrace = true
            };
            TargetElapsedTime = TimeSpan.FromTicks(TimeSpan.TicksPerSecond / _targetFPS);
            Content.RootDirectory   = "Content";
            Assets = Content;
        }

        protected override void LoadContent()
        {
            // Init spritebatch and physics engine
            SpriteBatch = new SpriteBatch(Graphics.GraphicsDevice);
            World = new WorldHandler(Content, SpriteBatch, new Vector2(0, 100f));

            ConvertUnits.SetDisplayUnitToSimUnitRatio(20f);
            _view = Matrix.Identity;
            _font = Content.Load<SpriteFont>("font");

            ////////////////////
            //    ADD GAME OBJECTS HERE
            ///////////////////
            _renderedGameObjects = new List<GameObject> 
            {
                new Wall(World, new Rectangle(0,420,800,80)),
                new Wall(World, new Rectangle(560,327,40,20)),
                new Wall(World, new Rectangle(0,0,4,480)),
                new Wall(World, new Rectangle(0,400,5,480)),
                new Wall(World, new Rectangle(796,0,4,480)),
                new Wall(World, new Rectangle(200,300,70,20)),
                new Wall(World, new Rectangle(220,280,70,20)),
                new Wall(World, new Rectangle(0,0,800,4)),
                new Wall(World, new Rectangle(190,400,70,20)),
                new Wall(World, new Rectangle(60,325,70,20)),
                new Wall(World, new Rectangle(390,398,40,32)),
                new Wall(World, new Rectangle(432,388,40,32)),
                new Wall(World, new Rectangle(474,388,40,32)),

                new Player(World, new Vector2(350,230)),
             };

            World.AddHitbox(null, new Vector2(360, 350), new Vector2( 50,  50), "purple", CollisionType.invoker, "purple");
            World.AddHitbox(null, new Vector2(380, 340), new Vector2( 40,  80), "orange", CollisionType.invoker, "orange");
            World.AddHitbox(null, new Vector2(440, 290), new Vector2( 80,  40), "purple", CollisionType.invoker, "purple");
            World.AddHitbox(null, new Vector2(300, 300), new Vector2( 40,  20), "orange", CollisionType.invoker, "orange");
            World.AddHitbox(null, new Vector2(550, 290), new Vector2( 20, 120), "purple", CollisionType.invoker, "purple");
            World.AddHitbox(null, new Vector2( 60, 290), new Vector2( 40,  40), "orange", CollisionType.invoker, "orange");

            // Initialize debug
            _font = Content.Load<SpriteFont>("font");
        }

        public static int CurrentTargetFPS()
        {
            return _targetFPS;
        }

        private void HandleKeyboard()
        {
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Escape))
                Exit();

            if (state.IsKeyDown(Keys.F1) && _oldKeyState.IsKeyUp(Keys.F1))
                _drawDebug = !_drawDebug;

            _oldKeyState = state;
        }

        protected override void Update(GameTime gameTime) // 60 updates per ~1000ms, non-buffering (slowdown enabled)
        {

            // Physics world step, and then resolve collisions
            // Send to collisions, interacting objects
            World.PhysicsStep(gameTime);

            // update every game object
            foreach (GameObject obj in _renderedGameObjects)
            {
                obj.ResolveCollisions();
            }

            // update every game object
            foreach (GameObject obj in _renderedGameObjects)
            {
                obj.Update();
            }

            HandleKeyboard();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) // calls after Update()
        {

            // FPS and update count debug
            if (_frameCount >= _targetFPS - 1)
                _frameCount = 0;
            else
                _frameCount++;

            _frameRate = Math.Round((1 / gameTime.ElapsedGameTime.TotalSeconds), 1);

            GraphicsDevice.Clear(Color.Black);
            SpriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, _view);

            SpriteBatch.DrawString(_font, "Toggle Hitbox view:   [F1]", new Vector2(10, 10), Color.White);
            SpriteBatch.DrawString(_font, "_frameCount: " + _frameCount, new Vector2(10, 22), Color.White);
            SpriteBatch.DrawString(_font, "Target FPS: " + _targetFPS, new Vector2(10, 34), Color.White);
            SpriteBatch.DrawString(_font, "Avg FPS:    " + _frameRate, new Vector2(10, 46), Color.White);

            foreach (GameObject obj in _renderedGameObjects)
            {
                obj.Draw(gameTime);
                if(_drawDebug)
                {
                    obj.DrawDebug();
                }
            }

            if (_drawDebug)
            {
                World.DrawDebug();
            }

            SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}