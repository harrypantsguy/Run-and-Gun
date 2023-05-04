using System.Collections.Generic;
using UnityEngine;

namespace _Project.Codebase.NavigationMesh
{
    public class ShortestPathTree
    {
        public readonly Vector2Int source;
        public readonly Dictionary<Vector2Int, PathNode> nodes;
        public ShortestPathTree(Vector2Int source, Dictionary<Vector2Int, PathNode> nodes)
        {
            this.source = source;
            this.nodes = nodes;
        }
    }
}