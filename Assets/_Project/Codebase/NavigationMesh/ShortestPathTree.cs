using System.Collections.Generic;
using UnityEngine;

namespace _Project.Codebase.NavigationMesh
{
    public sealed class ShortestPathTree
    {
        public readonly Vector2Int source;
        public readonly Dictionary<Vector2Int, PathNode> nodes;
        
        public ShortestPathTree(Vector2Int source, Dictionary<Vector2Int, PathNode> nodes)
        {
            this.source = source;
            this.nodes = nodes;
        }

        public List<PathNode> GetNodesInRange(float range)
        {
            List<PathNode> nodesInRange = new List<PathNode>();
            foreach (PathNode node in nodes.Values)
                if (node.distance <= range)
                    nodesInRange.Add(node);
            return nodesInRange;
        }

        public void AddNodes(Dictionary<Vector2Int, PathNode> newNodes)
        {
            foreach (KeyValuePair<Vector2Int, PathNode> pair in newNodes)
                nodes[pair.Key] = pair.Value;
        }

        public bool ContainsPoint(Vector2Int point) => nodes.ContainsKey(point);

        public PathResults TryTracePath(Vector2Int pathEnd, in List<Vector2Int> path, float maxRange = Mathf.Infinity)
        {
            if (!nodes.TryGetValue(pathEnd, out PathNode node))
            {
                return new PathResults(PathResultType.NoPath, 0f);
            }

            float distance = node.distance;
            bool outsideOfRange = false;

            while (node != null)
            {
                if (node.distance <= maxRange)
                {
                    if (outsideOfRange)
                    {
                        outsideOfRange = false;
                        distance = node.distance;
                    }
                    path.Add(node.pos);
                }
                else
                    outsideOfRange = true;

                node = node.parent;
            }
            
            path.Reverse();

            return new PathResults(PathResultType.FullPath, distance);
        }
    }
}