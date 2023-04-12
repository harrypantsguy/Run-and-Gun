using System.Collections.Generic;
using DanonFramework.Runtime.Core.Utilities;
using UnityEngine;

namespace _Project.Codebase.Gameplay
{
    public class ProjectileSim
    {
        private float m_speed;
        private int m_pierceStrength;

        private const float c_cell_check_cast_dist = .03f;
        public const float DEFAULT_SPEED = 75f;
        private const float c_max_travel_dist = 120f;
            
        public ProjectileSim()
        {
            m_speed = DEFAULT_SPEED;
            m_pierceStrength = 2;
        }
        
        public List<ProjectileEvent> Simulate(Vector2 start, Vector2 direction)
        {
            Vector2 currentPosition = start;
            float simTime = 0f;
            
            List<ProjectileEvent> events = new List<ProjectileEvent>()
            {
                new(ProjectileEventType.Position, currentPosition, simTime)
            };

            float distanceTraveled = 0f;
            Cell cellInsideOf = null;
            float pierceHealth = m_pierceStrength;
            
            int loop = 0;
            while (true)
            {
                loop++;
                if (loop == 500)
                {
                    Debug.Log("stack overflow");
                    Debug.Break();
                    break;
                }
                float maxDistRemaining = c_max_travel_dist - distanceTraveled;
                float castDistance = maxDistRemaining;
                if (cellInsideOf != null)
                    castDistance = Mathf.Max(castDistance, CalculateEquivalentTravelDistanceFromDirection(direction) + .01f);

                Vector2 raycastSource = currentPosition;
                if (cellInsideOf != null)
                    raycastSource += direction * .001f;
                RaycastHit2D hit = Physics2D.Raycast(raycastSource, direction, castDistance);
                
                Vector2 raycastEnd = hit.collider == null ? currentPosition + direction * castDistance : hit.point;

                //Debug.DrawLine(currentPosition, raycastEnd, Color.red);
                //GizmoUtilities.DrawXAtPos(raycastEnd, .25f);

                Vector2? exitPoint = null;
                float exitTime = 0f;
                if (cellInsideOf != null)
                {
                   // float castDist = CalculateEquivalentTravelDistanceFromDirection(direction) + .005f;
                    
                  //  RaycastHit2D reverseHit = Physics2D.Linecast(currentPosition +
                   //                                              direction * castDist, currentPosition);
                   RaycastHit2D reverseHit = Physics2D.Linecast(raycastEnd + direction * .001f, currentPosition);
                   
                    //GizmoUtilities.DrawXAtPos(reverseHit.point, .125f, Color.yellow);
                    exitPoint = reverseHit.point;
                    exitTime = simTime + (exitPoint.Value - currentPosition).magnitude / m_speed;
                }
                
                if (hit.collider == null)
                {
                    if (cellInsideOf != null && exitPoint != null) // keep second condition incase
                        events.Add(new ProjectileEvent(ProjectileEventType.EndPierce, exitPoint.Value, exitTime));
                    
                    events.Add(new ProjectileEvent(ProjectileEventType.Termination, 
                        currentPosition + direction * maxDistRemaining, maxDistRemaining / m_speed));
                    break;
                }
                
                distanceTraveled += hit.distance;
                currentPosition = hit.point;
                
                simTime += hit.distance / m_speed;
                Cell hitCell = Building.building.GetWallAtPos(hit.point - hit.normal * c_cell_check_cast_dist);
                float newPierceHealth = pierceHealth;//pierceHealth - hitCell.pierceInfluence;

                if (cellInsideOf != null && exitPoint != null && Vector2.Distance(exitPoint.Value, currentPosition) > .005f)
                {
                    // keep second condition incase
                    events.Add(new ProjectileEvent(ProjectileEventType.EndPierce, exitPoint.Value, exitTime));
                    cellInsideOf = null;
                }

                if (newPierceHealth >= 0f)
                {
                    if (cellInsideOf == null)
                        events.Add(new ProjectileEvent(ProjectileEventType.StartPierce, currentPosition, simTime));
                    cellInsideOf = hitCell;
                }
            }

            return events;
        }

        private static float CalculateEquivalentTravelDistanceFromDirection(Vector2 direction)
        {
            float x = direction.x;
            float y = direction.y;

            float slope = Mathf.Abs(x) > Mathf.Abs(y) ? y / x : x / y;
            return Mathf.Sqrt(slope * slope + 1);
        }

        public static void SpawnProjectile(Vector2 position, Vector2 direction, float speed)
        {
            GameObject newProj = Object.Instantiate(ContentUtilities.GetCachedAsset<GameObject>(PrefabAssetGroup.BASIC_PROJECTILE));
            newProj.transform.right = direction;
            newProj.transform.position = position;
            newProj.GetComponent<ProjectileSim>().m_speed = speed;
        }
    }
}