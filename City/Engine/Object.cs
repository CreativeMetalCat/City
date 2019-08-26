using System;

namespace City.Engine
{

    ///<summary>Base of all bases of the engine</summary>
    public class Object : IDisposable
    {
        string name;
        readonly string DisplayName;

        GameHandler game;

        public GameHandler Game { get => game; set => game = value; }
        public string Name { get => name; set => name = value; }

        public Object(GameHandler game, string Name)
        {
            this.game = game;
            this.name = Name;
        }

        public Object(GameHandler game)
        {
            this.game = game;
            name = "nonameobject";
        }

        public virtual void Dispose()
        {
            throw new NotImplementedException();
        }

        public virtual void HandleInput(Microsoft.Xna.Framework.Input.Keys[] keys)
        {

        }

        /// <summary>
        /// Happens during loadContent, but can be called outside
        /// </summary>
        public virtual void Init()
        {

        }

        public virtual void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {

        }


    }

}
