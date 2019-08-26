using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace City.Engine.Components
{
    public class Component : Object
    {

        public readonly Actor owner = null;

        public Component(GameHandler game, Actor owner) : base(game)
        {
            this.owner = owner;
            if (owner == null) { throw new Exceptions.NullOnwerException("Owner of component is  null"); }
        }

        public Component(GameHandler game, string Name, Actor owner) : base(game, Name)
        {
            this.owner = owner;

            if (owner == null) { throw new Exceptions.NullOnwerException("Owner of component " + Name + " was null"); }
        }
    }

    public class DrawableComponent : Component
    {
        bool visible = true;

        public DrawableComponent(GameHandler game, Actor owner) : base(game, owner)
        {
        }

        public DrawableComponent(GameHandler game, string Name, Actor owner) : base(game, Name, owner)
        {
        }

        public bool Visible { get => visible; set => visible = value; }

        public virtual void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {

        }
    }

    public class BasicMovementComponent : Component
    {
        public BasicMovementComponent(GameHandler game, Actor owner) : base(game, owner)
        {
        }

        public BasicMovementComponent(GameHandler game, string Name, Actor owner) : base(game, Name, owner)
        {
        }

        public override void HandleInput(Keys[] keys)
        {
            base.HandleInput(keys);
            foreach (var key in keys)
            {
                if (key == Keys.Up)
                {
                    owner.location.Y -= 1;
                }

                if (key == Keys.Down)
                {
                    owner.location.Y += 1;
                }

                if (key == Keys.Left)
                {
                    owner.location.X -= 1;
                }

                if (key == Keys.Right)
                {
                    owner.location.X += 1;
                }
            }
        }
    }
    /// <summary>
    /// Component that sets owner's position to the mouse's position
    /// </summary>
    public class MouseFollowComponent : Component
    {
        public MouseFollowComponent(GameHandler game, Actor owner) : base(game, owner)
        {
        }

        public MouseFollowComponent(GameHandler game, string Name, Actor owner) : base(game, Name, owner)
        {
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (owner.Game.GraphicsDevice.Viewport.Bounds.Contains(Microsoft.Xna.Framework.Input.Mouse.GetState().Position))
            {
                owner.location = new Vector3(Microsoft.Xna.Framework.Input.Mouse.GetState().Position.X, Microsoft.Xna.Framework.Input.Mouse.GetState().Position.Y, 0);
            }
        }
    }

}
