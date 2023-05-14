using System.Collections.Generic;
using _Project.Codebase.Gameplay.Projectiles;
using UnityEngine;

namespace _Project.Codebase.Gameplay.Shooter
{
    public class Weapon
    {
        public bool IsProjectileActive => m_projectiles.Count > 0;
        private readonly List<Projectile> m_projectiles = new();
        private readonly int m_maxBounces;

        public Weapon(int maxProjectileBounces = -1)
        {
            m_maxBounces = maxProjectileBounces;
        }

        public void Fire(Vector2 position, Vector2 direction)
        {
            Projectile newProjectile = Projectile.SpawnProjectile(position, direction, 25, m_maxBounces);
            m_projectiles.Add(newProjectile);
            newProjectile.OnDestroyProjectile += (projectile => m_projectiles.Remove(projectile));
        }
    }
}