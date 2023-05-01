using UnityEngine;

namespace _Project.Codebase.Gameplay.Characters
{
    public class EnemyObject : CharacterObject
    {
        [SerializeField] private int m_maxHealth;
        [SerializeField] private float m_firingRange;
        [SerializeField] private Transform m_projectileSpawnPos;
        public override Character Initialize(Vector2Int position)
        { 
            return Character = new EnemyCharacter(position, GetComponent<NavmeshAgent>(), characterRenderer, 
                m_maxHealth, m_firingRange, m_projectileSpawnPos);
        }
    }
}