using UnityEngine;

namespace _Project.Codebase.Gameplay
{
    public class EnemyObject : MonoBehaviour
    {
        public EnemyCharacter Enemy { get; private set; }
        public EnemyCharacter Initialize(Vector2Int position)
        {
            return Enemy = new EnemyCharacter(GetComponent<NavmeshAgent>(), position);
        }
    }
}