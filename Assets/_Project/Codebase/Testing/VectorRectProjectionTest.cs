using _Project.Codebase.Gameplay.World;
using _Project.Codebase.Modules;
using DanonFramework.Runtime.Core.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Codebase.Testing
{
    public class VectorRectProjectionTest : MonoBehaviour
    {
        private GameObject m_startObject;
        private GameObject m_endObject;
        private WorldRegions m_worldRegions;
        
        private void Start()
        {
            m_worldRegions = new WorldRegions();
            SceneManager.SetActiveScene(gameObject.scene);
            m_startObject = new GameObject("Start");
            m_endObject = new GameObject("End");
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying) return;
            
            Gizmos.color = Color.green;
            var startPos = m_startObject.transform.position;
            Gizmos.DrawWireSphere(startPos, .5f);
            Gizmos.color = Color.yellow;
            var endPos = m_endObject.transform.position;
            Gizmos.DrawWireSphere(endPos, .5f);
            Gizmos.DrawLine(startPos, endPos);
            Gizmos.DrawWireCube(Vector3.zero, m_worldRegions.shooterRegionExtents * 2f);

            Gizmos.color = Color.red;
            Vector2 insetRect = m_worldRegions.GetInsetRect(startPos, m_worldRegions.shooterRegionExtents);
            Gizmos.DrawWireCube(startPos, insetRect * 2f);
            
            Vector2 projectedPos = endPos;
            if (!m_worldRegions.IsPointInsideRegion(projectedPos, m_worldRegions.shooterRegionExtents))
                projectedPos = m_worldRegions.ProjectVectorOntoRegionEdge(
                    startPos, endPos - startPos, m_worldRegions.shooterRegionExtents);
            Gizmos.DrawWireSphere(projectedPos, .5f);
        }
    }
}
