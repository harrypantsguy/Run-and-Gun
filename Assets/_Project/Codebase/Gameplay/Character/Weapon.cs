using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Codebase.Gameplay.Character
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private Transform m_barrelTipTransform;
        [SerializeField] private float m_fireDelay;

        private float m_lastFireTime;
        private ProjectileSim m_projectileSim;
        private List<ProjectileEvent> m_events;

        private void Start()
        {
            m_projectileSim = new ProjectileSim();
        }

        private void Update()
        {
            m_events = m_projectileSim.Simulate(m_barrelTipTransform.position, m_barrelTipTransform.right);
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying) return;
            
            foreach (ProjectileEvent projEvent in m_events)
            {
                float radius = .5f;
                Color color = Color.white;
                switch (projEvent.type)
                {
                    case ProjectileEventType.Position:
                        break;
                    case ProjectileEventType.StartPierce:
                        color = Color.green;
                        break;
                    case ProjectileEventType.EndPierce:
                        color = Color.magenta;
                        break;
                    case ProjectileEventType.Ricochet:
                        break;
                    case ProjectileEventType.Termination:
                        color = Color.red;
                        break;
                }

                Gizmos.color = color;
                Gizmos.DrawWireSphere(projEvent.location, radius);
            }
        }

        public void Fire()
        {
            if (Time.time < m_lastFireTime + m_fireDelay) return;
            
            m_lastFireTime = Time.time;
            
            //ProjectileSim.SpawnProjectile(m_barrelTipTransform.position, m_barrelTipTransform.right, 
            //    ProjectileSim.DEFAULT_SPEED);
        }
    }
}