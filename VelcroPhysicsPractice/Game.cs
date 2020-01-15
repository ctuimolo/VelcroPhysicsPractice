using System.Collections.Generic;
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using VelcroPhysicsPractice.Scripts;

using Rooms.TestRoom;

namespace VelcroPhysicsPractice
{

    public class Game : Microsoft.Xna.Framework.Game
    {
        // Engine IO and Physics inits
        private static int  _targetFPS = 60;

        // Game camera and view
        private bool                _drawDebug = false;
        private KeyboardState       _oldKeyState;
        private Matrix              _view;
        private int                 _frameCount;
        private double              _frameRate;

        // Graphics and World physics
        public static GraphicsDeviceManager Graphics    { get; private set; }
        public static SpriteBatch           SpriteBatch { get; private set; }
        public static ContentManager        Assets      { get; private set; }
        public static RoomHandler           Rooms       { get; private set; }
        public static Room                  CurrentRoom { get; private set; }

        public Game()
        {
            Graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth        = 800,
                PreferredBackBufferHeight       = 480,
                SynchronizeWithVerticalRetrace  = true
            };
            TargetElapsedTime       = TimeSpan.FromTicks(TimeSpan.TicksPerSecond / _targetFPS);
            Content.RootDirectory   = "Content";
            Assets = Content;
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(Graphics.GraphicsDevice);
            Rooms       = new RoomHandler();

            Rooms.Hash["test room"] = new TestRoom(new Point(1000,1000));
            CurrentRoom = Rooms.Hash["test room"];

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
            HandleKeyboard();
            CurrentRoom.Update();
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

            SpriteBatch.DrawString(Debug.Assets.DebugFont, "Toggle Hitbox view:   [F1]", new Vector2(10, 10), Color.White);
            SpriteBatch.DrawString(Debug.Assets.DebugFont, "_frameCount: " + _frameCount, new Vector2(10, 22), Color.White);
            SpriteBatch.DrawString(Debug.Assets.DebugFont, "Target FPS: " + _targetFPS, new Vector2(10, 34), Color.White);
            SpriteBatch.DrawString(Debug.Assets.DebugFont, "Avg FPS:    " + _frameRate, new Vector2(10, 46), Color.White);

            CurrentRoom.Draw();

            SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}