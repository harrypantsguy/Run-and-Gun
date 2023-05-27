using System;
using UnityEngine.Tilemaps;

namespace _Project.Codebase.Gameplay.World
{
    [Serializable]
    public struct SerializedTile
    {
        public TileBase tile;
        public ISerializedTile serializedTile;
    }
}