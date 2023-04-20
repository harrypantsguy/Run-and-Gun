using System;
using System.Collections.Generic;
using DanonFramework.Runtime.Core.Utilities;
using UnityEngine;

namespace _Project.Codebase.NavigationMesh
{
    public sealed class WorldSpacePathController
    {
        private Pathfinder m_pathfinder;
        public List<Vector2> Path { get; }
        public Vector2 NextNode { get; private set; }
        public Vector2 LastNode { get; private set; }
        public Vector2 DirToNextNode { get; private set; }
        public float DistFromLastToNextNode { get; private set; }
        public bool AtPathEnd { get; private set; }
        public Action<Vector2, Vector2Int> OnReachPathEnd;
        private int m_pathIndex;
        private readonly List<Vector2Int> m_gridPath = new();
        private readonly bool m_cardinalOnly;
        private readonly Func<Vector2, Vector2Int> m_worldToGrid;
        private Func<Vector2Int, Vector2> m_gridToWorld;
        private readonly Converter<Vector2Int, Vector2> m_gridToWorldConverter;

        public WorldSpacePathController(Navmesh navmesh, Func<Vector2, Vector2Int> worldToGrid, 
            Func<Vector2Int, Vector2> gridToWorld, bool cardinalOnly)
        {
            m_pathfinder = new Pathfinder(navmesh);
            m_worldToGrid = worldToGrid;
            m_gridToWorld = gridToWorld;
            m_gridToWorldConverter = new Converter<Vector2Int, Vector2>(gridToWorld);
            m_cardinalOnly = cardinalOnly;
            Path = new List<Vector2>();
            AtPathEnd = true;
            OnReachPathEnd = (vector2, vector2Int) => { };
        }

        public PathResult GenerateAndSetPath(Vector2 source, Vector2 target)
        {
            PathResult result = GeneratePath(source, target, Path, m_gridPath);
            m_pathIndex = 0;
            if (result is PathResult.FullPath or PathResult.PartialPath)
                UpdateData();
            return result;
        }

        public PathResult GeneratePath(Vector2 source, Vector2 target, in List<Vector2> path)
        {
            List<Vector2Int> gridPath = new List<Vector2Int>();
            return GeneratePath(source, target, path, gridPath);
        }

        private PathResult GeneratePath(Vector2 source, Vector2 target, in List<Vector2> path, in List<Vector2Int> gridPath)
        {
            PathResult result = m_pathfinder.FindPath(m_worldToGrid(source), m_worldToGrid(target), m_cardinalOnly, gridPath);
            path.Clear();
            path.AddRange(gridPath.ConvertAll(m_gridToWorldConverter));
            path.Reverse();
            return result;
        }

        public bool TryProgressToNextNode()
        {
            if (AtPathEnd) return false;
            m_pathIndex++;
            UpdateData();
            return true;
        }

        private void UpdateData()
        {
            AtPathEnd = m_pathIndex >= Path.Count - 1;
            if (AtPathEnd)
                OnReachPathEnd?.Invoke(Path[^1], m_gridPath[^1]);
            if (Path.Count == 0) return;
            LastNode = Path[m_pathIndex];
            if (!AtPathEnd)
                NextNode = Path[m_pathIndex + 1];
            DirToNextNode = Path[m_pathIndex] - NextNode;
            DistFromLastToNextNode = DirToNextNode.magnitude;
            DirToNextNode.Normalize();
        }
    }
}