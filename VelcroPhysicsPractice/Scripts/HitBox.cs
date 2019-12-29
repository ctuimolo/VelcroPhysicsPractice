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
        private WorldHandler worldHander;

        // Hitboxfields
        public Body owner;
        public Vector2 origin;
        public Vector2 offset;
        public Vector2 size;
        public Vector2 position;
        public CollisionPackage collisionPackage;
        public string value;
        public bool enabled = true;
        public bool delete = false;

        public delegate void enact();

        // VelcroPhysics bodies
        //private Body body;

        public Hitbox(WorldHandler rootWorldHandler, Body setOwner, Vector2 setOffset, Vector2 setSize, string color, CollisionType type, string value)
        {
            worldHander = rootWorldHandler;
            contentManager = worldHander.ContentManager;
            spriteBatch = worldHander.SpriteBatch;
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
            collisionPackage = new CollisionPackage 
            {
                value = value,
                type = type
            };
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
