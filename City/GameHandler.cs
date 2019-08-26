using City.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace City
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameHandler : Game
    {
        readonly GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D placeholder;

        SoundEffect beam;

         Actor mouseDisplayActor;

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

           beam = Content.Load<SoundEffect>("Sounds/beams/beamstart5");

            mouseDisplayActor = new Engine.Actor(this, "mousedisplay", new Vector3(0, 0, 0), 0.0f);
            mouseDisplayActor.Components.Add(new Engine.Components.ImageDisplayComponent(this, mouseDisplayActor, "Textures/mouse/icons8-cursor-24"));
            //GetActorByName("actor1").Components.Add(new Engine.Components.BasicMovementComponent(this, GetActorByName("actor1")));
            mouseDisplayActor.Components.Add(new Engine.Components.MouseFollowComponent(this, mouseDisplayActor));
            mouseDisplayActor.Init();

            AddActor(new Player(this, "player", new Vector3(0, 0, 0), 0.0f));

            GetActorByName("player").Init();

            
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

            mouseDisplayActor.HandleInput(Keyboard.GetState().GetPressedKeys());
            mouseDisplayActor.Update(gameTime);

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            spriteBatch.Begin();
            foreach (var actor in actors)
            {
                actor.Draw(spriteBatch);
            }
            mouseDisplayActor.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
