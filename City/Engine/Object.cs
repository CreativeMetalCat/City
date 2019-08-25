using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace City.Engine
{

    ///<summary>Base of all bases of the engine</summary>
    public class Object : IDisposable
    {
        string Name;

        GameHandler game;

        public Object(GameHandler game, string Name)
        {
            this.game = game;
        }

        public Object(GameHandler game)
        {
            this.game = game;
            Name = "nonameobject";
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public virtual void HandleInput(Microsoft.Xna.Framework.Input.Keys key)
        {

        }

        public virtual void Init()
        {

        }

        public virtual void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {

        }


    }
    
}
