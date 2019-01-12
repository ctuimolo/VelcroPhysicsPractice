using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using VelcroPhysics.Dynamics;

using VelcroPhysics.Utilities;
using VelcroPhysics.Factories;

namespace VelcroPhysicsPractice.Scripts
{
    public class HitBox
    {
        // Monogame drawing fields
        private SpriteBatch spriteBatch;
        private readonly Texture2D sprite;

        // Hitbox fields
        public GameObject owner;
        public Vector2 origin;
        public Vector2 size;
        public Vector2 position;
        public Vector2 offset;

        // VelcroPhysics bodies
        public Body body;

        public HitBox(World rootWorld, SpriteBatch rootSpriteBatch, ContentManager rootContent, GameObject setOwner, Rectangle coordinates, string color)
        {
            spriteBatch = rootSpriteBatch;
            owner = setOwner;

            sprite = rootContent.Load<Texture2D>(color);

            offset = new Vector2(coordinates.X, coordinates.Y);
            size = new Vector2(coordinates.Width, coordinates.Height);
            origin = new Vector2(size.X/2, size.Y/2);
            if (owner != null)
            {
                position = ConvertUnits.ToDisplayUnits(owner.body.Position) + offset;
            } else
            {
                position = offset;
            }

            body = BodyFactory.CreateRectangle(
                rootWorld,
                ConvertUnits.ToSimUnits(size.X),
                ConvertUnits.ToSimUnits(size.Y),
                1f,
                ConvertUnits.ToSimUnits(position + new Vector2(size.X / 2, size.Y / 2)),
                0,
                BodyType.Kinematic,
                this
            );
            body.FixedRotation = true;
            body.FixtureList[0].CollisionCategories = VelcroPhysics.Collision.Filtering.Category.Cat1;
            body.Friction = 0;
            body.IsSensor = true;
        }

        public void Update()
        {
            position = ConvertUnits.ToDisplayUnits(owner.body.Position) + offset;
        }

        public void Draw()
        {
            spriteBatch.Draw(
                sprite,
                position,
                new Rectangle(0, 0, (int)size.X, (int)size.Y),
                new Color(255,255,255,160),
                0f,
                origin,
                1f,
                SpriteEffects.None,
                0f
            );
        }
    }
}
