using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using VelcroPhysics.Dynamics;
using VelcroPhysics.Utilities;

using VelcroPhysicsPractice.Scripts;

namespace VelcroPhysicsPractice
{

    public class Game : Microsoft.Xna.Framework.Game
    {
        // Engine IO and Physics inits
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _batch;
        private readonly World _world;

        // For camera controls
        private Matrix _view;

        // Game Objects to make
        private List<GameObject> _renderedGameObjects;
        private List<HitBox> _worldHitBoxes;

        // Debug

        public Game()
        {
            _graphics = new GraphicsDeviceManager(this) {
                PreferredBackBufferWidth = 800,
                PreferredBackBufferHeight = 480
            };

            Content.RootDirectory = "Content";
            _world = new World(new Vector2(0, 50f));
            
        }

        protected override void LoadContent()
        {
            // Initialize camera controls
            _view = Matrix.Identity;
            _batch = new SpriteBatch(_graphics.GraphicsDevice);
            ConvertUnits.SetDisplayUnitToSimUnitRatio(64f);

            ////////////////////
            //    ADD GAME OBJECTS HERE
            ///////////////////
            _renderedGameObjects = new List<GameObject> {
                new Player(Content, _world, _batch, new Vector2(350,230)),
                new Wall(Content, _world, _batch, new Rectangle(0,420,800,80)),
                new Wall(Content, _world, _batch, new Rectangle(0,0,4,480)),
                new Wall(Content, _world, _batch, new Rectangle(796,0,4,480)),

                new Wall(Content, _world, _batch, new Rectangle(200,340,70,20)),
                new Wall(Content, _world, _batch, new Rectangle(220,320,70,20)),
                new Wall(Content, _world, _batch, new Rectangle(190,400,70,20)),
                new Wall(Content, _world, _batch, new Rectangle(60,325,70,20)),
            };

        }

        protected override void Update(GameTime gameTime)
        {
            HandleKeyboard();

            ///// Physics world step, and then resolve collisions
            /// Send to collisions, interacting objects
            _world.Step((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f);

            foreach (GameObject obj in _renderedGameObjects)
            {
                obj.Update();
            }

            base.Update(gameTime);

        }


        private void HandleKeyboard()
        {
            KeyboardState state = Keyboard.GetState();
         
            if (state.IsKeyDown(Keys.Escape))
                Exit();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _batch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, _view);


            foreach (GameObject obj in _renderedGameObjects)
            {
                obj.Draw();
            }

            _batch.End();

            base.Draw(gameTime);
        }
    }
}