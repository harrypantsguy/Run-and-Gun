using System;
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
        public WorldSpacePathController PathController { get; private set; } 
        public Action<Vector2, Vector2Int> onReachPathEnd;
        public bool followPath;

        public bool AtPathEnd => PathController.AtPathEnd;

        private void Awake()
        {
            GameModule gameModule = ModuleUtilities.Get<GameModule>();
            PathController = new WorldSpacePathController(gameModule.Building.navmesh, gameModule.Building.WorldToGrid,
                gameModule.Building.GridToWorld, false);
            PathController.onReachPathEnd += OnReachPathEnd;
        }

        private void OnReachPathEnd(Vector2 worldPos, Vector2Int gridPos)
        {
            onReachPathEnd?.Invoke(worldPos, gridPos);
            transform.position = worldPos;
        }

        private void FixedUpdate()
        {
            if (!followPath) return;
            
            float distToNode = Vector2.Distance(transform.position, PathController.NextNode);
            if (distToNode < m_nodeReachedDist)
                PathController.TryProgressToNextNode();

            if (!PathController.AtPathEnd)
                transform.position = Vector2.MoveTowards(transform.position, PathController.NextNode,
                    Time.fixedDeltaTime * m_moveSpeed);
        }

        public PathResults SetTargetPosition(Vector2Int pos, bool startMoving = true)
        {
            PathResults results = PathController.GenerateAndSetPath(transform.position, pos);
            followPath = startMoving;
            return results;
        }
        
        public PathResults SetTargetPosition(Vector2 pos, bool startMoving = true)
        {
            PathResults results = PathController.GenerateAndSetPath(transform.position, pos);
            followPath = startMoving;
            return results;
        }

        public PathResults GeneratePathTo(Vector2 pos) => PathController.GeneratePath(transform.position, pos);

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying || !m_debugPath) return;

            if (PathController == null) return;
            
            for (var i = 1; i < PathController.Path.Count; i++)
            {
                Vector2 last = PathController.Path[i - 1];
                Vector2 current = PathController.Path[i];
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(last, current);
            }
        }
    }
}