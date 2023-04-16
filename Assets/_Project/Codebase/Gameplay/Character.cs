using _Project.Codebase.Gameplay.World;
using UnityEngine;

namespace _Project.Codebase.Gameplay
{
    [RequireComponent(typeof(NavmeshAgent))]
    public class Character : MonoBehaviour
    {
        [SerializeField] private bool m_moveToTestTarget;
        private GameObject m_navTarget;
        private NavmeshAgent m_agent;
        
        private Building m_building;

        private void Start()
        {
            m_agent = GetComponent<NavmeshAgent>();
            
            m_navTarget = new GameObject("Navmesh Target")
            {
                transform =
                {
                    position = transform.position,
                    parent = transform
                }
            };
        }

        private void Update()
        {
            if (m_moveToTestTarget)
            {
                m_moveToTestTarget = false;
                m_agent.SetTargetPosition(m_navTarget.transform.position);
            }
        }
    }
}