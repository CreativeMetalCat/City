using Microsoft.Xna.Framework;

namespace City.BuildingSystem
{
    public class Factory
    {
        /// <summary>
        /// THIS BUILDING IS NOT FOR GAMEPLAY
        /// Creates building of class.
        /// </summary>
        /// <param name="Game"></param>
        /// <param name="location"></param>
        /// <param name="rotation"></param>
        /// <returns>Returns building of class rquires to be init/returns>
        public static PowerSourceBuilding CreateGeneratorBuilding(GameHandler Game, Vector3 location, Vector3 rotation)
        {
            PowerSourceBuilding building = new PowerSourceBuilding(Game, "generator", new Rectangle((int)location.X, (int)location.Y, 64, 64), new Rectangle((int)location.X - 64, (int)location.Y - 32, 192, 128), new Vector3(location.X, location.Y, 0), new Vector3(0, 0, 0), 0.0f);
            building.Components.Add(new Engine.Components.ImageDisplayComponent(Game, building, "Textures/buildings/power_generator64x64"));

            return building;
        }
    }
}
