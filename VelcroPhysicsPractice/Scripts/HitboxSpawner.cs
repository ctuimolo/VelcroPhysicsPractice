using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using VelcroPhysics.Dynamics;

using VelcroPhysics.Utilities;
using VelcroPhysics.Factories;

using System.Collections.Generic;

namespace VelcroPhysicsPractice.Scripts
{
    struct TimedHitbox
    {
        Hitbox hitbox;
        short timer;
        int id;
    }

    struct StaticHitbox
    {


        Hitbox hitbox;
        int id;
    }

    public class HitboxSpawner
    {
        private List<Hitbox> timeHitboxList;
        private Dictionary<Hitbox, int> hitboxDictionary;

        HitboxSpawner()
        {

        }

        public void CreateTimeHitbox(int setTimer, World rootWorld, SpriteBatch rootSpriteBatch, ContentManager rootContent, GameObject setOwner, Rectangle coordinates, string color, string setValue = "")
        {
            
        }
    }
}
