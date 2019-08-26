using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace City
{
    class Player : Engine.Actor
    {
        public Player(GameHandler game, Vector3 location, double lifeTime) : base(game, location, lifeTime)
        {
        }

        public Player(GameHandler game, string Name, Vector3 location, double lifeTime) : base(game, Name, location, lifeTime)
        {
        }

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
    }
}
