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
        private readonly float gravityScale = 1;

        // Game logic misc. fields
        private KeyboardState _oldKeyState;

        // Hitboxes fields
        private Dictionary<Fixture, int> currentCollisions;
        private int floorCollisionsCount = 0;
        public Hitbox bodyHitbox;
        public Hitbox feetHitbox;
        public Vector2 size;
        public Vector2 position;
        private Fixture feetCollider;
        public Body body;

        // Debug fields and strings
        private bool afterCollision = false;
        private string afterCollisionString;
        private string positionDebugString;
        public bool isFloored;
        private string isFlooredString;
        private bool isOverlappingOrange;
        private string isOverlappingOrangeString;
        private bool isOverlappingPink;
        private string isOverlappingPinkString;
        private readonly SpriteFont font;

        public BodyCollisionHandler(GameObject owner, WorldHandler rootWorldHandler, ContentManager rootContent, SpriteBatch rootSpriteBatch, Vector2 setPosition, Vector2 setSize)
        {
            // BodyCollisionHandler fields
            worldHandler = rootWorldHandler;
            BodyCollisionHandlerSprite = rootContent.Load<Texture2D>("white");
            size = setSize;
            BodyCollisionHandlerOrigin = new Vector2(size.X/2, size.Y/2);
            font = rootContent.Load<SpriteFont>("font");
            currentCollisions = new Dictionary<Fixture, int>();

            body = worldHandler.AddBody(
                owner,
                setPosition,
                setSize
            );
            bodyHitbox = worldHandler.AddHitbox(
                body,
                Vector2.Zero,
                setSize,
                "red"
            );
            feetHitbox = worldHandler.AddHitbox(
                body,
                new Vector2(0, size.Y / 2 + 1),
                new Vector2(size.X, 1),
                "red"
            );

            // VelcroPhysics body configuration
            /*body = BodyFactory.CreateRectangle(
                rootWorld,
                ConvertUnits.ToSimUnits(size.X),
                ConvertUnits.ToSimUnits(size.Y),
                1f,
                ConvertUnits.ToSimUnits(setPosition + new Vector2(size.X / 2, size.Y / 2)),
                0,
                BodyType.Dynamic,
                this
            );
            body.FixtureList[0].UserData = new Rectangle(0,0,(int)size.X,(int)size.Y);
            body.FixedRotation = true;
            body.GravityScale = gravityScale;
            body.Friction = 0;

            foreach (Fixture fixture in body.FixtureList)
            {
                fixture.AfterCollision += Collision;
                fixture.OnCollision += SensorCollsion;
                fixture.OnSeparation += SensorSeparation;
                fixture.OnSeparation += WallSeparation;
            }

            // Feet collilder
            feetCollider = FixtureFactory.AttachRectangle(
                ConvertUnits.ToSimUnits(size.X - 1), // Needs to be inbetween the width of the body, and 1 pixel smaller than body
                ConvertUnits.ToSimUnits(1),
                1f,
                ConvertUnits.ToSimUnits(new Vector2(0, size.Y/2)),
                body,
                new Rectangle(0, (int)size.Y/2, (int)size.X, 1)
            );
            feetCollider.IsSensor = true;
            feetCollider.OnCollision += FloorCollision;
            feetCollider.OnSeparation += FloorSeparation;*/
        }


        void Collision(Fixture fixtureA, Fixture fixtureB, Contact contact, ContactVelocityConstraint impulse)
        {
            afterCollision = true;
        }

        void FloorCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (fixtureB.Body.UserData is Wall)
            {
                floorCollisionsCount++;
                isFloored = true;
            }
        }

        void FloorSeparation(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (fixtureB.Body.UserData is Wall)
            {
                floorCollisionsCount--;
                if (floorCollisionsCount <= 0)
                {
                    isFloored = false;
                }
            }
        }

        void WallCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            afterCollision = true;
        }

        void WallSeparation(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            afterCollision = false;
        }

        void SensorCollsion(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (fixtureB.IsSensor)
            {
                switch(((CollisionPackage)fixtureB.Body.UserData).value) {
                    case "orange":

                        if (currentCollisions.ContainsKey(fixtureB))
                        {
                            currentCollisions[fixtureB] += 1;
                        } else
                        {
                            currentCollisions.Add(fixtureB, 1);
                            isOverlappingOrange = true;
                        }
                        break;
                    case "purple":
                        if (currentCollisions.ContainsKey(fixtureB))
                        {
                            currentCollisions[fixtureB] += 1;
                        }
                        else
                        {
                            currentCollisions.Add(fixtureB, 1);
                            isOverlappingPink = true;
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        void SensorSeparation(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (fixtureB.IsSensor)
            {
                switch (((CollisionPackage)fixtureB.Body.UserData).value)
                {
                    case "orange":
                        if(currentCollisions.ContainsKey(fixtureB))
                        {
                            currentCollisions[fixtureB] -= 1;
                            if (currentCollisions[fixtureB] == 0)
                            {
                                isOverlappingOrange = false;
                                currentCollisions.Remove(fixtureB);
                            }
                        } 
                        break;
                    case "purple":
                        if (currentCollisions.ContainsKey(fixtureB))
                        {
                            currentCollisions[fixtureB] -= 1;
                            if (currentCollisions[fixtureB] == 0)
                            {
                                isOverlappingPink = false;
                                currentCollisions.Remove(fixtureB);
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        public Vector2 getDisplayPosition()
        {
            return ConvertUnits.ToDisplayUnits(body.Position);
        }

        public void DebugDraw(SpriteBatch spriteBatch)
        {

            //////////////////////////////////////////////////////////////
            ///// Setup debug string
            positionDebugString = "Position: \n" +
                                  "X: " + (int)(ConvertUnits.ToDisplayUnits(body.Position.X) - size.X / 2) + "\n" +
                                  "Y: " + (int)(ConvertUnits.ToDisplayUnits(body.Position.Y) - size.Y / 2) + "\n";

            isFlooredString = "                   Grounded:    " + (isFloored ? "true" : "false");
            isOverlappingOrangeString = "   Hitbox Collisions:    " + (isOverlappingOrange ? "true" : "false");
            isOverlappingPinkString = "   Hitbox Collisions:    " + (isOverlappingPink ? "true" : "false");
            afterCollisionString = "Collisions present:    " + (afterCollision ? "true" : "false");

            spriteBatch.DrawString(font, positionDebugString, ConvertUnits.ToDisplayUnits(body.Position) + new Vector2(-60,-68), Color.CornflowerBlue);
            spriteBatch.DrawString(font, isFlooredString, ConvertUnits.ToDisplayUnits(body.Position) + new Vector2(-60, -82), Color.Gray);
            spriteBatch.DrawString(font, afterCollisionString, ConvertUnits.ToDisplayUnits(body.Position) + new Vector2(-60, -96), Color.Gray);
            spriteBatch.DrawString(font, "A " + isOverlappingPinkString, ConvertUnits.ToDisplayUnits(body.Position) + new Vector2(-20, -54), Color.Violet);
            spriteBatch.DrawString(font, "B " + isOverlappingOrangeString, ConvertUnits.ToDisplayUnits(body.Position) + new Vector2(-20, -40), Color.Orange);

            foreach (Fixture fixture in body.FixtureList)
            {
                if (!fixture.IsSensor)
                {
                    spriteBatch.Draw(
                        BodyCollisionHandlerSprite,
                        ConvertUnits.ToDisplayUnits(fixture.Body.Position) + new Vector2(((Rectangle)fixture.UserData).X, ((Rectangle)fixture.UserData).Y),
                        new Rectangle(0, 0, ((Rectangle)fixture.UserData).Width, ((Rectangle)fixture.UserData).Height),
                        new Color(10,200,200,90),
                        fixture.Body.Rotation,
                        new Vector2(((Rectangle)fixture.UserData).Width / 2, ((Rectangle)fixture.UserData).Height / 2),
                        1f,
                        SpriteEffects.None,
                        0f
                    );
                } else
                {
                    spriteBatch.Draw(
                        BodyCollisionHandlerSprite,
                        ConvertUnits.ToDisplayUnits(fixture.Body.Position) + new Vector2(((Rectangle)fixture.UserData).X, ((Rectangle)fixture.UserData).Y),
                        new Rectangle(0, 0, ((Rectangle)fixture.UserData).Width, ((Rectangle)fixture.UserData).Height),
                        new Color(255,0,0,90),
                        fixture.Body.Rotation,
                        new Vector2(((Rectangle)fixture.UserData).Width / 2, ((Rectangle)fixture.UserData).Height / 2),
                        1f,
                        SpriteEffects.None,
                        0f
                    );
                }
            }
        }
    }
}