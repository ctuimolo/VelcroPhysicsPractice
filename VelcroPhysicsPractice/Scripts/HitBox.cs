using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using VelcroPhysics.Dynamics;

using VelcroPhysics.Utilities;
using VelcroPhysics.Factories;

using System;
using System.Collections.Generic;

namespace VelcroPhysicsPractice.Scripts
{
    public enum CollisionType
    {
        none,
        wall,
        invoker,
    };

    public class CollisionPackage
    {
        public CollisionType type = CollisionType.none;
        public string value = "";
    }

    public class Hitbox
    {
        // Monogame drawing fields
        private SpriteBatch spriteBatch;
        private readonly Texture2D sprite;
        private ContentManager contentManager;

        // Hitboxfields
        public Body owner;
        public Vector2 origin;
        public Vector2 offset;
        public Vector2 size;
        public Vector2 position;
        public CollisionPackage collisionPackage;
        public string value;

        public delegate void enact();

        // VelcroPhysics bodies
        //private Body body;

        public Hitbox(ContentManager setContentManager, SpriteBatch setSpriteBatch, Body setOwner, Vector2 setOffset, Vector2 setSize, string color)
        {
            contentManager = setContentManager;
            spriteBatch = setSpriteBatch;
            owner = setOwner;
            size = setSize;
            origin = new Vector2(size.X / 2, size.Y / 2);
            offset = setOffset;
            if (owner != null)
            {
                position = ConvertUnits.ToDisplayUnits(owner.Position) + offset;
            } else
            {
                position = offset;
            }
            sprite = contentManager.Load<Texture2D>(color);
            collisionPackage = new CollisionPackage();
        }

        public void LoadContent()
        {
        }

        public void Initialize()
        {
        }

        public void Update()
        {
            if(owner != null) { 
                position = ConvertUnits.ToDisplayUnits(owner.Position) + offset;
            }
        }

        public void Draw()
        {

        }

        public void DrawDebug()
        {
            spriteBatch.Draw(
                sprite,
                position,
                new Rectangle(0, 0, (int)size.X, (int)size.Y),
                new Color(255,255,255,120),
                0f,
                origin,
                1f,
                SpriteEffects.None,
                0f
            );
        }
    }
}
