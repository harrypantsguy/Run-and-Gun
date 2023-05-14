
using _Project.Codebase.Gameplay.Characters;

namespace _Project.Codebase.Gameplay.World
{
    public class WorldRef
    {
        public readonly Building building;
        public Runner runner;
        public WorldRef(Building building)
        {
            this.building = building;
        }
    }
}