using System.Collections.Generic;
using _Project.Codebase.EditorUtilities;
using Priority_Queue;
using UnityEngine;

namespace _Project.Codebase.NavigationMesh
{
    public class DijkstrkaPathfinder
    {
        private readonly Navmesh m_navmesh;
        public DijkstrkaPathfinder(Navmesh navmesh)
        {
            m_navmesh = navmesh;
        }

        public Dictionary<Vector2Int, PathNode> FindPaths(Vector2Int source, int range)
        {
            Dictionary<Vector2Int, PathNode> nodes = new Dictionary<Vector2Int, PathNode>();
            FindPaths(source, range, nodes);
            return nodes;
        }
        
        public void FindPaths(Vector2Int source, int range, in Dictionary<Vector2Int, PathNode> nodesInRange)
        {
            SimplePriorityQueue<PathNode, float> nodes = new SimplePriorityQueue<PathNode, float>();

            PathNode startingNode = new PathNode(source, 0f);
            Dictionary<Vector2Int, PathNode> visitedNodes = new Dictionary<Vector2Int, PathNode>
            {
                {source, startingNode}
            }; 

            nodes.Enqueue(startingNode, 0f);

            int loops = 0;
            while (nodes.TryDequeue(out PathNode currentNode))
            {
                loops++;
                if (loops > 99999)
                {
                    Debug.LogWarning("Very bad");
                    return;
                }
                GizmoUtilities.DrawXAtPos(currentNode.pos + new Vector2(.5f, .5f), 1f);
                nodesInRange.Add(currentNode.pos, currentNode);
                for (int x = -1; x <= 1; x++)
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0) continue;

                    bool isDiagonal = Mathf.Abs(x) == 1 && Mathf.Abs(y) == 1;
                    Vector2Int nodePos = currentNode.pos + new Vector2Int(x, y);

                    if (!IsWalkable(nodePos)) continue;
                    
                    float dist = currentNode.distance + (isDiagonal ? 1.41421f : 1f);
                    if (dist > range) continue;
                    //GizmoUtilities.DrawXAtPos(nodePos + new Vector2(.5f, .5f), 1f, 
                     //   Color.Lerp(Color.red, Color.green, dist / range));

                    if (visitedNodes.TryGetValue(nodePos, out PathNode node))
                    {
                        if (dist < node.distance)
                        {
                            node.distance = dist;
                            node.parent = currentNode;
                            nodes.UpdatePriority(node, dist);
                        }
                    }
                    else
                    {
                        PathNode newNode = new PathNode(nodePos, dist, currentNode);
                        visitedNodes.Add(nodePos, newNode);
                        nodes.Enqueue(newNode, dist);
                    }
                }
            }
        }

        private SimplePriorityQueue<PathNode, float> GetNodeGraph(Vector2Int gridPos, int range)
        {
            SimplePriorityQueue<PathNode, float> nodes = new SimplePriorityQueue<PathNode, float>();
            Queue<PathNode> openTiles = new();
            int size = range * 2 + 1;
            bool[,] visitedPositions = new bool[size, size];

            openTiles.Enqueue(new PathNode(gridPos, 0f));
            while (openTiles.TryDequeue(out PathNode tile))
            {
                nodes.Enqueue(tile, tile.distance);
                for (var x = -1; x <= 1; x++)
                for (var y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0) continue;

                    bool isDiagonal = Mathf.Abs(x) == 1 && Mathf.Abs(y) == 1;

                    Vector2Int newPosition = tile.pos + new Vector2Int(x, y);
                    int localX = newPosition.x - gridPos.x + range;
                    int localY = newPosition.y - gridPos.y + range;
                    if (IsWalkable(newPosition) && !visitedPositions[localX, localY]
                                                && !openTiles.Contains(new PathNode(newPosition, 0f)))
                    {
                        float distance = tile.distance + (isDiagonal ? 1.41421f : 1f);
                        if (distance < range)
                        {
                            openTiles.Enqueue(new PathNode(newPosition, distance));
                        }

                        visitedPositions[localX, localY] = true;
                    }
                }
            }

            return nodes;
        }

        private bool IsWalkable(Vector2Int position) => m_navmesh.IsWalkableNode(position) && m_navmesh.IsValidNode(position);
    }
}