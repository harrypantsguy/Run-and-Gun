using System;
using UnityEngine.Tilemaps;

namespace _Project.Codebase.Gameplay.World
{
    [Serializable]
    public struct WallCellData
    {
        public TileBase tileBase;
        public WallType type;
        public int pierceInfluence;
        public int ricochetInfluence;
    }
}