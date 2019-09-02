using City.Engine.Components;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace City.Engine
{
    ///<summary>Base for everything on the scene/summary>
    public class Actor : Object
    {

        public Microsoft.Xna.Framework.Vector3 location;

        /// <summary>
        /// x-Yaw,y-Pitch,Z-roll in radians
        /// </summary>
        public Microsoft.Xna.Framework.Vector3 rotation;

        protected double lifeTime;

        protected double livedTime;

        protected bool pendingKill = false;

        protected List<Actor> children = new List<Actor>();

        protected List<Component> components = new List<Component>();

        /// <summary>
        /// Id that used if there mulitiple actors with the same name
        /// Used to give proper name
        /// </summary>
        public int Id = 0;

        #region Properties
        public bool PendingKill { get => pendingKill; set => pendingKill = value; }
        public double LivedTime { get => livedTime; set => livedTime = value; }
        public double LifeTime { get => lifeTime; set => lifeTime = value; }

        internal List<Actor> Children { get => children; set => children = value; }
        public List<Component> Components { get => components; set => components = value; }



        #endregion

        public virtual void OnBeginContact(Actor otherActor, Box2DX.Collision.Shape shape, Box2DX.Collision.Shape otherShape)
        {

        }

        public virtual void OnEndContact(Actor otherActor, Box2DX.Collision.Shape shape, Box2DX.Collision.Shape otherShape)
        {

        }

        public Matrix GetMatrix()
        {
            return Matrix.CreateRotationX(MathHelper.ToRadians(rotation.X))
                * Matrix.CreateRotationY(MathHelper.ToRadians(rotation.Y))
                * Matrix.CreateRotationZ(MathHelper.ToRadians(rotation.Z))
                * Matrix.CreateTranslation(location);
        }
        public delegate void OnMousePressedDelegate();

        public event OnMousePressedDelegate OnMousePressed;

        ///<param name="lifeTime">lifetime=0 for infinite</param>
        public Actor(GameHandler game, Vector3 location, Vector3 rotation, double lifeTime) : base(game)
        {
            this.location = location;
            this.lifeTime = lifeTime;
        }

        ///<param name="lifeTime">lifetime=0 for infinite. It uses seconds</param>
        ///<param name="Name">Id at the name is based on Id of actor not on the part of the string</param>
        public Actor(GameHandler game, string Name, Vector3 location, Vector3 rotation, double lifeTime) : base(game, Name)
        {
            this.location = location;
            this.lifeTime = lifeTime;
        }

        public virtual bool IsValid() { return !PendingKill; }

        /// <summary>
        /// Happens during loadContent, but can be called outside
        /// </summary>
        public override void Init()
        {
            foreach (var comp in components)
            {
                comp.Init();
            }
        }

        public override void HandleInput(Microsoft.Xna.Framework.Input.Keys[] keys)
        {
            foreach (var comp in components)
            {
                comp.HandleInput(keys);
            }
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            foreach (var comp in components)
            {
                comp.Update(gameTime);
            }
            if (lifeTime != 0.0f)
            {
                livedTime += gameTime.ElapsedGameTime.TotalSeconds;
                if (livedTime >= lifeTime)
                {
                    this.Dispose();
                }
            }

        }

        public virtual void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            foreach (var comp in components)
            {
                if (comp is Components.DrawableComponent)
                {
                    (comp as Components.DrawableComponent).Draw(spriteBatch);

                }
            }
        }

        public virtual void Draw(Matrix viewMatrix, Matrix projectionMatrix)
        {
            foreach (var comp in components)
            {
                if (comp is Components.DrawableComponent)
                {
                    (comp as Components.DrawableComponent).Draw(viewMatrix, projectionMatrix);

                }
            }
        }

        public virtual void Destroy()
        {
            PendingKill = true;
            Dispose();
        }

        public override void Dispose()
        {
            
            PendingKill = true;
            foreach (var item in components)
            {
                item.Dispose();
            }
            foreach (var child in Children)
            {
                child.Dispose();
            }
            System.GC.SuppressFinalize(this);
        }

    }

}
