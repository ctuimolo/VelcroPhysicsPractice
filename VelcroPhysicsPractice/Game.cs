using System.Collections.Generic;
using System;

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
        private SpriteFont _font;
        //private readonly World _world;
        private  WorldHandler worldHandler;

        // For camera controls
        private Matrix _view;

        // Game Objects to make
        private List<GameObject> _renderedGameObjects;
        private List<Hitbox> _worldHitboxes;

        // Debug
        private bool drawDebug = false;
        private SpriteFont font;
        private KeyboardState _oldKeyState;

        public Game()
        {
            _graphics = new GraphicsDeviceManager(this) {
                PreferredBackBufferWidth = 800,
                PreferredBackBufferHeight = 480
            };

            Content.RootDirectory = "Content";
            //_world = new World(new Vector2(0, 100f));
            _worldHitboxes = new List<Hitbox>();

        }

        bool AABBoverlapping(Hitbox self, Hitbox other)
        {
            // if self.left >= other.right && self.right <= other.left
            if((self.position.X - self.size.X/2) <= (other.position.X + other.size.X/2) &&
               (self.position.X + self.size.X/2) >= (other.position.X - other.size.X/2) &&
               (self.position.Y - self.size.Y / 2) <= (other.position.Y + other.size.Y / 2) &&
               (self.position.Y + self.size.Y / 2) >= (other.position.Y - other.size.Y / 2))
            {
                return true;
            }

            return false;
        }

        protected override void LoadContent()
        {
            _batch = new SpriteBatch(_graphics.GraphicsDevice);
            _view = Matrix.Identity;
            ConvertUnits.SetDisplayUnitToSimUnitRatio(20f);
            font = Content.Load<SpriteFont>("font");

            worldHandler = new WorldHandler(Content, _batch, new Vector2(0, 100f));


            ////////////////////
            //    ADD GAME OBJECTS HERE
            ///////////////////
            _renderedGameObjects = new List<GameObject> {
                new Wall(worldHandler, Content, _worldHitboxes, _batch, new Rectangle(0,420,800,80)),
                new Wall(worldHandler, Content, _worldHitboxes, _batch, new Rectangle(560,367,40,20)),
                new Wall(worldHandler, Content, _worldHitboxes, _batch, new Rectangle(0,0,4,480)),
                new Wall(worldHandler, Content, _worldHitboxes, _batch, new Rectangle(0,400,5,480)),
                new Wall(worldHandler, Content, _worldHitboxes, _batch, new Rectangle(796,0,4,480)),
                new Wall(worldHandler, Content, _worldHitboxes, _batch, new Rectangle(200,340,70,20)),
                new Wall(worldHandler, Content, _worldHitboxes, _batch, new Rectangle(220,320,70,20)),
                new Wall(worldHandler, Content, _worldHitboxes, _batch, new Rectangle(0,0,800,4)),
                new Wall(worldHandler, Content, _worldHitboxes, _batch, new Rectangle(190,400,70,20)),
                new Wall(worldHandler, Content, _worldHitboxes, _batch, new Rectangle(60,325,70,20)),
                new Wall(worldHandler, Content, _worldHitboxes, _batch, new Rectangle(390,398,40,32)),
                new Wall(worldHandler, Content, _worldHitboxes, _batch, new Rectangle(432,388,40,32)),
                new Wall(worldHandler, Content, _worldHitboxes, _batch, new Rectangle(474,388,40,32)),
                new Player(worldHandler, Content,  _batch, new Vector2(350,230)),

                /*new Hitbox(_world, _batch, Content, null, new Rectangle(440, 330, 40, 40), "orange", "orange"),
                new Hitbox(_world, _batch, Content, null, new Rectangle(90, 290, 40, 20), "purple", "purple"),
                new Hitbox(_world, _batch, Content, null, new Rectangle(300, 300, 80, 40), "purple", "purple"),
                new Hitbox(_world, _batch, Content, null, new Rectangle(550, 290, 40, 120), "purple", "purple"),
                new Hitbox(_world, _batch, Content, null, new Rectangle(60, 290, 20, 20), "orange", "orange"),*/
             };

            worldHandler.AddHitbox(null, new Vector2(360, 350), new Vector2(50, 50), "purple");
            worldHandler.AddHitbox(null, new Vector2(380, 340), new Vector2(40, 80), "orange");
            worldHandler.AddHitbox(null, new Vector2(440, 290), new Vector2(80, 40), "purple");
            worldHandler.AddHitbox(null, new Vector2(300, 300), new Vector2(40, 20), "orange");
            worldHandler.AddHitbox(null, new Vector2(550, 290), new Vector2(20, 120), "purple");
            worldHandler.AddHitbox(null, new Vector2(60, 290), new Vector2(40, 40), "orange");

            // Initialize debug
            _font = Content.Load<SpriteFont>("font");
        }

        protected override void Update(GameTime gameTime)
        {
            HandleKeyboard();

            // Physics world step, and then resolve collisions
            // Send to collisions, interacting objects
            //_world.Step((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f);
            worldHandler.PhysicsStep(gameTime);

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

            base.Update(gameTime);

        }


        private void HandleKeyboard()
        {
            KeyboardState state = Keyboard.GetState();
         
            if (state.IsKeyDown(Keys.Escape))
                Exit();

            if (state.IsKeyDown(Keys.F1) && _oldKeyState.IsKeyUp(Keys.F1))
                drawDebug = !drawDebug;

            _oldKeyState = state;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _batch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, _view);

            _batch.DrawString(font, "Toggle Hitbox view:   [F1]", new Vector2(10, 10), Color.White);


            foreach (GameObject obj in _renderedGameObjects)
            {
                obj.Draw();
                if(drawDebug)
                {
                    obj.DrawDebug();
                }
            }
            if (drawDebug)
            {
                worldHandler.DrawDebug();
            }

            _batch.End();
            base.Draw(gameTime);
        }
    }
}