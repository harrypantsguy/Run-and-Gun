
using _Project.Codebase.Gameplay.Characters;

namespace _Project.Codebase.Gameplay.World
{
    public class WorldScreenshot
    {
        public readonly Building building;
        public readonly Runner runner; 
        public WorldScreenshot(Building building, Runner runner)
        {
            this.building = building;
            this.runner = runner;
        }
    }
}