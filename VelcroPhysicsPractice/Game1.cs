using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using VelcroPhysics.Dynamics;
using VelcroPhysics.Factories;
using VelcroPhysics.Utilities;

namespace VelcroPhysicsPractice
{
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _batch;
        private KeyboardState _oldKeyState;

        private readonly World _world;

        private Body _playerBody;
        private Body _groundBody;

        private Texture2D _playerSprite;
        private Texture2D _groundSprite;

        private Vector2 _groundSize;

        // Simple camera controls
        private Matrix _view;

        private Vector2 _cameraPosition;
        private Vector2 _screenCenter;
        private Vector2 _groundOrigin;
        private Vector2 _playerOrigin;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 800,
                PreferredBackBufferHeight = 480
            };

            Content.RootDirectory = "Content";

            //Create a world with gravity.
            _world = new World(new Vector2(0, 64f));
            
        }

        protected override void LoadContent()
        {
            // Initialize camera controls
            _view = Matrix.Identity;
            _cameraPosition = Vector2.Zero;
            _screenCenter = new Vector2(_graphics.GraphicsDevice.Viewport.Width / 2f, _graphics.GraphicsDevice.Viewport.Height / 2f);
            _batch = new SpriteBatch(_graphics.GraphicsDevice);

            //_font = Content.Load<SpriteFont>("font");

            // Load sprites
            _playerSprite = Content.Load<Texture2D>("white"); //  96px x 96px => 1.5m x 1.5m
            _groundSprite = Content.Load<Texture2D>("grey"); // 512px x 64px =>   8m x 1m

            /* We need XNA to draw the ground and circle at the center of the shapes */
            _playerOrigin = new Vector2(_playerSprite.Width / 2f, _playerSprite.Height / 2f);
            /*_groundOrigin = new Vector2(_groundSprite.Width / 2f, _groundSprite.Height / 2f);
            _playerOrigin = new Vector2(_playerSprite.Width / 2f, _playerSprite.Height / 2f);*/

            // Velcro Physics expects objects to be scaled to MKS (meters, kilos, seconds)
            // 1 meters equals 64 pixels here
            ConvertUnits.SetDisplayUnitToSimUnitRatio(32f);

            /* Circle */
            // Convert screen center from pixels to meters
            Vector2 playerPosition = ConvertUnits.ToSimUnits(_screenCenter) + new Vector2(0, -1.5f);

            // Create the circle fixture
            _playerBody = BodyFactory.CreateRectangle(_world, ConvertUnits.ToSimUnits(_groundSprite.Width), ConvertUnits.ToSimUnits(_groundSprite.Height), 1f, playerPosition, 0, BodyType.Dynamic);
            _playerBody.FixedRotation = true;
            _playerBody.GravityScale = 1;
            _playerBody.FixtureList[0].CollisionCategories = VelcroPhysics.Collision.Filtering.Category.Cat1;
            _playerBody.FixtureList[0].CollidesWith = VelcroPhysics.Collision.Filtering.Category.Cat1 & VelcroPhysics.Collision.Filtering.Category.Cat1;


            // Give it some bounce and friction
            /*_playerBody.Restitution = 0.3f;
            _playerBody.Friction = 0.5f;*/

            /* Ground */
            Vector2 groundPosition = ConvertUnits.ToSimUnits(_screenCenter) + new Vector2(0, 4f);

            // Create the ground fixture
            _groundSize = new Vector2(720f,32f);
            _groundOrigin = new Vector2(_groundSize.X / 2f, _groundSize.Y / 2f);
            _groundBody = BodyFactory.CreateRectangle(_world, ConvertUnits.ToSimUnits(_groundSize.X), ConvertUnits.ToSimUnits(_groundSize.Y), 1f, groundPosition);
            _groundBody.FixtureList[0].CollisionCategories = VelcroPhysics.Collision.Filtering.Category.Cat1;
            /*_groundBody.Restitution = 0.3f;
            _groundBody.Friction = 0.5f;*/
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            HandleKeyboard();

            //We update the world
            _world.Step((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f);

            base.Update(gameTime);
        }


        private void HandleKeyboard()
        {
            KeyboardState state = Keyboard.GetState();

            // Move camera
            if (state.IsKeyDown(Keys.Left))
                _cameraPosition.X += 1.5f;

            if (state.IsKeyDown(Keys.Right))
                _cameraPosition.X -= 1.5f;

            if (state.IsKeyDown(Keys.Up))
                _cameraPosition.Y += 1.5f;

            if (state.IsKeyDown(Keys.Down))
                _cameraPosition.Y -= 1.5f;

            _view = Matrix.CreateTranslation(new Vector3(_cameraPosition - _screenCenter, 0f)) * Matrix.CreateTranslation(new Vector3(_screenCenter, 0f));

            // We make it possible to rotate the circle body

            if (state.IsKeyDown(Keys.A))
                _playerBody.LinearVelocity = new Vector2(-3f, _playerBody.LinearVelocity.Y);

            if (state.IsKeyDown(Keys.D))
                _playerBody.LinearVelocity = new Vector2(3f, _playerBody.LinearVelocity.Y);

            if (!state.IsKeyDown(Keys.D) && !state.IsKeyDown(Keys.A))
                _playerBody.LinearVelocity = new Vector2(0, _playerBody.LinearVelocity.Y);

            if (state.IsKeyDown(Keys.Space) && _oldKeyState.IsKeyUp(Keys.Space))
                _playerBody.LinearVelocity = new Vector2(_playerBody.LinearVelocity.X, -20f);

            if (state.IsKeyDown(Keys.Escape))
                Exit();

            _oldKeyState = state;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            //Draw circle and ground
            _batch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, _view);
            _batch.Draw(_playerSprite, ConvertUnits.ToDisplayUnits(_playerBody.Position), new Rectangle(0,0,32,32), Color.White, _playerBody.Rotation, _playerOrigin, 1f, SpriteEffects.None, 0f);
            _batch.Draw(_groundSprite, ConvertUnits.ToDisplayUnits(_groundBody.Position), new Rectangle(0,0,(int)_groundSize.X,(int)_groundSize.Y), Color.White, _groundBody.Rotation, _groundOrigin, 1f, SpriteEffects.None, 0f);
            _batch.End();

            base.Draw(gameTime);
        }
    }
}