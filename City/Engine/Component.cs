using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace City.Engine.Components
{
    public class Component : Object
    {

        public readonly Actor owner = null;

        public Component(GameHandler game, Actor owner) : base(game)
        {
            this.owner = owner;
        }

        public Component(GameHandler game, string Name, Actor owner) : base(game, Name)
        {
            this.owner = owner;
        }
    }

    public class DrawableComponent : Component
    {
        public DrawableComponent(GameHandler game, Actor owner) : base(game, owner)
        {
        }

        public DrawableComponent(GameHandler game, string Name, Actor owner) : base(game, Name, owner)
        {
        }

        public virtual void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {

        }
    }

}
