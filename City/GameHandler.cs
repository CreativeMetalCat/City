using City.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using NoiseTest;

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

        SoundEffect beam;

        Actor mouseDisplayActor;
        Engine.Components.CameraComponent currentCamera;

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

            base.Initialize();


            truck = Content.Load<Model>("Models/truck");

            beam = Content.Load<SoundEffect>("Sounds/beams/beamstart5");


            mouseDisplayActor = new Engine.Actor(this, "mousedisplay", new Vector3(0, 0, 0), new Vector3(0, 0, 0), 0.0f);

            mouseDisplayActor.Components.Add(new Engine.Components.ImageDisplayComponent(this, mouseDisplayActor, "Textures/mouse/icons8-cursor-24"));
            mouseDisplayActor.Components.Add(new Engine.Components.MouseFollowComponent(this, mouseDisplayActor));
            mouseDisplayActor.Init();



            AddActor(new Player(this, "player", new Vector3(0, 0, 0), new Vector3(0, 0, 0), 0.0f));

            currentCamera = new Engine.Components.CameraComponent(this, GetActorByName("player"), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, -5));
            GetActorByName("player").Components.Add(currentCamera);
            GetActorByName("player").Components.Add(new Engine.Components.BasicMovementComponent(this, GetActorByName("player")));

            GetActorByName("player").Init();

            OpenSimplexNoise oSimplexNoise = new OpenSimplexNoise();
            for (int x = 0; x < 50; x++)
            {
                for (int y = 0; y < 50; y++)
                {
                    double fNoise = oSimplexNoise.Evaluate(x, y);

                    if (fNoise < -0.3)
                    {
                        int copyId = AddActor(new GroundBaseActor(this, "water", false, new Vector3(x * 32, y * 32, 0), new Vector3(0, 0, 0), 0.0f));
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
                    else if (fNoise >= 0)
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
                    else if (fNoise >= -0.3 && fNoise < 0)
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
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                camPosition.X += 0.1f;
                camTarget.X += 0.1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                camPosition.Y -= 0.1f;
                camTarget.Y -= 0.1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                camPosition.Y += 0.1f;
                camTarget.Y += 0.1f;
            }
            mouseDisplayActor.HandleInput(Keyboard.GetState().GetPressedKeys());
            mouseDisplayActor.Update(gameTime);


        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            // TODO: Add your drawing code here

            spriteBatch.Begin();


            foreach (var actor in actors)
            {
                actor.Draw(currentCamera.ViewMatrix, currentCamera.ProjectionMatrix);
                actor.Draw(spriteBatch);
            }
            mouseDisplayActor.Draw(spriteBatch);
            mouseDisplayActor.Draw(currentCamera.ViewMatrix, currentCamera.ProjectionMatrix);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
