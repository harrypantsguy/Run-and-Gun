using UnityEngine;

namespace _Project.Codebase.Gameplay.Characters
{
    public class EnemyObject : CharacterObject
    {
        [SerializeField] private int m_maxHealth;
        public override Character Initialize(Vector2Int position)
        {
            return Character = new EnemyCharacter(position, GetComponent<NavmeshAgent>(), characterRenderer, 
                m_maxHealth);
        }
    }
}