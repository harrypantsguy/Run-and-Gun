﻿using System.Collections.Generic;
using _Project.Codebase.Gameplay.Projectile;
using _Project.Codebase.Gameplay.World;
using _Project.Codebase.Modules;
using DanonFramework.Runtime.Core.Utilities;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace _Project.Codebase.Gameplay
{
    public class ProjectileSim
    {
        private float m_speed;
        private int m_pierceStrength;
        
        private const float c_cell_check_cast_dist = .03f;
        private const float c_default_speed = 55f;
        private const float c_max_travel_dist = 400f;

        private WorldRegions m_worldRegions;

        private Building m_building;
            
        public ProjectileSim()
        {
            m_speed = c_default_speed;
            m_pierceStrength = 2;

            GameModule gameModule = ModuleUtilities.Get<GameModule>();
            m_building = gameModule.Building;
            m_worldRegions = gameModule.WorldRegions;
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
            SurfaceType lastEventSurfaceType = SurfaceType.Concrete;
            
            int its = 0;
            while (true)
            {
                its++;
                if (its == 500)
                {
                    Debug.LogWarning($"{nameof(ProjectileSim)}: stack overflow");
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

                RaycastHit2D exitHit = new RaycastHit2D();
                float exitTime = 0f;
                if (piercing)
                {
                    exitHit = GetExitRaycast(direction, raycastEnd, currentPosition, simTime, out exitTime);
                }
                
                if (hit.collider == null)
                {
                    if (piercing)
                        events.Add(new PierceEvent(ProjectileEventType.EndPierce, exitHit.point, exitHit.normal, exitTime, 
                            lastEventSurfaceType));

                    Vector2 offset = direction * maxDistRemaining;
                    Vector2 endPoint = currentPosition + offset;
                    GizmoUtilities.DrawXAtPos(endPoint, 1f, Color.red);
                    //if (!m_worldRegions.IsPointInsideRegion(endPoint, m_worldRegions.shooterRegionExtents))
                     //   endPoint = m_worldRegions.ProjectVectorOntoRegionEdge(currentPosition, 
                    //        offset, m_worldRegions.shooterRegionExtents);
                    events.Add(new ProjectileEvent(ProjectileEventType.Termination, 
                        endPoint, simTime + Vector2.Distance(endPoint, currentPosition) / m_speed));
                    break;
                }

                lastCastHitSurface = true;
                distanceTraveled += hit.distance;
                currentPosition = hit.point;
                
                simTime += hit.distance / m_speed;

                bool projHitWall = hit.collider.GetComponent<TilemapCollider2D>() != null;
                bool projHitCharacter = hit.collider.GetComponent<EnemyObject>() != null;

                Vector2 cellSamplePoint = hit.point - hit.normal * c_cell_check_cast_dist;
                Wall hitWall = m_building.GetWallAtPos(cellSamplePoint);

                SurfaceType surfaceType = SurfaceType.Concrete;
                if (projHitWall)
                    surfaceType = hitWall.type == WallType.Concrete ? SurfaceType.Concrete : SurfaceType.Glass;
                else if (projHitCharacter)
                    surfaceType = SurfaceType.Flesh;

                if (piercing && Vector2.Distance(exitHit.point, currentPosition) > .005f)
                {
                    piercing = false;
                    events.Add(new PierceEvent(ProjectileEventType.EndPierce, exitHit.point, exitTime,
                        lastEventSurfaceType));
                    cellInsideOf = null;
                }
                
                lastEventSurfaceType = surfaceType;

                if (projHitWall && hitWall == null)
                {
                    Debug.LogWarning("hit cell is null");
                    GizmoUtilities.DrawXAtPos(hit.point, .1f, Color.yellow);
                    GizmoUtilities.DrawXAtPos(cellSamplePoint, .1f, Color.red);
                    Debug.Break();
                    break;
                }

                if (projHitWall && hitWall.type != WallType.Glass)
                {
                    events.Add(new ProjectileEvent(ProjectileEventType.Ricochet, currentPosition, hit.normal, simTime));
                    direction = Vector2.Reflect(direction, hit.normal);
                    continue;
                }
                
                if (!piercing)
                    events.Add(new PierceEvent(ProjectileEventType.StartPierce, currentPosition, hit.normal, simTime,
                        surfaceType));
                piercing = true;

                cellInsideOf = hitWall;
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