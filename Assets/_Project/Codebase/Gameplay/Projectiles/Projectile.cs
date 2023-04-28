using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using _Project.Codebase.AssetGroups;
using _Project.Codebase.EditorUtilities;
using _Project.Codebase.Gameplay.Characters;
using _Project.Codebase.Gameplay.World;
using _Project.Codebase.Modules;
using DanonFramework.Runtime.Core.Utilities;
using Mono.CecilX.Cil;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace _Project.Codebase.Gameplay.Projectiles
{
    public class Projectile : MonoBehaviour
    {
        public event Action<Projectile> OnDestroyProjectile;
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
        private WorldRegions m_worldRegions;

        private const float c_default_speed = 40f;
        private const float c_max_travel_dist = 300f;
        private const float c_tick_rate = 1f / 50f;
        
        private void Initialize(Vector2 pos, Vector2 dir)
        {
            m_currentPosition = pos;
            m_travelDir = dir;
            var gameModule = ModuleUtilities.Get<GameModule>();
            m_building = gameModule.Building;
            m_worldRegions = gameModule.WorldRegions;
            StartCoroutine(Tick());
        }

        private void LateUpdate()
        {
            HandleInterpolationAndGraphics();
        }

        private void HandleInterpolationAndGraphics()
        {
            if (m_currentEvent != null)
            {
                Time.timeScale =
                    m_currentEvent.type is ProjectileEventType.StartPierce or ProjectileEventType.EndPierce
                    || m_hittableInside != null
                        ? .025f
                        : 1f;
            }

            if (m_currentEvent != null && Time.time >= m_currentEvent.time)
            {
                if (HandleEventAndReturnDestroyState()) return;
            }

            int loops = 0;
            while (m_currentEvent == null && m_queuedEvents.TryDequeue(out m_currentEvent))
            {
                loops++;
                if (loops >= 9999)
                {
                    Debug.LogWarning($"late update infinite loop, {m_currentEvent.type}, {m_currentEvent.terminate}");
                    return;
                }

                m_currentEventLength = m_currentEvent.time - m_currentEventStartTime;
                transform.right = m_currentEvent.travelDir;
                //Debug.Log($"{m_currentEventLength} = {m_currentEvent.time} - {m_currentEventStartTime}");
                if (m_currentEvent.time < Time.time)
                {
                    //Debug.Log("handling old event");
                    if (HandleEventAndReturnDestroyState()) return;
                }
            }

            if (m_currentEvent == null) return;

            if (m_currentEventLength == 0)
                transform.position = m_currentEvent.location;
            else
            {
                float progress = (Time.time - m_currentEventStartTime) / m_currentEventLength;
                transform.position = Vector2.Lerp(m_lastEventLocation, m_currentEvent.location, progress);
            }

            //Debug.Log(Vector2.Distance(transform.position, m_lastTransformPos));
            m_lastTransformPos = transform.position;
        }

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
                    debugColor = Color.green;
                    SpawnPierceParticleSystem(hitEvent.surfaceType, 
                        hitEvent.location + -hitEvent.travelDir * .005f, -hitEvent.travelDir);
                    break;
                case ProjectileEventType.EndPierce:
                    debugColor = Color.red;
                    SpawnPierceParticleSystem(hitEvent.surfaceType, 
                        hitEvent.location + hitEvent.travelDir * .005f, hitEvent.travelDir);
                    break;
                case ProjectileEventType.Ricochet:
                    debugColor = Color.cyan;
                    break;
            }

            GizmoUtilities.DrawXAtPos(m_currentEvent.location, .125f, debugColor, 1f);
            if (m_currentEvent.terminate)
            {
                Destroy(gameObject);
                OnDestroyProjectile?.Invoke(this);
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

        private void QueueEvents(float time, float deltaTime)
        {
            float travelDist = c_default_speed * deltaTime;
            travelDist = Mathf.Min(travelDist, c_max_travel_dist - m_distanceTraveled);

            float remainingTravelDist = travelDist;
            GizmoUtilities.DrawXAtPos(m_currentPosition, .25f, Color.magenta, 1f);

            m_currentEventStartTime = time;
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
                if (hit.collider != null)
                    GizmoUtilities.DrawXAtPos(hit.point, .1f, Color.gray, deltaTime);

                HandleEndingPierce(hit, deltaTime, ref remainingTravelDist, ref time);

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

                bool outsideScreenRegion = !m_worldRegions.IsPointInsideRegion(m_currentPosition, m_worldRegions.shooterRegionExtents);
                
                m_queuedEvents.Enqueue(
                    new ProjectileEvent(ProjectileEventType.Position, m_currentPosition, time,
                        m_travelDir, m_distanceTraveled >= c_max_travel_dist || outsideScreenRegion));
            } while (remainingTravelDist > 0f);
        }

        private void HandleEndingPierce(RaycastHit2D hit, float deltaTime, ref float remainingTravelDist, ref float time)
        {
            if (m_hittableInside != null)
            {
                Vector2 reverseCastSource = m_currentPosition + m_travelDir * remainingTravelDist,
                    reverseCastEnd = m_currentPosition;
                RaycastHit2D[] reverseHits = new RaycastHit2D[5];
                int numReverseHits = MathUtilities.LinecastTrulyAll(reverseCastSource, reverseCastEnd, reverseHits);

                Debug.DrawLine(reverseCastSource, reverseCastEnd, Color.yellow);
                if (numReverseHits > 0)
                {
                    RaycastHit2D reverseHit = reverseHits[numReverseHits - 1];
                    if (reverseHit.collider != null)
                        GizmoUtilities.DrawXAtPos(reverseHit.point, .075f, Color.yellow, deltaTime);

                    if (reverseHit.collider != null && reverseHit.collider == m_colliderInside &&
                        Vector2.Distance(reverseHit.point, hit.point) > .001f)
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
                    }
                }
            }
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
                    m_queuedEvents.Enqueue(new HitEvent(m_hittableInside != null
                            ? ProjectileEventType.Position : ProjectileEventType.StartPierce, hit.point, 
                        time, hitWall, SurfaceType.Glass, hit.normal, m_travelDir));
                    
                    SetObjectInside(hitWall, hit.collider, SurfaceType.Glass);

                    return false;
                }
                
                m_queuedEvents.Enqueue(new HitEvent(ProjectileEventType.Ricochet, hit.point, time,
                    hitWall, SurfaceType.Concrete, hit.normal, m_travelDir));
                m_travelDir = Vector2.Reflect(m_travelDir, hit.normal);
                return false;
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

        private IEnumerator Tick()
        {
            float t = 0f;
            float globalTime = Time.time;
            QueueEvents(globalTime,c_tick_rate);

            while (enabled)
            {
                t += Time.deltaTime;
                while (t >= c_tick_rate)
                {
                    t -= c_tick_rate;
                    QueueEvents(globalTime,c_tick_rate);
                }
                globalTime += Time.deltaTime;

                yield return null;
            }
        }

        private float CalcInterpolationTimeFromLastAndCurrentPos(float speed = c_default_speed) =>
            CalcInterpolationTime(m_lastPosition, m_currentPosition, speed);

        private float CalcInterpolationTime(Vector2 pos1, Vector2 pos2, float speed = c_default_speed) => 
            Vector2.Distance(pos1, pos2) / speed;

        public static Projectile SpawnProjectile(Vector2 pos, Vector2 direction)
        {
            GameObject newProjectileObj = ContentUtilities.Instantiate<GameObject>(PrefabAssetGroup.BASIC_PROJECTILE);
            newProjectileObj.transform.position = pos;
            newProjectileObj.transform.right = direction;
            Projectile projectile = newProjectileObj.GetComponent<Projectile>();
            projectile.Initialize(pos, direction);
            return projectile;
        }
    }
}