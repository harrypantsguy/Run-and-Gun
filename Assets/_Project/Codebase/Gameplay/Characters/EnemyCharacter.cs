using System.Threading.Tasks;
using _Project.Codebase.Gameplay.AI;
using _Project.Codebase.Gameplay.World;
using UnityEngine;

namespace _Project.Codebase.Gameplay.Characters
{
    public class EnemyCharacter : Character
    {
        private AIController m_AIController;
        
        private const int c_default_action_points = 1;

        public EnemyCharacter(NavmeshAgent agent, Vector2Int position) : base(agent, position)
        {
            m_AIController = new GruntAIController(this);
        }

        public async Task TakeTurn(WorldScreenshot worldContext)
        {
            for (int i = 0; i < c_default_action_points; i++)
            {
                await m_AIController.TakeTurn(worldContext);
            }
        }
    }
}