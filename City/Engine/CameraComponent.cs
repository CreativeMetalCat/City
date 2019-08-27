using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace City.Engine.Components
{
    class CameraComponent : Component
    {

        public Vector3 camTarget;
        public Vector3 camPosition;

        private Matrix viewMatrix;

        private Matrix projectionMatrix;

        public CameraComponent(GameHandler game, Actor owner, Vector3 camTarget, Vector3 camPosition) : base(game, owner)
        {
            this.camTarget = camTarget;
            this.camPosition = camPosition;

            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                               MathHelper.ToRadians(45f), owner.Game.graphics.
                               GraphicsDevice.Viewport.AspectRatio,
                1f, 1000f);
        }


        public CameraComponent(GameHandler game, string Name, Actor owner, Vector3 camTarget, Vector3 camPosition) : base(game, Name, owner)
        {
            this.camTarget = camTarget;
            this.camPosition = camPosition;

            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                               MathHelper.ToRadians(45f), owner.Game.graphics.
                               GraphicsDevice.Viewport.AspectRatio,
                1f, 1000f);
        }

        public Matrix ViewMatrix { get => viewMatrix; }
        public Matrix ProjectionMatrix { get => projectionMatrix;}

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);


            viewMatrix = Matrix.CreateLookAt(camPosition + owner.location, camTarget,
                       Vector3.Up);
        }
    }
}
