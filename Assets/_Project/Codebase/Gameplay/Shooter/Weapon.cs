using System.Collections.Generic;
using _Project.Codebase.Gameplay.Projectiles;
using UnityEngine;

namespace _Project.Codebase.Gameplay.Shooter
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private Transform m_barrelTipTransform;

        private float m_lastFireTime;
        private List<ProjectileEvent> m_events = new();

        public void Fire()
        {
            Projectile.SpawnProjectile(transform.position, transform.right);
        }
    }
}