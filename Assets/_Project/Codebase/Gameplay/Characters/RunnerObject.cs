using UnityEngine;

namespace _Project.Codebase.Gameplay.Characters
{
    public class RunnerObject : CharacterObject
    {
        public override Character Initialize(Vector2Int position)
        {
            return Character = new Runner(position, GetComponent<NavmeshAgent>(), GetComponent<CharacterRenderer>());
        }
    }
}