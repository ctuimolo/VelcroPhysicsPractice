using System.Collections.Generic;
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using VelcroPhysicsPractice.Scripts;

namespace VelcroPhysicsPractice
{

    public class Game : Microsoft.Xna.Framework.Game
    {
        // Engine IO and Physics inits
        private static int  _targetFPS = 60;

        // Game camera and view
        private Matrix              _view;
        private bool                _drawDebug = false;
        private KeyboardState       _oldKeyState;
        private int                 _frameCount;
        private double              _frameRate;

        // Graphics and World physics
        public static RoomManager           Rooms       { get; private set; }
        public static GraphicsDeviceManager Graphics    { get; private set; }
        public static SpriteBatch           SpriteBatch { get; private set; }
        public static ContentManager        Assets      { get; private set; }
        public static SpriteFont            DebugFont   { get; private set; }

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
            
            Rooms = new RoomManager();

            Rooms.Hash["test"] = new Room(new Point(800,480));
            Rooms.CurrentRoom = Rooms.Hash["test"];
            Rooms.CurrentRoom.LoadContent();

            DebugFont = Content.Load<SpriteFont>("font");
            _view = Matrix.Identity;
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
            Rooms.CurrentRoom.Update();

            HandleKeyboard();
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

            SpriteBatch.DrawString(DebugFont, "Toggle Hitbox view:   [F1]", new Vector2(10, 10), Color.White);
            SpriteBatch.DrawString(DebugFont, "_frameCount: " + _frameCount, new Vector2(10, 22), Color.White);
            SpriteBatch.DrawString(DebugFont, "Target FPS: " + _targetFPS, new Vector2(10, 34), Color.White);
            SpriteBatch.DrawString(DebugFont, "Avg FPS:    " + _frameRate, new Vector2(10, 46), Color.White);

            Rooms.CurrentRoom.Draw();

            SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}