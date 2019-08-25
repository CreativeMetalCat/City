using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using City.Engine.Components;

namespace City.Engine
{
    ///<summary>Base for everything on the scene/summary>
    public class Actor : Object
    {

        Microsoft.Xna.Framework.Vector3 location;

        TimeSpan lifeTime;

        TimeSpan livedTime;

        bool pendingKill;

        List<Actor> children = new List<Actor>();

        List<Component> components = new List<Component>();

        #region Properties
        public bool PendingKill { get => pendingKill; set => pendingKill = value; }
        public TimeSpan LivedTime { get => livedTime; set => livedTime = value; }
        public TimeSpan LifeTime { get => lifeTime; set => lifeTime = value; }
        public Vector3 Location { get => location; set => location = value; }
        internal List<Actor> Children { get => children; set => children = value; }
        public List<Component> Components { get => components; set => components = value; }



        #endregion

        ///<param name="lifeTime">lifetime=0 for infinite</param>
        public Actor(GameHandler game, Vector3 location, TimeSpan lifeTime) : base(game)
        {
            this.location = location;
            this.lifeTime = lifeTime;
        }

        ///<param name="lifeTime">lifetime=0 for infinite. It uses seconds</param>
        public Actor(GameHandler game, string Name, Vector3 location, TimeSpan lifeTime) : base(game, Name)
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

        public override void HandleInput(Microsoft.Xna.Framework.Input.Keys key)
        {
            foreach (var comp in components)
            {
                comp.HandleInput(key);
            }
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            foreach(var comp in components)
            {
                comp.Update(gameTime);
            }
            if (lifeTime.Seconds != 0.0f)
            {
                livedTime += gameTime.ElapsedGameTime;
                if (livedTime == lifeTime)
                {
                    this.Destroy();
                }
            }

        }

        public virtual void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            foreach(var comp in components)
            {
                if(comp is Components.DrawableComponent)
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
