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
        private IProjectileHittable m_hittableInside;
        private Collider2D m_colliderInside;
        private SurfaceType m_surfaceTypeInside;
        private Vector2 m_lastTransformPos;
        private Vector2 m_lastEventLocation;
        
        private const float c_default_speed = 35f;
        private const float c_max_travel_dist = 150f;

        private void Awake()
        {
            m_building = ModuleUtilities.Get<GameModule>().Building;
        }

        private void LateUpdate()
        {
           // if (m_queuedEvents.Count == 0) return;
            
            if (m_currentEvent != null && Time.time >= m_currentEvent.time)
            {
                if (HandleEventAndReturnDestroyState()) return;
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
                //Debug.Log($"{m_currentEventLength} = {m_currentEvent.time} - {m_currentEventStartTime}");
                if (m_currentEvent.time < Time.time)
                {
                    //Debug.Log("handling old event");
                    if (HandleEventAndReturnDestroyState()) return;
                }
            }

            if (m_currentEvent == null) return;
            
            float progress = (Time.time - m_currentEventStartTime) / m_currentEventLength;
            transform.position = Vector2.Lerp(m_lastEventLocation, m_currentEvent.location, progress);
            //Debug.Log(Vector2.Distance(transform.position, m_lastTransformPos));
            m_lastTransformPos = transform.position;
        }

        /// <summary>
        /// Returns whether or not the object was destroyed
        /// </summary>
        /// <returns></returns>
        private bool HandleEventAndReturnDestroyState()
        {
            if (m_currentEvent.time > m_currentEventStartTime)
                m_currentEventStartTime = m_currentEvent.time;
            
            m_lastEventLocation = m_currentEvent.location;

            Color debugColor = m_currentEvent.terminate ? Color.black : Color.white;
            HitEvent hitEvent = m_currentEvent as HitEvent;
            switch (m_currentEvent.type)
            { 
                case ProjectileEventType.Position:
                    break;
                case ProjectileEventType.StartPierce:
                    debugColor = Color.red;
                    SpawnPierceParticleSystem(hitEvent.surfaceType, 
                        hitEvent.location + -hitEvent.travelDir * .005f, -hitEvent.travelDir);
                    break;
                case ProjectileEventType.EndPierce:
                    debugColor = Color.green;
                    SpawnPierceParticleSystem(hitEvent.surfaceType, 
                        hitEvent.location + hitEvent.travelDir * .005f, hitEvent.travelDir);
                    break;
                case ProjectileEventType.Ricochet:
                    debugColor = Color.cyan;
                    break;
            }

            GizmoUtilities.DrawXAtPos(m_currentEvent.location, .125f, debugColor);
            if (m_currentEvent.terminate)
            {
                Destroy(gameObject);
                return true;
            }

            m_currentEvent = null;
            return false;
        }

        private void SpawnPierceParticleSystem(SurfaceType type, Vector2 position, Vector2 direction)
        {
            string particleSystemName;
            if (type == SurfaceType.Glass)
                particleSystemName = PrefabAssetGroup.GLASS_PIERCE_PARTICLE_SYSTEM;
            else
                particleSystemName = PrefabAssetGroup.FLESH_PIERCE_PARTICLE_SYSTEM;
            GameObject pierceParticles = ContentUtilities.Instantiate<GameObject>(particleSystemName);
            pierceParticles.transform.position = position;
            pierceParticles.transform.right = direction;
        }

        private void FixedUpdate()
        {
            float travelDist = c_default_speed * Time.fixedDeltaTime;
            travelDist = Mathf.Min(travelDist, c_max_travel_dist - m_distanceTraveled);
            float time = Time.fixedTime;

            float remainingTravelDist = travelDist;
            //GizmoUtilities.DrawXAtPos(m_currentPosition, .25f, Color.magenta, Time.fixedDeltaTime);

            m_currentEventStartTime = Time.fixedTime;
            m_lastEventLocation = m_currentPosition;
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

                if (m_hittableInside != null)
                {
                    Vector2 reverseCastSource = m_currentPosition + m_travelDir * remainingTravelDist, 
                        reverseCastEnd = m_currentPosition;
                    RaycastHit2D reverseHit = Physics2D.Linecast(reverseCastSource, reverseCastEnd);
                    //if (hit.collider != null)
                    //    Debug.Log($"dist: {reverseHit.collider.Distance(hit.collider)}");
                    Debug.DrawLine(reverseCastSource, reverseCastEnd, Color.yellow);
                    if (reverseHit.collider != null && reverseHit.collider == m_colliderInside)
                    {
                        UpdateCurrentPosition(reverseHit.point + m_travelDir * .003f);
                        float distFromLastToHit = Vector2.Distance(m_lastPosition, m_currentPosition);
                        m_distanceTraveled += distFromLastToHit;
                        remainingTravelDist -= distFromLastToHit;
                        time += CalcInterpolationTimeFromLastAndCurrentPos();
                        m_queuedEvents.Enqueue(
                            new HitEvent(ProjectileEventType.EndPierce, reverseHit.point, 
                               time, m_hittableInside, m_surfaceTypeInside, -reverseHit.normal, m_travelDir));
                        SetObjectInside(null, null);
                        continue;
                    }
                }
                if (hit)
                {
                    if (ProcessHitAndReturnDestroyState(hit, ref remainingTravelDist, ref time)) break;
                    continue;
                }

                Vector2 offset = m_travelDir * remainingTravelDist;
                m_distanceTraveled += remainingTravelDist;
                remainingTravelDist = 0f;
                UpdateCurrentPosition(m_currentPosition + offset);
                time += CalcInterpolationTimeFromLastAndCurrentPos();
                
                m_queuedEvents.Enqueue(
                    new ProjectileEvent(ProjectileEventType.Position, m_currentPosition, time,
                        m_travelDir, m_distanceTraveled >= c_max_travel_dist));
                
            } while (remainingTravelDist > 0f);
        }

        private void UpdateCurrentPosition(Vector2 newPos)
        {
            m_lastPosition = m_currentPosition;
            m_currentPosition = newPos;
        }

        private void SetObjectInside(IProjectileHittable hittable, Collider2D col, SurfaceType type = SurfaceType.Concrete)
        {
            m_hittableInside = hittable;
            m_colliderInside = col;
            m_surfaceTypeInside = type;
        }

        private bool ProcessHitAndReturnDestroyState(RaycastHit2D hit, ref float remainingTravelDist, ref float time)
        {
            UpdateCurrentPosition(hit.point + m_travelDir * .003f);
            remainingTravelDist -= hit.distance;
            m_distanceTraveled += hit.distance;
            time += CalcInterpolationTimeFromLastAndCurrentPos();

            if (hit.collider.TryGetComponent(out TilemapCollider2D collider))
            {
                Vector2 cellSamplePoint = hit.point - hit.normal * .003f;
                Wall hitWall = m_building.GetWallAtPos(cellSamplePoint);
                if (hitWall.type == WallType.Glass)
                {
                    SetObjectInside(hitWall, hit.collider, SurfaceType.Glass);
                    
                    m_queuedEvents.Enqueue(new HitEvent(ProjectileEventType.StartPierce, hit.point, 
                        time, hitWall, SurfaceType.Glass, hit.normal, m_travelDir));

                    return false;
                }
                
                m_queuedEvents.Enqueue(new HitEvent(ProjectileEventType.Position, hit.point, time,
                    hitWall, SurfaceType.Concrete, hit.normal, m_travelDir, true));
                return true;
            }

            if (hit.collider.TryGetComponent(out CharacterObject characterObject))
            {
                SetObjectInside(characterObject.Character, hit.collider, SurfaceType.Flesh);
                m_queuedEvents.Enqueue(new HitEvent(ProjectileEventType.StartPierce, hit.point, 
                    time, characterObject.Character, SurfaceType.Flesh, hit.normal, m_travelDir));

                return false;
            }
            
            Debug.LogWarning("Projectile hit unimplemented target.");
            m_queuedEvents.Enqueue(new ProjectileEvent(ProjectileEventType.Position, hit.point, time, m_travelDir));
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