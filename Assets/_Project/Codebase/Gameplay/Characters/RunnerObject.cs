using UnityEngine;

namespace _Project.Codebase.Gameplay.Characters
{
    public class RunnerObject : CharacterObject
    {
        [SerializeField] private int m_maxHealth;
        public override Character Initialize(Vector2Int position)
        {
            Character = new Runner(position, GetComponent<NavmeshAgent>(), characterRenderer, m_maxHealth);
            base.Initialize(position);
            return Character;
        }
    }
}