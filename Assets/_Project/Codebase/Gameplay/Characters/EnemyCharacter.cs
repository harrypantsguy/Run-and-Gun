using _Project.Codebase.Gameplay.AI;
using _Project.Codebase.Gameplay.Player;
using _Project.Codebase.Gameplay.World;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Codebase.Gameplay.Characters
{
    public class EnemyCharacter : Character
    {
        private readonly AIController m_AIController;
        public override PlayerSelectableType SelectableType => PlayerSelectableType.Enemy;

        public EnemyCharacter(Vector2Int position, NavmeshAgent agent, CharacterRenderer renderer, int maxHealth)
            : base(position, agent, renderer, maxHealth)
        {
            m_AIController = new GruntAIController(this);
        }

        public async UniTask TakeTurn(WorldScreenshot worldContext)
        {
            await m_AIController.TakeTurn(worldContext);
        }
    }
}