using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Codebase.Gameplay.World
{
    [Serializable]
    public struct SpawnTileCollection 
    {
        public Color debugColor;
        public bool debug;
        public List<Vector2Int> locations;
    }
}