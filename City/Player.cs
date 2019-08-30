using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System.Linq;
namespace City
{
    class Player : Engine.Actor
    {

        FMOD.Sound spawnFailSound;
        FMOD.Sound spawnPowerSource;
        FMOD.Sound spawnSuccess;

        /// <summary>
        /// If game has more that one player
        /// </summary>
        public readonly int playerId = 0;

        public Engine.Components.CameraComponent playerCamera;

        bool leftMouseButtonPressed;

        bool rightMouseButtonPressed;

        public Player(GameHandler game, Vector3 location, Vector3 rotation, double lifeTime) : base(game, location, rotation, lifeTime)
        {
        }

        public Player(GameHandler game, string Name, Vector3 location, Vector3 rotation, double lifeTime) : base(game, Name, location, rotation, lifeTime)
        {
        }

        public override void Init()
        {
            System.Diagnostics.Debug.WriteLine(Game.GetContentDirectory() + "/Sounds/ui/gameplay/cannot-build.wav");

            spawnFailSound = Game.soundPlayer.LoadSound(Game.GetContentDirectory() + "/Sounds/ui/gameplay/cannot-build.wav", FMOD.MODE._3D_LINEARROLLOFF);

            spawnPowerSource = Game.soundPlayer.LoadSound(Game.GetContentDirectory() + "/Sounds/ui/gameplay/wire-connect-pole.wav", FMOD.MODE._3D_LINEARROLLOFF);

            spawnSuccess = Game.soundPlayer.LoadSound(Game.GetContentDirectory() + "/Sounds/ui/gameplay/build-large.wav", FMOD.MODE._3D_LINEARROLLOFF);

            playerCamera = new Engine.Components.CameraComponent(Game, this, new Vector3(0, 0, 0), new Vector3(0, 0, 0));

            components.Add(playerCamera);

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
            if (Microsoft.Xna.Framework.Input.Mouse.GetState().LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                if (!leftMouseButtonPressed)
                {
                    leftMouseButtonPressed = true;

                    Vector3 gridPos = new Vector3();

                    gridPos.X = (int)(Microsoft.Xna.Framework.Input.Mouse.GetState().Position.X / Game.gridSize.X) * Game.gridSize.X;
                    gridPos.Y = (int)(Microsoft.Xna.Framework.Input.Mouse.GetState().Position.Y / Game.gridSize.Y) * Game.gridSize.Y;
                    gridPos.Z = 0;

                    bool canSpawn = true;
                    foreach (var actor in Game.actors.OfType<GroundBaseActor>())
                    {
                        if (actor.location.X == gridPos.X && actor.location.Y == gridPos.Y)
                        {
                            if (!actor.canBeBuiltOn)
                            {
                                canSpawn &= false; break;
                            }
                        }
                    }
                    foreach (var actor in Game.actors.OfType<Building>())
                    {
                        if (actor.location.X == gridPos.X && actor.location.Y == gridPos.Y) { canSpawn &= false; break; }
                    }

                    if (canSpawn)
                    {//new Vector3(Microsoft.Xna.Framework.Input.Mouse.GetState().X, Microsoft.Xna.Framework.Input.Mouse.GetState().Y, 0)
                        Game.AddActor(new PowerConsumingBuilding(Game, "consumer", new Rectangle((int)gridPos.X - 32, (int)gridPos.Y - 32, 64, 64), new Rectangle((int)gridPos.X - 32, (int)gridPos.Y - 32, 64, 64), new Vector3(gridPos.X, gridPos.Y, 0), new Vector3(0, 0, 0), 6.0f));
                        Game.actors[Game.actors.Count - 1].Components.Add(new Engine.Components.ImageDisplayComponent(Game, Game.actors[Game.actors.Count - 1], "Textures/grassy_bricks"));
                        Game.actors[Game.actors.Count - 1].Init();

                        FMOD.VECTOR pos = new FMOD.VECTOR();
                        pos.x = Game.actors[Game.actors.Count - 1].location.X;
                        pos.y = Game.actors[Game.actors.Count - 1].location.Y;
                        pos.z = Game.actors[Game.actors.Count - 1].location.Z;

                        FMOD.VECTOR vel = new FMOD.VECTOR();

                        FMOD.VECTOR alt_pane_pos = new FMOD.VECTOR();

                        Game.soundPlayer.PlaySound(spawnSuccess, null, false).set3DAttributes(ref pos,ref vel,ref alt_pane_pos);
                    }
                    else
                    {
                        FMOD.VECTOR pos = new FMOD.VECTOR();
                        pos.x = gridPos.X;
                        pos.y = gridPos.Y;
                        pos.z = 0;

                        FMOD.VECTOR vel = new FMOD.VECTOR();

                        FMOD.VECTOR alt_pane_pos = new FMOD.VECTOR();

                        Game.soundPlayer.PlaySound(spawnFailSound, null, false).set3DAttributes(ref pos, ref vel, ref alt_pane_pos);
                    }

                }
            }
            if (Microsoft.Xna.Framework.Input.Mouse.GetState().LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
            {
                leftMouseButtonPressed = false;
            }

            if (Microsoft.Xna.Framework.Input.Mouse.GetState().RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                if (!rightMouseButtonPressed)
                {
                    rightMouseButtonPressed = true;

                    Vector3 gridPos = new Vector3();

                    gridPos.X = (int)(Microsoft.Xna.Framework.Input.Mouse.GetState().Position.X / Game.gridSize.X) * Game.gridSize.X;
                    gridPos.Y = (int)(Microsoft.Xna.Framework.Input.Mouse.GetState().Position.Y / Game.gridSize.Y) * Game.gridSize.Y;
                    gridPos.Z = 0;

                    bool canSpawn = true;
                    foreach (var actor in Game.actors.OfType<GroundBaseActor>())
                    {

                        if (actor.location.X == gridPos.X && actor.location.Y == gridPos.Y)
                        {
                            if (!actor.canBeBuiltOn)
                            {
                                canSpawn &= false; break;
                            }
                        }
                    }
                    foreach (var actor in Game.actors.OfType<Building>())
                    {
                        if (actor.location.X == gridPos.X && actor.location.Y == gridPos.Y) { canSpawn &= false; break; }
                    }
                    if (canSpawn)
                    {


                        Game.AddActor(new PowerSourceBuilding(Game, "generator", new Rectangle((int)gridPos.X - 32, (int)gridPos.Y - 32, 32, 32), new Rectangle((int)gridPos.X - 32, (int)gridPos.Y - 32, 96, 96), new Vector3(gridPos.X, gridPos.Y, 0), new Vector3(0, 0, 0), 5.0f));
                        Game.actors[Game.actors.Count - 1].Components.Add(new Engine.Components.ImageDisplayComponent(Game, Game.actors[Game.actors.Count - 1], "Textures/light_source"));
                        Game.actors[Game.actors.Count - 1].Init();

                        FMOD.VECTOR pos = new FMOD.VECTOR();
                        pos.x = Game.actors[Game.actors.Count - 1].location.X;
                        pos.y = Game.actors[Game.actors.Count - 1].location.Y;
                        pos.z = Game.actors[Game.actors.Count - 1].location.Z;

                        FMOD.VECTOR vel = new FMOD.VECTOR();

                        FMOD.VECTOR alt_pane_pos = new FMOD.VECTOR();

                        Game.soundPlayer.PlaySound(spawnPowerSource, null, false).set3DAttributes(ref pos, ref vel, ref alt_pane_pos);
                    }
                    else
                    {
                        FMOD.VECTOR pos = new FMOD.VECTOR();
                        pos.x = gridPos.X;
                        pos.y = gridPos.Y;
                        pos.z = 0;

                        FMOD.VECTOR vel = new FMOD.VECTOR();

                        FMOD.VECTOR alt_pane_pos = new FMOD.VECTOR();

                        Game.soundPlayer.PlaySound(spawnFailSound, null, false).set3DAttributes(ref pos, ref vel, ref alt_pane_pos);
                    }

                }
            }
            if (Microsoft.Xna.Framework.Input.Mouse.GetState().RightButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
            {
                rightMouseButtonPressed = false;
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

        public override void Dispose()
        {
            PendingKill = true;
            foreach (var item in components)
            {
                item.Dispose();
            }
            foreach (var child in Children)
            {
                child.Dispose();
            }
            spawnFailSound.release();
            spawnPowerSource.release();
            spawnSuccess.release();

            System.GC.SuppressFinalize(this);
        }
    }
}
