using System.Collections.Generic;
using UnityEngine;

namespace _Project.Codebase.NavigationMesh
{
    public class Navmesh
    {
        private readonly Dictionary<Vector2Int, NavmeshNode> m_nodes = new();

        public Navmesh(Vector2Int size, bool[,] walkableNodes = null)
        {
            bool initializingNodes = walkableNodes != null;
            for (int x = 0; x < size.x; x++)
            for (int y = 0; y < size.y; y++)
            {
                bool walkable = initializingNodes && walkableNodes[x,y];
                Vector2Int pos = new Vector2Int(x, y);
                m_nodes[pos] = new NavmeshNode(pos, walkable);
            }
        }

        public List<Vector2Int> SolvePath(Vector2Int start, Vector2Int end)
        {
            List<Vector2Int> path = new List<Vector2Int>();

            Dictionary<Vector2Int, PathNode> openNodes = new();
            Dictionary<Vector2Int, PathNode> closedNodes = new();
            
            openNodes.Add(start, new PathNode(start, 0f, 0f, null));

            return path;
        }
        
        public NavmeshNode GetNodeAtPos(Vector2Int pos)
        {
            m_nodes.TryGetValue(pos, out NavmeshNode node);
            return node;
        }

        public void SetNodeWalkableState(Vector2Int pos, bool walkableState)
        {
            m_nodes[pos].walkable = walkableState;
        }
    }
}