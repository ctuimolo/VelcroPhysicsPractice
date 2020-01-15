using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace VelcroPhysicsPractice.Debug
{
    public static class Assets
    {
        public static SpriteFont    DebugFont   = Game.Assets.Load<SpriteFont>("font");
        public static Texture2D     RedBox      = Game.Assets.Load<Texture2D>("red");
        public static Texture2D     BlueBox     = Game.Assets.Load<Texture2D>("blue");
        public static Texture2D     GreenBox    = Game.Assets.Load<Texture2D>("green");
        public static Texture2D     OrangeBox   = Game.Assets.Load<Texture2D>("orange");
        public static Texture2D     PurpleBox   = Game.Assets.Load<Texture2D>("purple");
        public static Texture2D     PinkBox     = Game.Assets.Load<Texture2D>("pink");
        public static Texture2D     GreyBox     = Game.Assets.Load<Texture2D>("grey");
        public static Texture2D     BlackBox    = Game.Assets.Load<Texture2D>("black");
    }
}
