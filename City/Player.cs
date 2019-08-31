using Microsoft.Xna.Framework;
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

            spawnPowerSource = Game.soundPlayer.LoadSound(Game.GetContentDirectory() + "/Sounds/ui/gameplay/wire-connect-pole.wav", FMOD.MODE._2D);

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

            if (!Game._desktop.IsMouseOverGUI)
            {
                int buildingId = 0; ;
                for (int i = 0; i < (Game._desktop.GetChild(0) as Myra.Graphics2D.UI.Grid).ChildrenCount; i++)
                {
                    if ((Game._desktop.GetChild(0) as Myra.Graphics2D.UI.Grid).GetChild(i).Id == "buildingType")
                    {
                        buildingId = ((Game._desktop.GetChild(0) as Myra.Graphics2D.UI.Grid).GetChild(i) as Myra.Graphics2D.UI.ComboBox).SelectedIndex.GetValueOrDefault();
                    }

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
                        if (buildingId == 0)
                        {
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
                                if (actor.collision.Intersects(new Rectangle((int)gridPos.X, (int)gridPos.Y, 32, 32))) { canSpawn &= false; break; }

                            }
                            if (canSpawn)
                            {//new Vector3(Microsoft.Xna.Framework.Input.Mouse.GetState().X, Microsoft.Xna.Framework.Input.Mouse.GetState().Y, 0)
                                Game.AddActor(new PowerConsumingBuilding(Game, "consumer", new Rectangle((int)gridPos.X, (int)gridPos.Y, 32, 32), new Rectangle((int)gridPos.X - 32, (int)gridPos.Y - 32, 64, 64), new Vector3(gridPos.X, gridPos.Y, 0), new Vector3(0, 0, 0), 6.0f));
                                Game.actors[Game.actors.Count - 1].Components.Add(new Engine.Components.ImageDisplayComponent(Game, Game.actors[Game.actors.Count - 1], "Textures/grassy_bricks"));
                                Game.actors[Game.actors.Count - 1].Init();

                                FMOD.VECTOR pos = new FMOD.VECTOR();
                                pos.x = Game.actors[Game.actors.Count - 1].location.X;
                                pos.y = Game.actors[Game.actors.Count - 1].location.Y;
                                pos.z = Game.actors[Game.actors.Count - 1].location.Z;

                                FMOD.VECTOR vel = new FMOD.VECTOR();

                                FMOD.VECTOR alt_pane_pos = new FMOD.VECTOR();

                                Game.soundPlayer.PlaySound(spawnSuccess, null, false).set3DAttributes(ref pos, ref vel, ref alt_pane_pos);
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
                        else if (buildingId == 1)
                        {
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
                                if (actor.collision.Intersects(new Rectangle((int)gridPos.X, (int)gridPos.Y, 64, 64))) { canSpawn &= false; break; }
                            }
                            if (canSpawn)
                            {


                                Game.AddActor(BuildingSystem.Factory.CreateGeneratorBuilding(Game, new Vector3(gridPos.X, gridPos.Y, 0), new Vector3(0, 0, 0)));

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
                        else
                        {
                            throw new System.Exception("buildingId is bigger than amount of existing types. Value is " + buildingId);
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

                        foreach (Building building in Game.actors.OfType<Building>())
                        {
                            if(building.collision.Contains((int)(Microsoft.Xna.Framework.Input.Mouse.GetState().Position.X / Game.gridSize.X) * Game.gridSize.X, (int)(Microsoft.Xna.Framework.Input.Mouse.GetState().Position.Y / Game.gridSize.Y) * Game.gridSize.Y))
                            {
                                building.Dispose();
                                Game.actors.Remove(building);
                                break;
                            }
                        }
                    }
                }
                if (Microsoft.Xna.Framework.Input.Mouse.GetState().RightButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
                {
                    rightMouseButtonPressed = false;
                }
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
