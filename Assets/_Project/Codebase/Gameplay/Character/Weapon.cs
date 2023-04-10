using UnityEngine;

namespace _Project.Codebase.Gameplay.Character
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private Transform m_barrelTipTransform;
        [SerializeField] private float m_fireDelay;

        private float m_lastFireTime;
        
        public void Fire()
        {
            if (Time.time < m_lastFireTime + m_fireDelay) return;
            
            m_lastFireTime = Time.time;
            
            Projectile.SpawnProjectile(m_barrelTipTransform.position, m_barrelTipTransform.right, 
                Projectile.DEFAULT_SPEED);
        }
    }
}