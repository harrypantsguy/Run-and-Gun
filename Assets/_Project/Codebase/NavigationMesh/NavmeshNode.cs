using UnityEngine;

namespace _Project.Codebase.NavigationMesh
{
    public sealed class NavmeshNode
    {
        public Vector2Int pos;
        public bool walkable;

        public NavmeshNode(Vector2Int pos, bool walkable)
        {
            this.pos = pos;
            this.walkable = walkable;
        }
    }
}