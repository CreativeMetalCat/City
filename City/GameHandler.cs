using City.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Myra;
using Myra.Graphics2D.UI;
using NoiseTest;
using System.Collections.Generic;
using Box2DX.Common;
using Box2DX.Collision;
using Box2DX.Dynamics;




namespace City
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameHandler : Game
    {
        public readonly GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D placeholder;

        public Engine.Sound.SoundPlayer soundPlayer;

        public Myra.Graphics2D.UI.Desktop _desktop;

        Actor mouseDisplayActor;
        Engine.Components.CameraComponent currentCamera;

        public float physicsScaleX = 64;
        public float physicsScaleY = 64;

        public Engine.Physics.PhysicsWorld physicsWorld;


        //Camera
        Vector3 camTarget;
        Vector3 camPosition;

        Model truck;

        public readonly Vector2 gridSize;
        /// <summary>
        /// Containes all actors of scene. You can get specific by using GetActorByName or add by using AddActor(name)
        /// Needs to be accesseble from actor class itself
        /// </summary>
        public readonly List<Actor> actors = new List<Actor>();


        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        public static extern System.IntPtr LoadLibrary(string dllToLoad);

        public string GetContentDirectory()
        {
            return System.Environment.CurrentDirectory + "/" + Content.RootDirectory;
        }

        public Actor GetActorByName(string name, bool useId = true)
        {


            foreach (var actor in actors)
            {
                if (useId && actor.Id != 0)
                {
                    if ((actor.Name + actor.Id.ToString()) == name) { return actor; }
                }
                else
                {
                    if (actor.Name == name) { return actor; }
                }
            }
            return null;
        }

        /// <summary>
        /// Adds new actor.
        /// If there is already actor with this name increments last number of name to avoid having same names
        /// </summary>
        /// <param name="actor"> Actor to add</param>
        /// <returns>Actor.Id or 0 if no other actors use this name</returns>
        public int AddActor(Actor actor)
        {

            int id = 0;
            if (actors.Count != 0)
            {
                foreach (var item in actors)
                {
                    if (item.Name == actor.Name)
                    {
                        id++;
                    }
                }
                if (id != 0)
                {
                    actor.Id = id;
                    actors.Add(actor);
                    return id;
                }
                else
                {
                    actors.Add(actor);
                    return 0;
                }
            }
            else
            {
                actors.Add(actor);
                return 0;
            }
        }

        /// <summary>
        /// Adds new actor.
        /// If there is already actor with this name returns false
        /// </summary>
        /// <param name="actor"> Actor to add</param>
        public bool AddActorUniqueName(Actor actor)
        {
            if (actors.Count != 0)
            {
                foreach (var item in actors)
                {
                    if (item.Name == actor.Name)
                    {
                        return false;
                    }
                }

                actors.Add(actor);
                return true;

            }
            else
            {
                actors.Add(actor);
                return true;
            }
        }

        public GameHandler()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            gridSize.X = 32;
            gridSize.Y = 32;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            IsMouseVisible = true;
            #region phys

            //world bounds
            // if bodies reach the end of the world, but it will be slower or stops completly.
            Box2DX.Collision.AABB worldAABB = new Box2DX.Collision.AABB();
            worldAABB.LowerBound.Set(-100.0f, -100.0f);
            worldAABB.UpperBound.Set(100.0f, 100.0f);

            physicsWorld = new Engine.Physics.PhysicsWorld(new Vector2(0, 9.8f).ToBox2DVector(), worldAABB);

            // Define the ground body.
            BodyDef groundBodyDef = new BodyDef();
            groundBodyDef.Position.Set(0.0f / physicsScaleX, 256.0f / physicsScaleY);

            // Call the body factory which creates the ground box shape.
            // The body is also added to the world.
            Body groundBody = physicsWorld.CreateBody(groundBodyDef);

            // Define the ground box shape.
            PolygonDef groundShapeDef = new PolygonDef();

            // The extents are the half-widths of the box.
            groundShapeDef.SetAsBox(1000.0f / physicsScaleX, 10.0f / physicsScaleY);

            // Add the ground shape to the ground body.
            groundBody.CreateShape(groundShapeDef);

            


            #endregion

            //FMOD.VERSION.dll
            base.Initialize();
            System.Diagnostics.Debug.WriteLine(System.IO.Path.GetFullPath("FMOD/64/fmod.dll"));
            if (System.Environment.Is64BitProcess)
                LoadLibrary(System.IO.Path.GetFullPath("FMOD/64/fmod.dll"));
            else
                LoadLibrary(System.IO.Path.GetFullPath("FMOD/32/fmod.dll"));

            truck = Content.Load<Model>("Models/truck");

            soundPlayer = new Engine.Sound.SoundPlayer(this);
            soundPlayer.Init();


            mouseDisplayActor = new Engine.Actor(this, "mousedisplay", new Vector3(0, 0, 0), new Vector3(0, 0, 0), 0.0f);

            mouseDisplayActor.Components.Add(new Engine.Components.ImageDisplayComponent(this, mouseDisplayActor, "Textures/mouse/icons8-cursor-24"));
            mouseDisplayActor.Components.Add(new Engine.Components.MouseFollowComponent(this, mouseDisplayActor));
            mouseDisplayActor.Init();

            OpenSimplexNoise oSimplexNoise = new OpenSimplexNoise();
            for (int x = 0; x < 50; x++)
            {
                for (int y = 0; y < 50; y++)
                {
                    double fNoise = oSimplexNoise.Evaluate(x / 2, y / 2);

                    if (fNoise < -0.1)
                    {
                        int copyId = AddActor(new GroundBaseActor(this, "water", true, new Vector3(x * 32, y * 32, 0), new Vector3(0, 0, 0), 0.0f));
                        if (copyId != 0)
                        {
                            GetActorByName("water" + copyId).Components.Add(new Engine.Components.ImageDisplayComponent(this, GetActorByName("water" + copyId), "Textures/nature/water1"));
                            GetActorByName("water" + copyId).Init();
                        }
                        else
                        {
                            GetActorByName("water").Components.Add(new Engine.Components.ImageDisplayComponent(this, GetActorByName("water"), "Textures/nature/water1"));
                            GetActorByName("water").Init();
                        }

                    }
                    else if (fNoise >= 0 && fNoise < 0.5)
                    {
                        int copyId = AddActor(new GroundBaseActor(this, "grass", true, new Vector3(x * 32, y * 32, 0), new Vector3(0, 0, 0), 0.0f));
                        if (copyId != 0)
                        {
                            GetActorByName("grass" + copyId).Components.Add(new Engine.Components.ImageDisplayComponent(this, GetActorByName("grass" + copyId), "Textures/nature/grass1"));
                            GetActorByName("grass" + copyId).Init();
                        }
                        else
                        {
                            GetActorByName("grass").Components.Add(new Engine.Components.ImageDisplayComponent(this, GetActorByName("grass"), "Textures/nature/grass1"));
                            GetActorByName("grass").Init();
                        }

                    }
                    else if (fNoise >= 0.5)
                    {
                        int copyId = AddActor(new GroundBaseActor(this, "snow", true, new Vector3(x * 32, y * 32, 0), new Vector3(0, 0, 0), 0.0f));
                        if (copyId != 0)
                        {
                            GetActorByName("snow" + copyId).Components.Add(new Engine.Components.ImageDisplayComponent(this, GetActorByName("snow" + copyId), "Textures/nature/snow1"));
                            GetActorByName("snow" + copyId).Init();
                        }
                        else
                        {
                            GetActorByName("snow").Components.Add(new Engine.Components.ImageDisplayComponent(this, GetActorByName("snow"), "Textures/nature/snow1"));
                            GetActorByName("snow").Init();
                        }
                    }
                    else if (fNoise >= -0.1 && fNoise < 0)
                    {
                        int copyId = AddActor(new GroundBaseActor(this, "sand", true, new Vector3(x * 32, y * 32, 0), new Vector3(0, 0, 0), 0.0f));
                        if (copyId != 0)
                        {
                            GetActorByName("sand" + copyId).Components.Add(new Engine.Components.ImageDisplayComponent(this, GetActorByName("sand" + copyId), "Textures/nature/sand1"));
                            GetActorByName("sand" + copyId).Init();
                        }
                        else
                        {
                            GetActorByName("sand").Components.Add(new Engine.Components.ImageDisplayComponent(this, GetActorByName("sand"), "Textures/nature/sand1"));
                            GetActorByName("sand").Init();
                        }
                    }
                }
            }

            AddActor(new Actor(this, "ConcertHallReverb", new Vector3(0, 0, 0), new Vector3(0, 0, 0), 0.0f));
            GetActorByName("ConcertHallReverb").Components.Add(new Engine.Components.ReverbComponent(this, GetActorByName("ConcertHallReverb"), new Vector3(0, 0, 0), FMOD.PRESET.CONCERTHALL(), 32, 32));
            GetActorByName("ConcertHallReverb").Init();

            AddActor(new Actor(this, "QUARRYReverb", new Vector3(128, 0, 0), new Vector3(0, 0, 0), 0.0f));
            GetActorByName("QUARRYReverb").Components.Add(new Engine.Components.ReverbComponent(this, GetActorByName("QUARRYReverb"), new Vector3(0, 0, 0), FMOD.PRESET.QUARRY(), 32, 32));
            GetActorByName("QUARRYReverb").Init();

            AddActor(new Actor(this, "UNDERWATEReverb", new Vector3(0, 128, 0), new Vector3(0, 0, 0), 0.0f));
            GetActorByName("UNDERWATEReverb").Components.Add(new Engine.Components.ReverbComponent(this, GetActorByName("UNDERWATEReverb"), new Vector3(0, 0, 0), FMOD.PRESET.UNDERWATER(), 32, 32));
            GetActorByName("UNDERWATEReverb").Init();

            AddActor(new Player(this, "player", new Vector3(0, 0, 0), new Vector3(0, 0, 0), 0.0f));
            // GetActorByName("player").Components.Add(new Engine.Components.BasicMovementComponent(this, GetActorByName("player")));
            GetActorByName("player").Components.Add(new Engine.Components.ImageDisplayComponent(this, GetActorByName("player"), "Textures/solid"));

           

            GetActorByName("player").Init();
            currentCamera = (GetActorByName("player") as Player).playerCamera;

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            placeholder = this.Content.Load<Texture2D>("Textures/picture");

            MyraEnvironment.Game = this;

            var grid = new Grid
            {
                RowSpacing = 8,
                ColumnSpacing = 8
            };

            grid.ColumnsProportions.Add(new Grid.Proportion(Grid.ProportionType.Auto));
            grid.ColumnsProportions.Add(new Grid.Proportion(Grid.ProportionType.Auto));
            grid.RowsProportions.Add(new Grid.Proportion(Grid.ProportionType.Auto));
            grid.RowsProportions.Add(new Grid.Proportion(Grid.ProportionType.Auto));

            // TextBlock
            var helloWorld = new TextBlock
            {
                Id = "label",
                Text = "Hello, World!"
            };
            grid.Widgets.Add(helloWorld);

            // ComboBox
            var combo = new ComboBox
            {
                GridColumn = 1,
                GridRow = 0,
                Id = "buildingType"
            };

            combo.Items.Add(new ListItem("Dev.PowerSourceBuilding", Microsoft.Xna.Framework.Color.White));
            combo.Items.Add(new ListItem("Dev.PowerConsumeBuilding", Microsoft.Xna.Framework.Color.White));
            grid.Widgets.Add(combo);


            // Add it to the desktop
            _desktop = new Desktop();
            _desktop.Widgets.Add(grid);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            physicsWorld.Step(gameTime, 5, 5);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
            for (int i = 0; i < actors.Count; i++)
            {
                if (!actors[i].IsValid())
                {
                    actors.RemoveAt(i);
                }
            }
            for (int i = 0; i < actors.Count; i++)
            {
                actors[i].HandleInput(Keyboard.GetState().GetPressedKeys());
                actors[i].Update(gameTime);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.OemPlus))
            {
                camPosition.Z += 0.1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.OemMinus))
            {
                camPosition.Z -= 0.1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                camPosition.X -= 0.1f;
                camTarget.X -= 0.1f;
                //body.ApplyImpulse(new Vec2(-10.0f/physicsScaleX, 0.0f), body.GetLocalCenter());
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                camPosition.X += 0.1f;
                camTarget.X += 0.1f;
                //body.ApplyImpulse(new Vec2(10.0f / physicsScaleX, 0.0f), body.GetLocalCenter());
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                camPosition.Y -= 0.1f;
                camTarget.Y -= 0.1f;
                //body.ApplyImpulse(new Vec2(0.0f, -10.0f / physicsScaleY), body.GetLocalCenter());
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                camPosition.Y += 0.1f;
                camTarget.Y += 0.1f;
            }
            mouseDisplayActor.HandleInput(Keyboard.GetState().GetPressedKeys());
            mouseDisplayActor.Update(gameTime);

            //GetActorByName("player").location = new Vector3(body.GetPosition().X * physicsScaleX, body.GetPosition().Y * physicsScaleY, 0);
            //System.Diagnostics.Debug.WriteLine(new Vector3(body.GetPosition().X, body.GetPosition().Y, 0));
            GetActorByName("player").GetMatrix();
            soundPlayer.Set3DListenerAttributes((GetActorByName("player") as Player).playerId, GetActorByName("player").location, new Vector3(0, 0, 0), GetActorByName("player").GetMatrix().Forward, GetActorByName("player").GetMatrix().Up);
            soundPlayer.Update(gameTime);


        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.White);
            // TODO: Add your drawing code here

            spriteBatch.Begin();


            foreach (var actor in actors)
            {
                actor.Draw(currentCamera.ViewMatrix, currentCamera.ProjectionMatrix);
                actor.Draw(spriteBatch);
            }
            if (!IsMouseVisible)
            {
                mouseDisplayActor.Draw(spriteBatch);
                mouseDisplayActor.Draw(currentCamera.ViewMatrix, currentCamera.ProjectionMatrix);
            }
            spriteBatch.End();

            _desktop.Bounds = new Rectangle(0, 0, GraphicsDevice.PresentationParameters.BackBufferWidth,
                                                    GraphicsDevice.PresentationParameters.BackBufferHeight);
            _desktop.Render();
            base.Draw(gameTime);
        }


    }
}
