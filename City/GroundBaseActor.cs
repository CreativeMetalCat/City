using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace City
{
    /// <summary>
    /// Class that defines what kind of ground it is, can player build there, resources on it etc.
    /// </summary>
    class GroundBaseActor : Engine.Actor
    {
        public readonly bool canBeBuiltOn;

        public GroundBaseActor(GameHandler game, bool canBeBuiltOn, Vector3 location, Vector3 rotation, double lifeTime) : base(game, location, rotation, lifeTime)
        {
            this.canBeBuiltOn = canBeBuiltOn;
        }

        public GroundBaseActor(GameHandler game, string Name, bool canBeBuiltOn, Vector3 location, Vector3 rotation, double lifeTime) : base(game, Name, location, rotation, lifeTime)
        {
            this.canBeBuiltOn = canBeBuiltOn;
        }
    }
}
