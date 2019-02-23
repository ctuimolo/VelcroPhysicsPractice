﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using VelcroPhysics.Dynamics;

using VelcroPhysics.Utilities;
using VelcroPhysics.Factories;

using System;
using System.Collections.Generic;

namespace VelcroPhysicsPractice.Scripts
{
    public struct CollisionPackage
    {
        public CollisionType type;
        public string value;
    }

    public class Hitbox : GameObject
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
        private CollisionPackage collisionPackage;
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
        }

        /*public Hitbox(World rootWorld, SpriteBatch rootSpriteBatch, ContentManager rootContent, GameObject setOwner, Rectangle coordinates, string color, string setValue = "")
        {
            spriteBatch = rootSpriteBatch;
            owner = setOwner;

            sprite = rootContent.Load<Texture2D>(color);

            offset = new Vector2(coordinates.X, coordinates.Y);
            size = new Vector2(coordinates.Width, coordinates.Height);
            origin = new Vector2(size.X/2, size.Y/2);
            if (owner != null)
            {
                position = ConvertUnits.ToDisplayUnits(body.Position) + offset;
            } else
            {
                position = offset;
            }

            collisionPackage = new CollisionPackage()
            {
                type = CollisionType.invoker,
                value = setValue,
            };

            body = BodyFactory.CreateRectangle(
                rootWorld,
                ConvertUnits.ToSimUnits(size.X),
                ConvertUnits.ToSimUnits(size.Y),
                1f,
                ConvertUnits.ToSimUnits(position + new Vector2(size.X / 2, size.Y / 2)),
                0,
                BodyType.Kinematic,
                collisionPackage
            );
            body.FixedRotation = true;
            body.Friction = 0;
            body.FixtureList[0].IsSensor = true;
        }*/

        public override void LoadContent()
        {
        }

        public override void Initialize()
        {
        }

        public override void Update()
        {
            if(owner != null) { 
                position = ConvertUnits.ToDisplayUnits(owner.Position) + offset;
            }
        }

        public override void Draw()
        {

        }

        public override void DrawDebug()
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
