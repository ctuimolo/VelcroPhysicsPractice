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
        private readonly World _world;

        // For camera controls
        private Matrix _view;

        // Game Objects to make
        private List<GameObject> _renderedGameObjects;

        // Debug
        private string contactinRangeString;
        private HitBox testHitbox;
        private bool drawDebug = true;
        private KeyboardState _oldKeyState;

        public Game()
        {
            _graphics = new GraphicsDeviceManager(this) {
                PreferredBackBufferWidth = 800,
                PreferredBackBufferHeight = 480
            };

            Content.RootDirectory = "Content";
            _world = new World(new Vector2(0, 100f));
            
        }

        bool AABBoverlapping(GameObject self, GameObject other)
        {
            // if self.left >= other.right && self.right <= other.left
            

            return false;
        }

        protected override void LoadContent()
        {
            // Initialize camera controls
            _view = Matrix.Identity;
            _batch = new SpriteBatch(_graphics.GraphicsDevice);
            ConvertUnits.SetDisplayUnitToSimUnitRatio(20f);

            ////////////////////
            //    ADD GAME OBJECTS HERE
            ///////////////////
            _renderedGameObjects = new List<GameObject> {
                new Wall(Content, _world, _batch, new Rectangle(0,420,800,80)),
                new Wall(Content, _world, _batch, new Rectangle(0,0,4,480)),
                new Wall(Content, _world, _batch, new Rectangle(0,400,5,480)),
                new Wall(Content, _world, _batch, new Rectangle(796,0,4,480)),
                new Wall(Content, _world, _batch, new Rectangle(200,340,70,20)),
                new Wall(Content, _world, _batch, new Rectangle(220,320,70,20)),
                new Wall(Content, _world, _batch, new Rectangle(0,0,800,4)),
                new Wall(Content, _world, _batch, new Rectangle(190,400,70,20)),
                new Wall(Content, _world, _batch, new Rectangle(60,325,70,20)),
                new Wall(Content, _world, _batch, new Rectangle(390,388,40,32)),
                new Player(Content, _world, _batch, new Vector2(350,230)),
            };

            testHitbox = new HitBox(_world, _batch, Content, null, new Rectangle(440, 355, 40, 40), "purple");

            // Initialize debug
            _font = Content.Load<SpriteFont>("font");
        }

        protected override void Update(GameTime gameTime)
        {
            HandleKeyboard();

            ///// Physics world step, and then resolve collisions
            /// Send to collisions, interacting objects
            _world.Step((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f);

            foreach (VelcroPhysics.Collision.ContactSystem.Contact contact in _world.ContactList)
            {
                if(contact != null)
                {
                    contactinRangeString = "true";
                    if(AABBoverlapping((GameObject)contact.FixtureA.Body.UserData, (GameObject)contact.FixtureB.Body.UserData))
                    {
                        Console.WriteLine("COLLIDED");
                    }
                }
            }

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

            foreach (GameObject obj in _renderedGameObjects)
            {
                obj.Draw();
                _batch.DrawString(_font, "Contacts in range:     " + contactinRangeString, new Vector2(60,38), Color.Gray);
            }

            if (drawDebug)
            {
                testHitbox.Draw();
            }
            _batch.End();
            base.Draw(gameTime);
            contactinRangeString = "false";
        }
    }
}