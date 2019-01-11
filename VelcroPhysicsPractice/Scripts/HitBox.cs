using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace VelcroPhysicsPractice.Scripts
{
    class HitBox
    {
        public GameObject owner;
        public Vector2 origin;
        public Vector2 size;
        public Vector2 position;

        public HitBox(GameObject setOwner, Vector2 offset, Vector2 setSize)
        {
            owner = setOwner;

        }

        public void Update()
        {
        }

        public void Draw()
        {

        }
    }
}
