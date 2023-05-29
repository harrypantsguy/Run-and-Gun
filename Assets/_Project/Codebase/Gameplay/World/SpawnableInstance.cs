using System;

namespace _Project.Codebase.Gameplay.World
{
    [Serializable]
    public struct SpawnableInstance
    {
        public TileScriptable obj;
        public int minOccurrences;
        public int maxOccurrences;
    }
}