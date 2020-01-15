using System;
using System.Collections.Generic;

namespace VelcroPhysicsPractice.Scripts
{
    public class RoomManager
    {
        public Dictionary<string, Room> Hash;

        public Room CurrentRoom { get; set; }

        public RoomManager()
        {
            Hash = new Dictionary<string, Room>();
        }
    }
}
