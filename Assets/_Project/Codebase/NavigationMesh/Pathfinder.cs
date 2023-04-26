using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Project.Codebase.NavigationMesh
{
    public sealed class Pathfinder
    {
        private readonly Dictionary<Vector2Int, PathNode> m_openNodes = new();
        private readonly Dictionary<Vector2Int, PathNode> m_closedNodes = new();
        private Navmesh m_navmesh;
        
        public Pathfinder(Navmesh navmesh)
        {
            m_navmesh = navmesh;
        }

        public PathResult FindPath(in Vector2Int start, in Vector2Int end, in bool cardinalOnly, in List<Vector2Int> path,
            in bool allowPartialPaths = false, in Heuristic heuristic = Heuristic.Euclidean)
        {
            path.Clear();
            m_openNodes.Clear();
            m_closedNodes.Clear();

            var startNode = new PathNode(start);
            m_openNodes.Add(start, startNode);

            var nodesVisited = 0;
            
            while (m_openNodes.Count > 0)
            {
                var currentNode = m_openNodes.Values.OrderBy(node => node.F).First();
                
                m_openNodes.Remove(currentNode.Pos);
                m_closedNodes.Add(currentNode.Pos, currentNode);

                if (currentNode.Pos == end)
                {
                    TracePathNonAlloc(currentNode, path);
                    return PathResult.FullPath;
                }

                if (nodesVisited > Navmesh.SEARCH_LIMIT)
                {
                    if (allowPartialPaths)
                    {
                        TracePathNonAlloc(GetNodeClosestToCell(end, m_closedNodes.Values), path);
                        return PathResult.PartialPath;
                    }
                    
                    TracePathNonAlloc(null, path);
                    return PathResult.NoPath;
                }

                for (var x = -1; x <= 1; x++)
                {
                    for (var y = -1; y <= 1; y++)
                    {
                        var isDiagonal = Mathf.Abs(x) == 1 && Mathf.Abs(y) == 1;
                        if (x == 0 && y == 0 || (cardinalOnly && isDiagonal)) continue;

                        if (isDiagonal)
                        {
                            if (!m_navmesh.IsWalkableNode(currentNode.Pos + new Vector2Int(x, 0)) ||
                                !m_navmesh.IsWalkableNode(currentNode.Pos + new Vector2Int(0, y))) continue;
                        }
                        
                        var cell = currentNode.Pos + new Vector2Int(x, y);

                        if (m_closedNodes.ContainsKey(cell))
                            continue;
                        
                        if (!m_navmesh.IsValidNode(cell))
                            continue;

                        if (!m_navmesh.IsWalkableNode(cell))
                            continue;

                        var child = new PathNode(cell);
                        var additiveGCost = 0f;//m_navmesh.GetNodeAdditiveGCost(cell);
                        
                        child.G = currentNode.G + additiveGCost + (isDiagonal ? Navmesh.DIAGONAL_COST : Navmesh.CARDINAL_COST);
                        child.H = CalculateHeuristic(heuristic, cell, end);
                        child.UpdateF();
                        child.parent = currentNode;

                        if (m_openNodes.TryGetValue(cell, out var nodeAlreadyInOpenList) && child.F > nodeAlreadyInOpenList.F)
                            continue;

                        m_openNodes[cell] = child;
                    }
                }

                nodesVisited++;
            }

            return PathResult.NoPath;
        }

        private static PathNode GetNodeClosestToCell(in Vector2Int cell, in IEnumerable<PathNode> nodes)
        {
            var closestDist = float.MaxValue;
            PathNode closestNode = default;
            
            foreach (var node in nodes)
            {
                var dist = Vector2.SqrMagnitude(node.Pos - cell);

                if (dist > closestDist) continue;

                closestDist = dist;
                closestNode = node;
            }

            return closestNode;
        }

        private void TracePathNonAlloc(in PathNode fromNode, in List<Vector2Int> path)
        {
            path.Clear();

            if (fromNode == null) return;

            PathNode node = fromNode;
            while (node != null)
            {
                path.Add(node.Pos);
                node = node.parent;
            }
        }

        private static float CalculateHeuristic(in Heuristic heuristic, in Vector2Int cell, in Vector2Int goalCell)
        {
            var dx = Mathf.Abs(cell.x - goalCell.x);
            var dy = Mathf.Abs(cell.y - goalCell.y);

            return heuristic switch
            {
                Heuristic.Manhattan => dx + dy,
                Heuristic.Diagonal => Navmesh.CARDINAL_COST * (dx + dy) + 
                                      (Navmesh.DIAGONAL_COST - 2f * Navmesh.CARDINAL_COST) * Mathf.Min(dx, dy),
                Heuristic.Euclidean => Mathf.Sqrt(Mathf.Pow(cell.x - goalCell.x, 2) + Mathf.Pow(cell.y - goalCell.y, 2)),
                _ => default
            };
        }
    }
}