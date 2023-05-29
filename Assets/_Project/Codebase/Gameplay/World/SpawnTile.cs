using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Codebase.Gameplay.World
{
    [Serializable]
    public struct SpawnTile 
    {
        public List<TileScriptable> spawnableTiles;
        public List<Vector2Int> locations;
        public Color debugColor;
    }
}