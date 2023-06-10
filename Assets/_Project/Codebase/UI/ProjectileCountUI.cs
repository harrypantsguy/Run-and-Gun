using System.Collections.Generic;
using _Project.Codebase.Gameplay.Shooter;
using _Project.Codebase.Modules;
using DanonFramework.Core;
using UnityEngine;

namespace _Project.Codebase.UI
{
    public class ProjectileCountUI : MonoBehaviour
    {
        [SerializeField] private GameObject m_projectilePrefab;

        private readonly List<GameObject> m_projectiles = new();
        private ShooterController m_shooterController;
        
        private void Start()
        {
            m_shooterController = ModuleUtilities.Get<GameModule>().PlayerManager.ShooterController;
        }

        private void Update()
        {
            for (int i = m_projectiles.Count - 1; i >= m_shooterController.ShotsRemaining; i--)
            {
                Destroy(m_projectiles[i].gameObject);
                m_projectiles.RemoveAt(i);
            }
            
            for (int i = m_projectiles.Count; i < m_shooterController.ShotsRemaining; i++)
            {
                m_projectiles.Add(Instantiate(m_projectilePrefab, transform));
            }
        }
    }
}