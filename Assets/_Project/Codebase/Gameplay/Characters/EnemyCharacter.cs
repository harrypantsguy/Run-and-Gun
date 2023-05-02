using _Project.Codebase.Gameplay.AI;
using _Project.Codebase.Gameplay.Player;
using _Project.Codebase.Gameplay.Shooter;
using _Project.Codebase.Gameplay.World;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Codebase.Gameplay.Characters
{
    public class EnemyCharacter : Character
    {
        private readonly AIController m_AIController;
        public readonly float firingRange;
        public override PlayerSelectableType SelectableType => PlayerSelectableType.Enemy;
        private readonly Transform m_projectileSpawnPos;
        private readonly Weapon m_weapon;

        public EnemyCharacter(Vector2Int position, NavmeshAgent agent, CharacterRenderer renderer, int maxHealth, 
            float firingRange, Transform projectileSpawnPos)
            : base(position, agent, renderer, maxHealth)
        {
            m_AIController = new GuardAIController(this);
            this.firingRange = firingRange;
            m_projectileSpawnPos = projectileSpawnPos;
            m_weapon = new Weapon();
        }

        public async UniTask TakeTurn(WorldScreenshot worldContext)
        {
            await m_AIController.TakeTurn(worldContext);
        }

        public async UniTask FireGun(Vector2 target)
        {
            m_weapon.Fire(m_projectileSpawnPos.position, (target - (Vector2)m_projectileSpawnPos.position).normalized);
            await UniTask.WaitWhile(() => m_weapon.IsProjectileActive);
        }
    }
}