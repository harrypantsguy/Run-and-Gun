using System.Collections.Generic;
using _Project.Codebase.Gameplay.Projectile;
using UnityEngine;

namespace _Project.Codebase.Gameplay
{
    public class ProjectileSim
    {
        private float m_speed;
        private int m_pierceStrength;

        private const float c_cell_check_cast_dist = .03f;
        private const float c_default_speed = 150f;
        private const float c_max_travel_dist = 200f;
            
        public ProjectileSim()
        {
            m_speed = c_default_speed;
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
            bool lastCastHitSurface = false;
            bool piercing = false;

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
                //if (cellInsideOf != null)
                //    castDistance = Mathf.Max(castDistance, CalculateEquivalentTravelDistanceFromDirection(direction) + .01f);

                Vector2 raycastSource = currentPosition;
                if (lastCastHitSurface)
                    raycastSource += direction * .001f;
                RaycastHit2D hit = Physics2D.Raycast(raycastSource, direction, castDistance);
                
                Vector2 raycastEnd = hit.collider == null ? currentPosition + direction * castDistance : hit.point;

                //Debug.DrawLine(currentPosition, raycastEnd, Color.red);
                //GizmoUtilities.DrawXAtPos(raycastEnd, .25f);

                RaycastHit2D exitHit = new RaycastHit2D();
                float exitTime = 0f;
                if (piercing)
                {
                    exitHit = GetExitRaycast(direction, raycastEnd, currentPosition, simTime, out exitTime);
                }
                
                if (hit.collider == null)
                {
                    if (piercing)
                        events.Add(new ProjectileEvent(ProjectileEventType.EndPierce, exitHit.point, exitHit.normal, exitTime));

                    events.Add(new ProjectileEvent(ProjectileEventType.Termination, 
                    currentPosition + direction * maxDistRemaining, simTime + maxDistRemaining / m_speed));
                    break;
                }

                lastCastHitSurface = true;
                distanceTraveled += hit.distance;
                currentPosition = hit.point;
                
                simTime += hit.distance / m_speed;
                Vector2 cellSamplePoint = hit.point - hit.normal * c_cell_check_cast_dist;
                Cell hitCell = Building.building.GetWallAtPos(cellSamplePoint);

                if (cellInsideOf != null && Vector2.Distance(exitHit.point, currentPosition) > .005f)
                {
                    piercing = false;
                    events.Add(new ProjectileEvent(ProjectileEventType.EndPierce, exitHit.point, exitTime));
                    cellInsideOf = null;
                }

                if (hitCell == null)
                {
                    Debug.Log("hit cell is null");
                    GizmoUtilities.DrawXAtPos(hit.point, .1f, Color.yellow);
                    GizmoUtilities.DrawXAtPos(cellSamplePoint, .1f, Color.red);
                    Debug.Break();
                    break;
                }

                if (hitCell.pierceInfluence > 0)
                {
                    events.Add(new ProjectileEvent(ProjectileEventType.Ricochet, currentPosition, hit.normal, simTime));
                    direction = Vector2.Reflect(direction, hit.normal);
                    continue;
                }
                
                if (!piercing)
                    events.Add(new ProjectileEvent(ProjectileEventType.StartPierce, currentPosition, hit.normal, simTime));
                piercing = true;

                cellInsideOf = hitCell;
            }

            return events;
        }

        private RaycastHit2D GetExitRaycast(Vector2 direction, Vector2 raycastEnd, Vector2 currentPosition, float simTime, out float exitTime)
        {
            Vector2? exitPoint;
            RaycastHit2D reverseHit = Physics2D.Linecast(raycastEnd + direction * .001f, currentPosition);

            //GizmoUtilities.DrawXAtPos(reverseHit.point, .125f, Color.yellow);
            exitPoint = reverseHit.point;
            exitTime = simTime + (exitPoint.Value - currentPosition).magnitude / m_speed;
            return reverseHit;
        }

        private static float CalculateEquivalentTravelDistanceFromDirection(Vector2 direction)
        {
            float x = direction.x;
            float y = direction.y;

            float slope = Mathf.Abs(x) > Mathf.Abs(y) ? y / x : x / y;
            return Mathf.Sqrt(slope * slope + 1);
        }
    }
}