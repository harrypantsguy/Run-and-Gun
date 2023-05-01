using System.Collections.Generic;
using _Project.Codebase.Gameplay.Projectiles;
using UnityEngine;

namespace _Project.Codebase.Gameplay.Shooter
{
    public class Weapon
    {
        private readonly List<Projectile> m_projectiles = new();
        public bool IsProjectileActive => m_projectiles.Count > 0;

        public void Fire(Vector2 position, Vector2 direction)
        {
            Projectile newProjectile = Projectile.SpawnProjectile(position, direction, 25);
            m_projectiles.Add(newProjectile);
            newProjectile.OnDestroyProjectile += (projectile => m_projectiles.Remove(projectile));
        }
    }
}