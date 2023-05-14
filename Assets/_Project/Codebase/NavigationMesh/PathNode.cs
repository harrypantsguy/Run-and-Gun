using System;
using UnityEngine;

namespace _Project.Codebase.NavigationMesh
{
    public sealed class PathNode
    {
        public readonly Vector2Int pos;
        public float G { get; set; }
        public float H { get; set; }
        public float F { get; private set; }
        public PathNode parent;
        public float distance;
        public bool inSightOfPlayer;

        public PathNode(Vector2Int pos, float distance, PathNode parent = null)
        {
            this.pos = pos;
            this.distance = distance;
            this.parent = parent;
        }

        public void UpdateF()
        {
            F = G + H;
        }
        
        public bool Equals(PathNode other)
        {
            return pos.Equals(other.pos);
        }

        public override bool Equals(object obj)
        {
            return obj is PathNode other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(pos);
        }
    }
}