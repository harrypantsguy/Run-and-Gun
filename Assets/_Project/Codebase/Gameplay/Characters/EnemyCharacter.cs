using System.Threading.Tasks;
using _Project.Codebase.Gameplay.AI;
using _Project.Codebase.Gameplay.World;
using UnityEngine;

namespace _Project.Codebase.Gameplay.Characters
{
    public class EnemyCharacter : Character
    {
        private readonly AIController m_AIController;

        public EnemyCharacter(Vector2Int position, NavmeshAgent agent, CharacterRenderer renderer) : base(position, agent, renderer)
        {
            m_AIController = new GruntAIController(this);
        }

        public async Task TakeTurn(WorldScreenshot worldContext)
        {
            for (int i = 0; i < DEFAULT_MAX_ACTION_POINTS; i++)
            {
                await m_AIController.TakeTurn(worldContext);
            }
        }
    }
}