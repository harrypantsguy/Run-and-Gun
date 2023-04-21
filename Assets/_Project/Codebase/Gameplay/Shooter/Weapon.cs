using System.Collections.Generic;
using _Project.Codebase.Gameplay.Projectiles;
using UnityEngine;

namespace _Project.Codebase.Gameplay.Shooter
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private Transform m_barrelTipTransform;

        private float m_lastFireTime;
        private ProjectileSim m_projectileSim;
        private List<ProjectileEvent> m_events = new();
        private ProjectileSimReenactor m_reenactor;

        private void Start()
        {
            m_projectileSim = new ProjectileSim();
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying) return;
            return;

            Vector2 lastPos = Vector2.zero;
            for (var i = 0; i < m_events.Count; i++)
            {
                var projEvent = m_events[i];
                
                float radius = .25f;
                Color pointColor = Color.white;
                Color lineColor = Color.white;
                switch (projEvent.type)
                {
                    case ProjectileEventType.Position:
                        break;
                    case ProjectileEventType.StartPierce:
                        pointColor = Color.green;
                        break;
                    case ProjectileEventType.EndPierce:
                        pointColor = Color.magenta;
                        lineColor = Color.red;
                        break;
                    case ProjectileEventType.Ricochet:
                        pointColor = Color.cyan;
                        break;
                    case ProjectileEventType.Termination:
                        pointColor = Color.red;
                        break;
                }

                Gizmos.color = pointColor;
                Gizmos.DrawWireSphere(projEvent.location, radius);

                if (i != 0)
                {
                    Gizmos.color = lineColor;
                    Gizmos.DrawLine(lastPos, projEvent.location);
                }

                lastPos = projEvent.location;
            }
        }

        public void Fire()
        {
            Projectile.SpawnProjectile(transform.position, transform.right);
        }
    }
}