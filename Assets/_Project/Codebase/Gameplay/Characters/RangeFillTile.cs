using System;
using UnityEngine;

namespace _Project.Codebase.Gameplay.Characters
{
    public struct RangeFillTile
    {
        public readonly Vector2Int pos;
        public readonly float distance;

        public RangeFillTile(Vector2Int pos, float distance)
        {
            this.pos = pos;
            this.distance = distance;
        }

        public bool Equals(RangeFillTile other)
        {
            return pos.Equals(other.pos);
        }

        public override bool Equals(object obj)
        {
            return obj is RangeFillTile other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(pos);
        }
    }
}