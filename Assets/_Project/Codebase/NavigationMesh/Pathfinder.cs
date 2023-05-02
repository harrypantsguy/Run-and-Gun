using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Project.Codebase.NavigationMesh
{
    public sealed class Pathfinder
    {
        private readonly Dictionary<Vector2Int, PathNode> m_openNodes = new();
        private readonly Dictionary<Vector2Int, PathNode> m_closedNodes = new();
        private readonly Navmesh m_navmesh;

        private const float c_diagonal_dist = 1.41421f;

        public Pathfinder(Navmesh navmesh)
        {
            m_navmesh = navmesh;
        }

        private PathResults FindPath(in Vector2Int start, in Vector2Int end, in bool cardinalOnly, in List<Vector2Int> path, 
            PathResultType forcedResultType, in bool allowPartialPaths = false, float maxDistance = Mathf.Infinity,
            in Heuristic heuristic = Heuristic.Euclidean)
        {
            PathResults results = FindPath(start, end, cardinalOnly, path, allowPartialPaths, maxDistance, heuristic);
            return new PathResults(forcedResultType, results.distance);
        }
        
        public PathResults FindPath(in Vector2Int start, in Vector2Int end, in bool cardinalOnly, in List<Vector2Int> path,
            in bool allowPartialPaths = false, float maxDistance = Mathf.Infinity, in Heuristic heuristic = Heuristic.Euclidean)
        {
            path.Clear();
            
            if (maxDistance == 0f)
                return new PathResults(PathResultType.NoPath, 0f);
            
            m_openNodes.Clear();
            m_closedNodes.Clear();

            var startNode = new PathNode(start, 0f);
            m_openNodes.Add(start, startNode);

            var nodesVisited = 0;
            
            while (m_openNodes.Count > 0)
            {
                PathNode currentNode = m_openNodes.Values.OrderBy(node => node.F).First();
                
                m_openNodes.Remove(currentNode.Pos);
                m_closedNodes.Add(currentNode.Pos, currentNode);

                if (currentNode.Pos == end)
                {
                    bool isPartialPath = TracePathAndReturnPartialPathState(currentNode, path, maxDistance, out PathNode endNode);
                    if (allowPartialPaths || !isPartialPath)
                    {
                        if (isPartialPath)
                            return FindPath(start, endNode.Pos, cardinalOnly, path, PathResultType.PartialPath, 
                                true, maxDistance, heuristic);
                        
                        return new PathResults(PathResultType.FullPath, endNode.distance);
                    }

                    return new PathResults(PathResultType.NoPath, 0f);
                }

                if (nodesVisited > Navmesh.SEARCH_LIMIT)
                {
                    if (allowPartialPaths)
                    {
                        PathNode closestNode = GetNodeClosestToCell(end, m_closedNodes.Values);
                        TracePathAndReturnPartialPathState(closestNode, path, maxDistance, out PathNode endNode);
                        return new PathResults(PathResultType.PartialPath, endNode.distance);
                    }
                    
                    TracePathAndReturnPartialPathState(null, path, maxDistance, out PathNode endNode2);
                    return new PathResults(PathResultType.NoPath, 0f);
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
                        
                        float newDistance = currentNode.distance + (isDiagonal ? c_diagonal_dist : 1f);
                        if (!allowPartialPaths && newDistance > maxDistance) continue;

                        PathNode child = new PathNode(cell, newDistance);
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

            return new PathResults(PathResultType.NoPath, 0f);
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

        private bool TracePathAndReturnPartialPathState(in PathNode fromNode, in List<Vector2Int> path, float maxDistance, 
            out PathNode endNode)
        {
            path.Clear();
            
            endNode = null;
            
            if (fromNode == null) return false;

            bool isPartialPath = false;
            
            PathNode node = fromNode;
            while (node != null)
            {
                if (node.distance < maxDistance)
                {
                    if (endNode == null)
                        endNode = node;
                    path.Add(node.Pos);
                }
                else
                    isPartialPath = true;

                node = node.parent;
            }

            return isPartialPath;
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