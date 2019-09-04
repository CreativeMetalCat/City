using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

        /// <summary>
        /// If component is supposed to have location
        /// </summary>
        /// <returns>Returs location+ownerlocation or  throws System.NotImplementedException </returns>
        public virtual Vector3 GetWorldLocation()
        {
            throw new System.NotImplementedException();
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

        /// <summary>
        /// This functions exist because models do not require spite batch
        /// </summary>
        public virtual void Draw(Matrix viewMatrix, Matrix projectionMatrix)
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
                if (key == Keys.OemPlus)
                {
                    owner.location.Z += 1;
                }

                if (key == Keys.OemMinus)
                {
                    owner.location.Z -= 1;
                }
            }
        }

        public override void Dispose()
        {

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
        public override void Dispose()
        {

        }
    }


    public class StaticMeshComponent : DrawableComponent
    {
        public Microsoft.Xna.Framework.Graphics.Model model;

        public Matrix transofrmMatrix;

        public Vector3 location;

        public Vector3 rotation;

        protected string modelName;

        public StaticMeshComponent(GameHandler game, Actor owner, string modelName, Vector3 location, Vector3 rotation) : base(game, owner)
        {
            this.modelName = modelName;
            this.location = location;
            this.rotation = rotation;
        }

        public StaticMeshComponent(GameHandler game, string Name, Actor owner, string modelName, Vector3 location, Vector3 rotation) : base(game, Name, owner)
        {
            this.modelName = modelName;
            this.location = location;
            this.rotation = rotation;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Init()
        {
            model = owner.Game.Content.Load<Model>(modelName);
        }
        public override void Draw(Matrix viewMatrix, Matrix projectionMatrix)
        {
            base.Draw(viewMatrix, projectionMatrix);




            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    //effect.EnableDefaultLighting();
                    effect.AmbientLightColor = new Vector3(1f, 0, 0);
                    effect.View = viewMatrix;
                    effect.World = Matrix.CreateTranslation(location - owner.location) * Matrix.CreateRotationX(rotation.X + owner.rotation.X) * Matrix.CreateRotationY(rotation.Y + owner.rotation.Y) * Matrix.CreateRotationZ(rotation.Z + owner.rotation.Z);
                    effect.Projection = projectionMatrix;
                    effect.Alpha = 1;
                }
                mesh.Draw();
            }
        }

    }

    /// <summary>
    /// Handles loading of the script
    /// 
    /// </summary>
    public class ScriptComponent : Component
    {
        string fileName;
        System.Reflection.Assembly assembly;
        /// <summary>
        /// Main class in the script(the one that must contain functions that will be called)
        /// </summary>
        System.Type program;


        public ScriptComponent(GameHandler game, string fileName, Actor owner) : base(game, owner)
        {
            this.fileName = fileName;
        }

        public ScriptComponent(GameHandler game, string Name, string fileName, Actor owner) : base(game, Name, owner)
        {
            this.fileName = fileName;
        }

        public override void Init()
        {
            base.Init();
            Microsoft.CSharp.CSharpCodeProvider provider = new Microsoft.CSharp.CSharpCodeProvider();
            System.CodeDom.Compiler.CompilerParameters parameters = new System.CodeDom.Compiler.CompilerParameters();

            // Reference to System.Drawing library
            parameters.ReferencedAssemblies.Add("System.dll");
            // True - memory generation, false - external file generation
            parameters.GenerateInMemory = true;
            // True - exe file generation, false - dll file generation
            parameters.GenerateExecutable = true;

            string code = System.IO.File.ReadAllText(fileName);

            System.CodeDom.Compiler.CompilerResults results = provider.CompileAssemblyFromSource(parameters, code);

            if (results.Errors.HasErrors)
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();

                foreach (System.CodeDom.Compiler.CompilerError error in results.Errors)
                {
                    sb.AppendLine(string.Format("Error ({0}): {1}", error.ErrorNumber, error.ErrorText));
                }

                throw new System.InvalidOperationException(sb.ToString());
            }
            assembly = results.CompiledAssembly;
            program = assembly.GetType("Sript");


        }

        public virtual System.Reflection.MethodInfo GetMethod(string name)
        {
            return program.GetMethod(name);
        }
    }


}
