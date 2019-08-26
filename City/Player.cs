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
            if(Microsoft.Xna.Framework.Input.Mouse.GetState().LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                Game.AddActor(new Engine.Actor(Game,"actor", new Vector3(Microsoft.Xna.Framework.Input.Mouse.GetState().X, Microsoft.Xna.Framework.Input.Mouse.GetState().Y, 0), 1.0f));
                Game.actors[Game.actors.Count-1].Components.Add(new Engine.Components.ImageDisplayComponent(Game, Game.actors[Game.actors.Count - 1], "Textures/grassy_bricks"));
                Game.actors[Game.actors.Count - 1].Init();
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
