using System.Collections.Generic;
using System.Threading.Tasks;
using _Project.Codebase.Gameplay.AIBehaviours;
using _Project.Codebase.Gameplay.World;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Codebase.Gameplay
{
    public class EnemyCharacter : Character
    {
        private AIController m_AIController;

        private readonly List<AIDecision> m_turnDecisions = new();
        
        private const int c_default_action_points = 2;

        public EnemyCharacter(NavmeshAgent agent) : base(agent)
        {
            m_AIController = new GruntAIController(this);
        }

        public void DecideTurn(WorldScreenshot world)
        {
            m_turnDecisions.Clear();
            for (int i = 0; i < c_default_action_points; i++)
            {
                m_turnDecisions.Add(m_AIController.Decide(world));
            }
        }
    }
}