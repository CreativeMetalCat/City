using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace City.Engine.Components
{
    public class ImageDisplayComponent : DrawableComponent
    {
        public Vector3 location;

        protected string textureName;

        Microsoft.Xna.Framework.Graphics.Texture2D texture;

        public Texture2D Texture { get => texture; set => texture = value; }

        public ImageDisplayComponent(GameHandler game, Actor owner, string textureName) : base(game, owner)
        {
            this.textureName = textureName;
        }

        public ImageDisplayComponent(GameHandler game, string Name, Actor owner, string textureName) : base(game, Name, owner)
        {
            this.textureName = textureName;
        }

        public override void Init()
        {
            base.Init();
            this.texture = Game.Content.Load<Texture2D>(textureName);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (spriteBatch != null && !this.texture.IsDisposed && Visible)
            {
                spriteBatch.Draw(texture, new Vector2(this.location.X + this.owner.location.X, this.location.Y + this.owner.location.Y));
            }
        }
        public override void Dispose()
        {
            //texture.Dispose();
        }
    }
}
