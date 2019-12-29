using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using VelcroPhysics.Dynamics;
using VelcroPhysics.Collision.ContactSystem;
using VelcroPhysics.Dynamics.Solver;
using VelcroPhysics.Factories;
using VelcroPhysics.Utilities;

using System;

namespace VelcroPhysicsPractice.Scripts
{
    public class BodyCollisionHandler
    {
        WorldHandler worldHandler;

        // BodyCollisionHandler Monogame drawing fields
        private readonly Texture2D BodyCollisionHandlerSprite;

        // BodyCollisionHandler VelcroPhysics fields
        private Vector2 BodyCollisionHandlerOrigin;
        //private readonly float gravityScale = 1;

        // hold all collisions here
        public List<CollisionPackage> currentCollisions;
        public List<CollisionPackage> currentFloorCollisions;

        // Game logic misc. fields
        // Hitboxes fields
        //private Dictionary<Fixture, int> currentCollisions;
        //private int floorCollisionsCount = 0;
        public Hitbox bodyHitbox;
        public Hitbox feetHitbox;
        public Vector2 size;
        public Vector2 position;
        //private Fixture feetCollider;
        public Body body;

        // Debug fields and strings
        public bool isFloored;
        private readonly SpriteFont font;

        public BodyCollisionHandler(GameObject owner, WorldHandler rootWorldHandler, Vector2 setPosition, Vector2 setSize)
        {
            // BodyCollisionHandler fields
            worldHandler = rootWorldHandler;
            BodyCollisionHandlerSprite = worldHandler.ContentManager.Load<Texture2D>("white");
            size = setSize;
            BodyCollisionHandlerOrigin = new Vector2(size.X/2, size.Y/2);
            font = worldHandler.ContentManager.Load<SpriteFont>("font");
            //currentCollisions = new Dictionary<Fixture, int>();
            currentCollisions = new List<CollisionPackage>();
            currentFloorCollisions = new List<CollisionPackage>();

            body = worldHandler.AddBody(
                owner,
                setPosition,
                setSize
            );

            bodyHitbox = AddHitbox(
                body,
                Vector2.Zero,
                setSize,
                "red",
                CollisionType.invoker,
                "Player body"
            );

            feetHitbox = AddHitbox(
                body,
                new Vector2(0, size.Y / 2),
                new Vector2(size.X, 2),
                "red",
                CollisionType.invoker,
                "Player feet"
            );
        }

        public Hitbox AddHitbox(Body owner, Vector2 offset, Vector2 size, string color, CollisionType type, string value)
        {
            return worldHandler.AddHitbox(
                body,
                offset,
                size,
                color,
                type,
                value
            );
        }

        public void ClearCollisions()
        {
            currentCollisions.Clear();
            currentFloorCollisions.Clear();
        }

        public void CheckWorldCollisions()
        {
            ClearCollisions();
            worldHandler.CheckHitboxCollision(bodyHitbox, currentCollisions);
            worldHandler.CheckHitboxCollision(feetHitbox, currentFloorCollisions);
            CheckFloored();
        }

        public void CheckFloored()
        {
            if (currentFloorCollisions.Count > 0)
            {
                foreach (CollisionPackage collision in currentFloorCollisions)
                {
                    if(collision.type == CollisionType.wall)
                    {
                        isFloored = true;
                    }
                }
            } else
            {
                isFloored = false;
            }
        }

        public Vector2 getDisplayPosition()
        {
            return ConvertUnits.ToDisplayUnits(body.Position);
        }

        public void DebugDraw(SpriteBatch spriteBatch)
        {
        }
    }
}