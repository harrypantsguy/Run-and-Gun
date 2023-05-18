﻿using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Codebase.Gameplay.World;
using _Project.Codebase.Modules;
using _Project.Codebase.NavigationMesh;
using DanonFramework.Runtime.Core.Utilities;
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

        public readonly Dictionary<Vector2Int, ShortestPathTree> pathTrees = new();

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
        
        public void CalculateAllPathsFromSource(Vector2 pos, float range)
        {
            CalculateAllPathsFromSource(m_building.WorldToGrid(pos), range);
        }
        
        public void CalculateAllPathsFromSource(Vector2Int gridPos, float range)
        {
            ShortestPathTree newTree = new ShortestPathTree(gridPos, m_dijkstrkaPathfinder.FindPaths(gridPos, (int)range));
            pathTrees[gridPos] = newTree;
            OnGeneratePathTree?.Invoke(newTree);
        }

        private bool TryGetPathTreeAtCurrentPosition(out ShortestPathTree tree) =>
            TryGetPathTreeAtPosition(m_building.WorldToGrid(transform.position), out tree);    

        private bool TryGetPathTreeAtPosition(Vector2Int pos, out ShortestPathTree tree) =>
            pathTrees.TryGetValue(pos, out tree);
        
        public Vector2 GetClosestTilePosInRange(Vector2 pos, float maxDistance, out float distanceFromAgent)
        {
            Vector2Int gridPos = m_building.WorldToGrid(pos);
            if (TryGetPathTreeAtCurrentPosition(out ShortestPathTree tree) && tree.nodes.TryGetValue(gridPos, out PathNode node) && 
                node.distance <= maxDistance)
            {
                distanceFromAgent = node.distance;
                return m_building.GridToWorld(node.pos);
            }
            
            if (tree == null)
            {
                Debug.LogWarning("Failed to locate tree at current position, try updating paths.");
                distanceFromAgent = 0f;
                return pos;
            }

            PathNode closestTile = 
                tree.GetNodesInRange(maxDistance).OrderBy(n => Vector2.Distance(n.pos, gridPos)).ToList()[0];
            distanceFromAgent = closestTile.distance;
            return m_building.GridToWorld(closestTile.pos);
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
            List<Vector2Int> gridPath = new List<Vector2Int>();
            PathResults results = TryGetPath(m_building.WorldToGrid(source), m_building.WorldToGrid(target), gridPath);
            path.Clear();
            path.AddRange(gridPath.ConvertAll(m_gridToWorldConverter));
            return results;
        }

        public PathResults TryGetPath(Vector2Int source, Vector2Int target, in List<Vector2Int> path)
        {
            path.Clear();
            if (!TryGetPathTreeAtPosition(source, out ShortestPathTree tree))
                return new PathResults(PathResultType.NoPath, 0f);
            if (tree.ContainsPoint(target))
                return tree.TryTracePath(target, path);
            return new PathResults(PathResultType.NoPath, 0f);
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