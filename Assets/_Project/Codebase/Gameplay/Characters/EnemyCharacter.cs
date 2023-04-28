using System.Threading.Tasks;
using _Project.Codebase.Gameplay.AI;
using _Project.Codebase.Gameplay.Player;
using _Project.Codebase.Gameplay.World;
using UnityEngine;

namespace _Project.Codebase.Gameplay.Characters
{
    public class EnemyCharacter : Character
    {
        private readonly AIController m_AIController;
        public override PlayerSelectableType SelectableType => PlayerSelectableType.Enemy;

        public EnemyCharacter(Vector2Int position, NavmeshAgent agent, CharacterRenderer renderer) : base(position, agent, renderer)
        {
            m_AIController = new GruntAIController(this);
        }

        public async Task TakeTurn(WorldScreenshot worldContext)
        {
            await m_AIController.TakeTurn(worldContext);
        }
    }
}