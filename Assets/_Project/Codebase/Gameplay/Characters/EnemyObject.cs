using UnityEngine;

namespace _Project.Codebase.Gameplay.Characters
{
    public class EnemyObject : CharacterObject
    {
        public EnemyCharacter Enemy { get; private set; }
        public EnemyCharacter Initialize(Vector2Int position)
        {
            return Enemy = new EnemyCharacter(GetComponent<NavmeshAgent>(), position);
        }

        public override Character Character => Enemy;
    }
}