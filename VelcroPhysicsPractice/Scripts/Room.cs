using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;


using System.Collections.Generic;
using System;

namespace VelcroPhysicsPractice.Scripts
{
    public class Room
    {
        public WorldHandler World { get; protected set; }

        public Room(Point worldSize)
        {
            World = new WorldHandler(worldSize);
        }

        public Room(WorldHandler sharedWorld)
        {
            World = sharedWorld;
        }

        public virtual void LoadContent()   { }
        public virtual void Update()        { }
        public virtual void Draw()          { }
    }
}
