using UnityEngine;

namespace _Project.Codebase.Gameplay.Characters
{
    public class EnemyObject : CharacterObject
    {
        public override Character Initialize(Vector2Int position)
        {
            return Character = new EnemyCharacter(position, GetComponent<NavmeshAgent>(), GetComponent<CharacterRenderer>());
        }
    }
}