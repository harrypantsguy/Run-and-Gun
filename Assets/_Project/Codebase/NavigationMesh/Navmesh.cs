using System.Collections.Generic;
using UnityEngine;

namespace _Project.Codebase.NavigationMesh
{
    public sealed class Navmesh
    {
        public const float DIAGONAL_COST = 1f;
        public const float CARDINAL_COST = .707f;
        public const int SEARCH_LIMIT = 1000;
        
        private readonly Dictionary<Vector2Int, NavmeshNode> m_nodes = new();

        public Navmesh(Dictionary<Vector2Int, bool> nodes)
        {
            foreach (KeyValuePair<Vector2Int, bool> node in nodes)
            {
                m_nodes[node.Key] = new NavmeshNode(node.Key, node.Value);
            }
        }

        public bool IsValidNode(Vector2Int pos) => true;

        public bool IsWalkableNode(Vector2Int pos)
        {
            return m_nodes.TryGetValue(pos, out NavmeshNode node) && node.walkable;
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