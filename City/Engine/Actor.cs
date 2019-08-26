using City.Engine.Components;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace City.Engine
{
    ///<summary>Base for everything on the scene/summary>
    public class Actor : Object
    {

        public Microsoft.Xna.Framework.Vector3 location;

        double lifeTime;

        double livedTime;

        bool pendingKill;

        List<Actor> children = new List<Actor>();

        List<Component> components = new List<Component>();

        #region Properties
        public bool PendingKill { get => pendingKill; set => pendingKill = value; }
        public double LivedTime { get => livedTime; set => livedTime = value; }
        public double LifeTime { get => lifeTime; set => lifeTime = value; }

        internal List<Actor> Children { get => children; set => children = value; }
        public List<Component> Components { get => components; set => components = value; }



        #endregion

        ///<param name="lifeTime">lifetime=0 for infinite</param>
        public Actor(GameHandler game, Vector3 location, double lifeTime) : base(game)
        {
            this.location = location;
            this.lifeTime = lifeTime;
        }

        ///<param name="lifeTime">lifetime=0 for infinite. It uses seconds</param>
        public Actor(GameHandler game, string Name, Vector3 location, double lifeTime) : base(game, Name)
        {
            this.location = location;
            this.lifeTime = lifeTime;
        }

        public virtual bool IsValid() { return PendingKill; }

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
                if (livedTime == lifeTime)
                {
                    this.Destroy();
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

        public virtual void Destroy()
        {
            PendingKill = true;
            Dispose();
        }
    }

}
