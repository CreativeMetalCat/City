using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using City.Engine;
using Microsoft.Xna.Framework;

namespace City
{
    class Building : Engine.Actor
    {
        /// <summary>
        /// Primitive solution to prevent spawning buildings on each other
        /// </summary>
        Rectangle collision;

        public Building(GameHandler game, Vector3 location, Vector3 rotation, double lifeTime) : base(game, location, rotation, lifeTime)
        {

        }

        public Building(GameHandler game, string Name, Vector3 location, Vector3 rotation, double lifeTime) : base(game, Name, location, rotation, lifeTime)
        {
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Game != null)
            {
                
            }
            else
            {
                throw new NullReferenceException("Attempt to use null Game Handler");
            }
        }
    }
}
