using UnityEngine;

namespace _Project.Codebase.NavigationMesh
{
    public class PathNode
    {
        public readonly Vector2Int pos;
        public readonly float g;
        public readonly float h;
        public readonly PathNode parent;

        public PathNode(Vector2Int pos, float g, float h, PathNode parent)
        {
            this.pos = pos;
            this.g = g;
            this.h = h;
            this.parent = parent;
        }
    }
}