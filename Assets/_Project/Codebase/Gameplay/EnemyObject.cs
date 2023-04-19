using UnityEngine;

namespace _Project.Codebase.Gameplay
{
    public class EnemyObject : MonoBehaviour, IInititializer<EnemyCharacter>
    {
        public EnemyCharacter Enemy { get; private set; }
        public EnemyCharacter Initialize()
        {
            return Enemy = new EnemyCharacter(GetComponent<NavmeshAgent>());
        }
    }
}