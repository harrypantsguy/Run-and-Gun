using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Codebase.NavigationMesh
{
    public class PathIterator
    {
        public List<Vector2> Path { get; private set; } = new();
        public Vector2 NextNode { get; private set; }
        public Vector2 LastNode { get; private set; }
        public Vector2 DirToNextNode { get; private set; }
        public float DistFromLastToNextNode { get; private set; }
        public bool AtPathEnd { get; private set; }
        public event Action<Vector2> OnReachPathEnd;
        
        private int m_pathIndex;

        public void SetPath(in List<Vector2> path)
        {
            Path = new List<Vector2>(path);
            m_pathIndex = 0;
            UpdateData();
        }
        
        public void ProgressToNextNode()
        {
            if (AtPathEnd) return;
            m_pathIndex++;
            UpdateData();
        }

        private void UpdateData()
        {
            AtPathEnd = m_pathIndex >= Path.Count - 1;
            if (AtPathEnd)
            {
                OnReachPathEnd?.Invoke(Path[^1]);
            }
            if (Path.Count == 0) return;
            LastNode = Path[m_pathIndex];
            if (!AtPathEnd)
                NextNode = Path[m_pathIndex + 1];
            DirToNextNode = NextNode - Path[m_pathIndex];
            DistFromLastToNextNode = DirToNextNode.magnitude;
            DirToNextNode.Normalize();
        }
    }
}