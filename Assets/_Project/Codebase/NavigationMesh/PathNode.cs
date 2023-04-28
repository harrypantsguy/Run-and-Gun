using UnityEngine;

namespace _Project.Codebase.NavigationMesh
{
    public sealed class PathNode
    {
        public Vector2Int Pos { get; }
        public float G { get; set; }
        public float H { get; set; }
        public float F { get; private set; }
        public PathNode parent;
        public readonly float distance;

        public PathNode(Vector2Int pos, float distance)
        {
            Pos = pos;
            this.distance = distance;
        }

        public void UpdateF()
        {
            F = G + H;
        }
    }
}