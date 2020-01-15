using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;


using System.Collections.Generic;
using System;

namespace VelcroPhysicsPractice.Scripts
{
    public class RoomHandler
    {
        public Dictionary<string, Room> Hash { get; private set; }

        public RoomHandler()
        {
            Hash = new Dictionary<string, Room>();
        }
    }
}
