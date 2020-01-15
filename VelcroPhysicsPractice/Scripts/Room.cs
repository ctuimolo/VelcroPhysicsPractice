using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Collections.Generic;



namespace VelcroPhysicsPractice.Scripts
{
    public class Room
    {
        private List<GameObject>    _renderedGameObjects;
        private bool                _drawDebug = false;
        private KeyboardState       _oldKeyState;

        public WorldHandler World       { get; private set; }
        public Point        RoomSize    { get; private set; }

        public Room(Point roomSize)
        {
            RoomSize = RoomSize;
        }

        public void LoadContent()
        {
            World = new WorldHandler(RoomSize);

            _renderedGameObjects = new List<GameObject>
            {
                new Wall(new Rectangle(0,420,800,80)),
                new Wall(new Rectangle(560,312,40,20)),
                new Wall(new Rectangle(0,0,4,480)),
                new Wall(new Rectangle(0,400,5,480)),
                new Wall(new Rectangle(796,0,4,480)),
                new Wall(new Rectangle(200,300,70,20)),
                new Wall(new Rectangle(220,280,70,20)),
                new Wall(new Rectangle(0,0,800,4)),
                new Wall(new Rectangle(190,400,70,20)),
                new Wall(new Rectangle(60,325,70,20)),
                new Wall(new Rectangle(390,388,40,32)),
                new Wall(new Rectangle(432,388,40,32)),
                new Wall(new Rectangle(474,388,40,32)),
                new Wall(new Rectangle(516,388,40,32)),
                new Wall(new Rectangle(558,388,40,32)),
                new Wall(new Rectangle(600,388,40,32)),
                new Wall(new Rectangle(642,388,40,32)),

                new Player(new Vector2(250,230)),
             };

            Hitbox tmpBox;

            tmpBox = World.AddHitbox(null, new Vector2(420, 310), new Point(30, 60), "orange");
            tmpBox.Data.Value = "orange";
            tmpBox.Data.String = "spuds";

            tmpBox = World.AddHitbox(null, new Vector2(550, 290), new Point(20, 120), "purple");
            tmpBox.Data.Value = "purple";
            tmpBox.Data.String = "cats";

            tmpBox = World.AddHitbox(null, new Vector2(60, 290), new Point(40, 40), "orange");
            tmpBox.Data.Value = "orange";
            tmpBox.Data.String = "bruh";

            tmpBox = World.AddHitbox(null, new Vector2(440, 290), new Point(80, 40), "purple");
            tmpBox.Data.Value = "purple";
            tmpBox.Data.String = "crunchy";

            tmpBox = World.AddHitbox(null, new Vector2(300, 250), new Point(40, 20), "orange");
            tmpBox.Data.Value = "orange";
            tmpBox.Data.String = "trash";

            tmpBox = World.AddHitbox(null, new Vector2(280, 340), new Point(20, 50), "purple");
            tmpBox.Data.Value = "purple";
            tmpBox.Data.String = "curly";

            tmpBox = World.AddHitbox(null, new Vector2(300, 340), new Point(20, 50), "orange");
            tmpBox.Data.Value = "orange";
            tmpBox.Data.String = "d00d";

            tmpBox = World.AddHitbox(null, new Vector2(320, 340), new Point(20, 50), "purple");
            tmpBox.Data.Value = "purple";
            tmpBox.Data.String = "very nice";

            tmpBox = World.AddHitbox(null, new Vector2(580, 120), new Point(66, 12), "purple");
            tmpBox.Data.Value = "purple";
            tmpBox.Data.String = "cute funny";

            tmpBox = World.AddHitbox(null, new Vector2(400, 620), new Point(40, 40), "orange");
            tmpBox.Data.Value = "orange";
            tmpBox.Data.String = "popcorn";

            tmpBox = World.AddHitbox(null, new Vector2(21, 400), new Point(24, 50), "purple");
            tmpBox.Data.Value = "purple";
            tmpBox.Data.String = "bruh bruh bruh";
        }

        private void HandleKeyboard()
        {
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.F1) && _oldKeyState.IsKeyUp(Keys.F1))
                _drawDebug = !_drawDebug;

            _oldKeyState = state;
        }

        public void UpdateWorld()
        {
            World.PhysicsStep();
        }

        public void Update()
        {
            // Physics world step, and then resolve collisions
            // Send to collisions, interacting objects
            World.PhysicsStep();

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
        }

        public void Draw()
        {
            foreach (GameObject obj in _renderedGameObjects)
            {
                obj.Draw();
                if (_drawDebug)
                {
                    obj.DrawDebug();
                }
            }

            if (_drawDebug)
            {
                World.DrawDebug();
            }
        }

        public void DrawDebug()
        {

        }
    }
}
