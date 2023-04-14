using UnityEngine;

namespace _Project.Codebase.Navmesh
{
    public class NavmeshNode
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