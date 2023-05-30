using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Codebase.Gameplay.World
{
    [Serializable]
    public struct SpawnTileCollection 
    {
        //[HideInInspector] public SpawnTileType type;
        public Color debugColor;
        public bool debug;
        public List<Vector2Int> locations;
    }
}