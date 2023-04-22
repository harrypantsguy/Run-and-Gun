using System.Collections.Generic;
using _Project.Codebase.Gameplay.World;
using _Project.Codebase.Modules;
using DanonFramework.Runtime.Core.Utilities;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace _Project.Codebase.Gameplay.Projectiles
{
    public class Projectile : MonoBehaviour
    {
        private Vector2 m_currentPosition;
        private Vector2 m_lastPosition;
        private float m_distanceTraveled;
        private Vector2 m_travelDir;
        private Building m_building;
        private float m_currentEventStartTime;
        private float m_currentEventLength;
        private readonly Queue<ProjectileEvent> m_queuedEvents = new();
        private ProjectileEvent m_currentEvent;
        private bool m_insideCollider;

        private const float c_default_speed = 35f;
        private const float c_max_travel_dist = 150f;

        private void Awake()
        {
            m_building = ModuleUtilities.Get<GameModule>().Building;
        }

        private void LateUpdate()
        {
            if (m_queuedEvents.Count == 0) return;
            
            if (m_currentEvent != null && Time.time >= m_currentEvent.time)
            {
                if (HandleEventOccurAndReturnDestroyState()) return;
            }

            int loops = 0;
            while (m_currentEvent == null && m_queuedEvents.TryDequeue(out m_currentEvent))
            {
                loops++;
                if (loops >= 30)
                {
                    Debug.LogWarning("late update infinite loop");
                    return;
                }
                    
                m_currentEventLength = m_currentEvent.time - m_currentEventStartTime;
                if (m_currentEvent.time < Time.time)
                {
                    if (HandleEventOccurAndReturnDestroyState()) return;
                }
            }
            
            float progress = (Time.time - m_currentEventStartTime) / m_currentEventLength;
            transform.position = Vector2.Lerp(m_lastPosition, m_currentPosition, progress);
        }

        /// <summary>
        /// Returns whether or not the object as destroyed
        /// </summary>
        /// <returns></returns>
        private bool HandleEventOccurAndReturnDestroyState()
        {
            if (m_currentEvent.time > m_currentEventStartTime)
                m_currentEventStartTime = m_currentEvent.time;
            
            switch (m_currentEvent.type)
            {
                case ProjectileEventType.Position:
                    break;
                case ProjectileEventType.StartPierce:
                    
                    break;
                case ProjectileEventType.EndPierce:
                    break;
                case ProjectileEventType.Ricochet:
                    break;
            }

            if (m_currentEvent.terminate)
            {
                Destroy(gameObject);
                return true;
            }

            m_currentEvent = null;
            return false;
        }

        private void FixedUpdate()
        {
            m_lastPosition = m_currentPosition;
            float travelDist = c_default_speed * Time.fixedDeltaTime;
            travelDist = Mathf.Min(travelDist, c_max_travel_dist - m_distanceTraveled);
            float time = Time.fixedTime;

            float remainingTravelDist = travelDist;
            int loops = 0;
            do
            {
                loops++;
                if (loops >= 30)
                {
                    Debug.LogWarning("fixed update infinite loop");
                    return;
                }
                RaycastHit2D hit = Physics2D.Raycast(m_currentPosition, m_travelDir, remainingTravelDist);

                if (hit)
                    if (ProcessHitAndReturnDestroyState(hit, ref remainingTravelDist, ref time)) break;
                
                Vector2 offset = m_travelDir * remainingTravelDist;
                m_distanceTraveled += remainingTravelDist;
                remainingTravelDist = 0f;
                UpdateCurrentPosition(m_currentPosition + offset);
                time += CalcInterpolationTime(m_lastPosition, m_currentPosition);

                m_queuedEvents.Enqueue(
                    new ProjectileEvent(ProjectileEventType.Position, m_currentPosition, time,
                        m_distanceTraveled >= c_max_travel_dist));
                
            } while (remainingTravelDist > 0f);

            m_currentEventStartTime = Time.fixedTime;
        }

        private void UpdateCurrentPosition(Vector2 newPos)
        {
            m_lastPosition = m_currentPosition;
            m_currentPosition = newPos;
        }

        private bool ProcessHitAndReturnDestroyState(RaycastHit2D hit, ref float remainingTravelDist, ref float time)
        {
            UpdateCurrentPosition(hit.point + m_travelDir * .003f);
            remainingTravelDist -= hit.distance;
            time += CalcInterpolationTimeFromLastAndCurrentPos();
            
            if (hit.collider.TryGetComponent(out TilemapCollider2D collider))
            {
                Vector2 cellSamplePoint = hit.point - hit.normal * .003f;
                Wall hitWall = m_building.GetWallAtPos(cellSamplePoint);
                if (hitWall.type == WallType.Glass)
                {
                    m_queuedEvents.Enqueue(new HitEvent(ProjectileEventType.StartPierce, hit.point, 
                        time, hitWall, SurfaceType.Glass));
                    return false;
                }
                
                m_queuedEvents.Enqueue(new HitEvent(ProjectileEventType.Position, hit.point, time,
                    hitWall, SurfaceType.Concrete, true));
                return true;
            }

            if (hit.collider.TryGetComponent(out CharacterObject characterObject))
            {
                m_queuedEvents.Enqueue(new HitEvent(ProjectileEventType.StartPierce, hit.point, 
                    time, characterObject.Character, SurfaceType.Flesh));
                return false;
            }
            
            Debug.LogWarning("Projectile hit unimplemented target.");
            m_queuedEvents.Enqueue(new ProjectileEvent(ProjectileEventType.Position, hit.point, time));
            return false;
        }

        private float CalcInterpolationTimeFromLastAndCurrentPos(float speed = c_default_speed) =>
            CalcInterpolationTime(m_lastPosition, m_currentPosition, speed);

        private float CalcInterpolationTime(Vector2 pos1, Vector2 pos2, float speed = c_default_speed) => 
            Vector2.Distance(pos1, pos2) / speed;

        public static void SpawnProjectile(Vector2 pos, Vector2 direction)
        {
            GameObject newProjectileObj = ContentUtilities.Instantiate<GameObject>(PrefabAssetGroup.BASIC_PROJECTILE);
            newProjectileObj.transform.position = pos;
            newProjectileObj.transform.right = direction;
            Projectile projectile = newProjectileObj.GetComponent<Projectile>();
            projectile.m_travelDir = direction;
            projectile.m_currentPosition = pos;
        }
    }
}