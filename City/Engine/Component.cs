using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace City.Engine
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
}
