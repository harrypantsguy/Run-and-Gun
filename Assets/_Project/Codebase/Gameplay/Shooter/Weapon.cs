using System.Collections.Generic;
using _Project.Codebase.Gameplay.Projectiles;
using UnityEngine;

namespace _Project.Codebase.Gameplay.Shooter
{
    public class Weapon : MonoBehaviour
    {
        public List<Projectile> Projectiles { get; private set; }
        private float m_lastFireTime;

        private void Awake()
        {
            Projectiles = new List<Projectile>();
        }

        public void Fire()
        {
            Projectile newProjectile = Projectile.SpawnProjectile(transform.position, transform.right);
            Projectiles.Add(newProjectile);
            newProjectile.OnDestroyProjectile += (projectile => Projectiles.Remove(projectile));
        }
    }
}