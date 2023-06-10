using _Project.Codebase.Gameplay.World;
using _Project.Codebase.Modules;
using DanonFramework.Core;
using UnityEngine;

namespace _Project.Codebase.Gameplay.Shooter
{
    public class LaserSight : MonoBehaviour
    {
        private LineRenderer m_lineRenderer;

        private Building m_building;
        
        private void Awake()
        {
            m_lineRenderer = GetComponent<LineRenderer>();
            m_building = ModuleUtilities.Get<GameModule>().Building;
        }

        public bool SetActivityState(bool state) => m_lineRenderer.enabled = state;

        private void LateUpdate()
        {
            Vector2 position = transform.position;
            Vector2 end = position;

            int iterations = 0;
            while (true)
            {
                iterations++;
                if (iterations > 100)
                {
                    Debug.LogWarning($"{nameof(LaserSight)}: stack overflow");
                    break;
                }
                
                RaycastHit2D hit = Physics2D.Raycast(position, transform.right);
                if (hit.collider == null)
                {
                    end = position + (Vector2)transform.right * 100f;
                    break;
                }

                Vector2 pointInsideCell = hit.point - hit.normal * .001f;
                Wall wall = m_building.GetWallAtPos(pointInsideCell);
                
                if (wall == null || wall.type != WallType.Glass)
                {
                    end = hit.point;
                    break;
                }

                position = pointInsideCell;
            }
            
            m_lineRenderer.positionCount = 2;
            m_lineRenderer.SetPositions(new Vector3[]
            {
                transform.position,
                end
            });
        }
    }
}