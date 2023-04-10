using System;
using UnityEngine.Tilemaps;

namespace _Project.Codebase.Gameplay
{
    [Serializable]
    public struct WallCellData
    {
        public TileBase tileBase;
        public int pierceInfluence;
        public int ricochetInfluence;
    }
}