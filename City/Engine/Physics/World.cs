using Box2DX.Collision;
using Box2DX.Dynamics;

namespace City.Engine.Physics
{
    /// <summary>
    /// Helps with using BOX2D's world. It uses City.Engine.Physics.ContactListener
    /// </summary>
    public class PhysicsWorld
    {
        readonly Box2DX.Dynamics.World world;
        Box2DX.Collision.AABB worldBoundaries;
        Box2DX.Common.Vec2 gravity;


        public PhysicsWorld(Box2DX.Common.Vec2 gravity, Box2DX.Common.Vec2 worldLowerBound, Box2DX.Common.Vec2 worldUpperBound)
        {
            AABB bound = new AABB();
            bound.LowerBound.Set(worldLowerBound.X, worldLowerBound.Y);
            bound.UpperBound.Set(worldUpperBound.X, worldUpperBound.Y);
            world = new World(bound, gravity, true);
            Engine.Physics.ContactListener listener = new Engine.Physics.ContactListener();
            world.SetContactListener(listener);
        }

        public PhysicsWorld(Box2DX.Common.Vec2 gravity, AABB worldBoundaries)
        {
            world = new World(worldBoundaries, gravity, true);
            Engine.Physics.ContactListener listener = new Engine.Physics.ContactListener();
            world.SetContactListener(listener);
        }

        public virtual void Step(Microsoft.Xna.Framework.GameTime gameTime, int velocityIterations, int positionIteration)
        {
            world.Step((float)gameTime.ElapsedGameTime.TotalSeconds, velocityIterations, positionIteration);
        }

        public virtual Body CreateBody(BodyDef bodyDef)
        {
            return world.CreateBody(bodyDef);
        }

        /// <summary>
        /// Disposes of body
        /// </summary>
        /// <param name="body"></param>
        public virtual void RemoveBody(Body body)
        {
            world.DestroyBody(body);
        }

        public Box2DX.Dynamics.World World { get => world; }
        public AABB WorldBoundaries { get => worldBoundaries; }
    }
}
