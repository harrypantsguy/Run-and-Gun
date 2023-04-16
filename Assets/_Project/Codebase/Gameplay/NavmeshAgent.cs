using _Project.Codebase.Modules;
using _Project.Codebase.NavigationMesh;
using DanonFramework.Runtime.Core.Utilities;
using UnityEngine;

namespace _Project.Codebase.Gameplay
{
    public class NavmeshAgent : MonoBehaviour
    {
        [SerializeField] private bool m_debugPath;
        [SerializeField] private float m_moveSpeed;
        [SerializeField] private float m_nodeReachedDist;
        private WorldSpacePathController m_pathController;

        private void Start()
        {
            GameModule gameModule = ModuleUtilities.Get<GameModule>();
            m_pathController = new WorldSpacePathController(gameModule.Navmesh, gameModule.Building.WorldToGrid,
                gameModule.Building.GridToWorld, true);
        }

        private void FixedUpdate()
        {
            float distToNode = Vector2.Distance(transform.position, m_pathController.NextNode);
            if (distToNode < m_nodeReachedDist)
                m_pathController.TryProgressToNextNode();

            if (!m_pathController.AtPathEnd)
            {
                transform.position = Vector2.MoveTowards(transform.position, m_pathController.NextNode, 
                    Time.fixedDeltaTime * m_moveSpeed);
            }
        }

        public void SetTargetPosition(Vector2 pos)
        {
            m_pathController.GeneratePath(transform.position, pos);
        }
        
        private void OnDrawGizmos()
        {
            if (!Application.isPlaying || !m_debugPath) return;
            
            for (var i = 1; i < m_pathController.Path.Count; i++)
            {
                Vector2 last = m_pathController.Path[i - 1];
                Vector2 current = m_pathController.Path[i];
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(last, current);
            }
        }
    }
}