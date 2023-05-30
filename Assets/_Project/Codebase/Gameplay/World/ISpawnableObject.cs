using UnityEngine;

namespace _Project.Codebase.Gameplay.World
{
    public interface ISpawnableObject
    {
        public ISpawnableObject Spawn(Vector2Int pos);
    }
}