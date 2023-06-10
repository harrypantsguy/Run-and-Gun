using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Codebase.Gameplay.World;
using _Project.Codebase.Modules;
using _Project.Codebase.NavigationMesh;
using DanonFramework.Core;
using UnityEngine;

namespace _Project.Codebase.Gameplay.Characters
{
    public class NavmeshAgent : MonoBehaviour
    {
        [SerializeField] private bool m_debugPath;
        [SerializeField] private float m_moveSpeed;
        [SerializeField] private float m_nodeReachedDist;
        private PathIterator m_pathIterator;
        private DijkstrkaPathfinder m_dijkstrkaPathfinder;
        public event Action<Vector2, Vector2Int> OnReachPathEnd;
        public event Action<ShortestPathTree> OnGeneratePathTree;  
        public Vector2 PathDir => m_pathIterator.DirToNextNode;
        [HideInInspector] public bool followPath;
        private Building m_building;
        private Converter<Vector2Int, Vector2> m_gridToWorldConverter;
        private Converter<Vector2, Vector2Int> m_worldToGridConverter;

        public ShortestPathTree PathTree { get; private set; }

        public bool AtPathEnd => m_pathIterator.AtPathEnd;

        private void Awake()
        {
            GameModule gameModule = ModuleUtilities.Get<GameModule>();
            m_building = gameModule.Building;
            m_pathIterator = new PathIterator();
            m_pathIterator.OnReachPathEnd += OnArriveAtPathEnd;
            m_dijkstrkaPathfinder = new DijkstrkaPathfinder(m_building.navmesh);
            m_gridToWorldConverter = m_building.GridToWorld;
            m_worldToGridConverter = m_building.WorldToGrid;
        }

        private void FixedUpdate()
        {
            if (!followPath) return;

            float distToNode = Vector2.Distance(transform.position, m_pathIterator.NextNode);
            if (distToNode < m_nodeReachedDist)
                m_pathIterator.ProgressToNextNode();

            if (!m_pathIterator.AtPathEnd)
                transform.position = Vector2.MoveTowards(transform.position, m_pathIterator.NextNode,
                    Time.fixedDeltaTime * m_moveSpeed);
        }
        
        public bool IsGeneratedPathTreeAtPos(Vector2Int pos) => PathTree != null && PathTree.source == pos;

        public void CalculateAllPathsFromSource(Vector2Int gridPos, float range)
        {
            PathTree = new ShortestPathTree(gridPos, m_dijkstrkaPathfinder.FindPaths(gridPos, (int)range));

            OnGeneratePathTree?.Invoke(PathTree);
        }

        public bool TryGetClosestTilePosInRange(Vector2 pos, float maxDistance, out float distanceFromAgent, out Vector2 closestPos)
        {
            Vector2Int gridPos = m_building.WorldToGrid(pos);
            closestPos = Vector2.zero;
            distanceFromAgent = 0f;
            
            if (PathTree == null)
            {
                Debug.LogWarning("Path tree uninitialized");
                return false;
            }
            
            if (PathTree.nodes.TryGetValue(gridPos, out PathNode node) && node.distance <= maxDistance && 
                !m_building.IsFloorObjectAtPos(node.pos))
            {
                distanceFromAgent = node.distance;
                closestPos = m_building.GridToWorld(node.pos);
                return true;
            }

            List<PathNode> nodesInRange = PathTree.GetNodesInRange(maxDistance);
            nodesInRange.RemoveAll(n => m_building.IsFloorObjectAtPos(n.pos));
            
            if (nodesInRange.Count == 0)
                return false;
            
            PathNode closestTile = nodesInRange.OrderBy(n => Vector2.Distance(n.pos, gridPos)).ToList()[0];
            distanceFromAgent = closestTile.distance;
            closestPos = m_building.GridToWorld(closestTile.pos);
            return true;
        }

        private void OnArriveAtPathEnd(Vector2 worldPos)
        {
            OnReachPathEnd?.Invoke(worldPos, m_building.WorldToGrid(worldPos));
            transform.position = worldPos;
        }
        
        public PathResults TryGetPath(Vector2 target, in List<Vector2> path)
        {
            return TryGetPath(transform.position, target, path);
        }
        
        public PathResults TryGetPath(Vector2Int target, in List<Vector2> path)
        {
            return TryGetPath(transform.position, m_building.GridToWorld(target), path);
        }

        public PathResults TryGetPath(Vector2 source, Vector2 target, in List<Vector2> path)
        {
            return TryGetPath(m_building.WorldToGrid(source), m_building.WorldToGrid(target), path);
        }

        public PathResults TryGetPath(Vector2Int source, Vector2Int target, in List<Vector2> path, float maxDistance = Mathf.Infinity)
        {
            path.Clear();
            if (PathTree == null)
                return new PathResults(PathResultType.NoPath, 0f);
            if (PathTree.ContainsPoint(target))
                return TracePath(PathTree, target, path, maxDistance);

            if (!TryGenPath(source, target)) return new PathResults(PathResultType.NoPath, 0f);

            return TracePath(PathTree, target, path, maxDistance);
        }

        private PathResults TracePath(ShortestPathTree tree, Vector2Int target, in List<Vector2> path, float maxDistance = Mathf.Infinity)
        {
            List<Vector2Int> gridPath = new List<Vector2Int>();
            PathResults results = tree.TryTracePath(target, gridPath, maxDistance);
            path.AddRange(gridPath.ConvertAll(m_gridToWorldConverter));
            return results;
        }

        public bool TryGenPath(Vector2Int source, Vector2Int target)
        {
            AstarPathfinder pathViabilityTester = new AstarPathfinder(m_building.navmesh);
            if (pathViabilityTester.FindPath(source, target, false, new List<Vector2Int>()).type == PathResultType.NoPath)
                return false;

            Dictionary<Vector2Int, PathNode> nodes = m_dijkstrkaPathfinder.FindPathToTarget(source, target);
            if (PathTree != null)
                PathTree.AddNodes(nodes);
            else
                PathTree = new ShortestPathTree(source, nodes);
            
            return true;
        }

        public PathResults SetPathTo(Vector2Int target, bool startFollowingPath)
        {
            return SetPathTo(m_building.GridToWorld(target), startFollowingPath);
        }
        
        public PathResults SetPathTo(Vector2 target, bool startFollowingPath)
        {
            List<Vector2> path = new List<Vector2>();
            PathResults results = TryGetPath(target, path);
            if (results.type == PathResultType.FullPath)
            {
                SetPathTo(path, startFollowingPath);
            }
                
            return results;
        }

        public void SetPathTo(List<Vector2> path, bool startFollowingPath)
        {
            m_pathIterator.SetPath(path);
            followPath = startFollowingPath;
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying || !m_debugPath) return;

            if (m_pathIterator == null) return;
            
            for (var i = 1; i < m_pathIterator.Path.Count; i++)
            {
                Vector2 last = m_pathIterator.Path[i - 1];
                Vector2 current = m_pathIterator.Path[i];
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(last, current);
            }
        }
    }
}