using System;
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
        private float m_interpolationTime;
        private bool m_queueToDestroy;
        private float m_destroyTime;
        private Building m_building;
        
        private const float c_default_speed = 35f;
        private const float c_max_travel_dist = 150f;

        private void Awake()
        {
            m_building = ModuleUtilities.Get<GameModule>().Building;
        }

        private void LateUpdate()
        {
            float progress = (Time.time - Time.fixedTime) / m_interpolationTime;
            transform.position = Vector2.Lerp(m_lastPosition, m_currentPosition, progress);
            if (m_queueToDestroy && Time.time >= m_destroyTime)
                Destroy(gameObject);
        }

        private void FixedUpdate()
        {
            m_lastPosition = m_currentPosition;
            float travelDist = c_default_speed * Time.fixedDeltaTime;
            travelDist = Mathf.Min(travelDist, c_max_travel_dist - m_distanceTraveled);
            
            RaycastHit2D hit = Physics2D.Raycast(m_currentPosition, m_travelDir, travelDist);
            
            if (hit)
            {
                if (ProcessHitAndReturnDestroyState(hit)) return;
            }
            
            Vector2 offset = m_travelDir * travelDist;
            m_distanceTraveled += travelDist;
            UpdateInterpolationTime();
            m_currentPosition += offset;
            if (m_distanceTraveled >= c_max_travel_dist)
                QueueToDestroyAtCurrentPosition();
        }

        private bool ProcessHitAndReturnDestroyState(RaycastHit2D hit)
        {
            if (hit.collider.TryGetComponent(out TilemapCollider2D collider))
            {
                Vector2 cellSamplePoint = hit.point - hit.normal * .003f;
                Wall hitWall = m_building.GetWallAtPos(cellSamplePoint);
                if (hitWall.type == WallType.Glass)
                {
                    return false;
                }
            }
            else if (hit.collider.TryGetComponent(out Character character))
            {
                return false;
            }
            
            m_currentPosition = hit.point;
            UpdateInterpolationTime();
            QueueToDestroyAtCurrentPosition();
            return true;
        }

        private void UpdateInterpolationTime() => 
            m_interpolationTime = Vector2.Distance(m_currentPosition, m_lastPosition) / c_default_speed;
        
        private void QueueToDestroyAtCurrentPosition()
        {
            m_queueToDestroy = true;
            m_destroyTime = Time.time + m_interpolationTime;
        }

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