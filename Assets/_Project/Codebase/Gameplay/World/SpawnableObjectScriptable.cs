using UnityEngine;
using UnityEngine.Tilemaps;

namespace _Project.Codebase.Gameplay.World
{
    public abstract class SpawnableObjectScriptable : ScriptableObject
    {
        public abstract ISpawnableObject SpawnObject(Building building, Vector2Int pos);
    }
}