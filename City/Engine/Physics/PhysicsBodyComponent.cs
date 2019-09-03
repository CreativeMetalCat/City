using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace City.Engine.Components.Physics
{
    /// <summary>
    /// Component that defines physics shapes(fixtures)
    /// </summary>
    class ShapeComponent : Component
    {

       public  static ShapeComponent CreateRectangeShape(GameHandler game, Actor owner,bool isSensor, float density, float friction, float halfX, float halfY, string Name = "RectangeShape")
        {
            ShapeComponent component = new ShapeComponent(game, Name, owner);
            component.shapeDef = new Box2DX.Collision.PolygonDef();
            component.shapeDef.Type = Box2DX.Collision.ShapeType.PolygonShape;
            component.shapeDef.Density = density;
            component.shapeDef.Friction = friction;
            component.shapeDef.IsSensor = isSensor;
            component.shapeDef.UserData = owner;

            (component.shapeDef as Box2DX.Collision.PolygonDef).SetAsBox(halfX / game.physicsScaleX, halfY / game.physicsScaleY);

            return component;
        }

        public Box2DX.Collision.ShapeDef shapeDef;

        public ShapeComponent(GameHandler game, Actor owner) : base(game, owner)
        {
           
        }

        public ShapeComponent(GameHandler game, string Name, Actor owner) : base(game, Name, owner)
        {
        }

        public override void Dispose()
        {
            
        }

    }

    class PhysicsBodyComponent : Component
    {
        public Box2DX.Dynamics.Body body;

        /// <summary>
        /// THIS VALUE IS NOT RELATIVE TO PARENT AND SCALED DOWN. USE GETWORLDLCOATION INSTEAD.
        /// Location of the body in world
        /// </summary>
        public Vector2 location;

        /// <summary>
        /// Returns location of the body in world
        /// </summary>
        /// <returns></returns>
        public override Vector3 GetWorldLocation()
        {
            return new Vector3(location.X * Game.physicsScaleX, location.Y * Game.physicsScaleY, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        /// <param name="location">Unscaled</param>
        /// <param name="fixedRotation"></param>
        /// <param name="owner"></param>
        public PhysicsBodyComponent(GameHandler game, Vector2 location, bool fixedRotation, Actor owner) : base(game, owner)
        {
            Box2DX.Dynamics.BodyDef bodyDef = new Box2DX.Dynamics.BodyDef();
            bodyDef.Position.Set(location.X / game.physicsScaleX, location.Y / game.physicsScaleY);
            bodyDef.FixedRotation = fixedRotation;
            bodyDef.UserData = owner;
            body = game.physicsWorld.CreateBody(bodyDef);
            body.SetUserData(owner);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        /// <param name="location">Unscaled</param>
        /// <param name="fixedRotation"></param>
        /// <param name="owner"></param>
        public PhysicsBodyComponent(GameHandler game, string Name, Vector2 location, bool fixedRotation, Actor owner) : base(game, Name, owner)
        {
            Box2DX.Dynamics.BodyDef bodyDef = new Box2DX.Dynamics.BodyDef();
            bodyDef.Position.Set(location.X / game.physicsScaleX, location.Y / game.physicsScaleY);
            bodyDef.FixedRotation = fixedRotation;
            bodyDef.UserData = owner;
            body = game.physicsWorld.CreateBody(bodyDef);
            body.SetUserData(owner);
        }

        public override void Init()
        {
            base.Init();

            foreach (PhysicsBodyComponent physicsBody in owner.Components.OfType<PhysicsBodyComponent>())
            {
                if (physicsBody != this) { throw new Engine.Exceptions.PhysicsBodyOverflow("Owner has another physics body. There must one physics body per actor"); }
            }

            foreach (ShapeComponent shape in owner.Components.OfType<ShapeComponent>())
            {
                if (shape.shapeDef != null)
                {
                    body.CreateShape(shape.shapeDef);
                }
            }
            body.SetMassFromShapes();
        }

        public void ApplyImpulseAtCenter(Vector2 impulse)
        {
            body.ApplyImpulse(impulse.ToBox2DVector(), body.GetWorldCenter());
        }

        public void ApplyImpulse(Vector2 impulse, Vector2 point)
        {
            body.ApplyImpulse(impulse.ToBox2DVector(), point.ToBox2DVector());
        }

        public Vector2 GetVelocity()
        {
            return body.GetLinearVelocity().ToVector();
        }

        public float GetMass()
        {
            return body.GetMass();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            location = new Vector2(body.GetPosition().X, body.GetPosition().Y);
            owner.location = GetWorldLocation();
        }

        public override void Dispose()
        {
            Game.physicsWorld.RemoveBody(body);
        }
    }
}
