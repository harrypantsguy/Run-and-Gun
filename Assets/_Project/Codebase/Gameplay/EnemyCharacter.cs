using System.Collections.Generic;
using _Project.Codebase.Gameplay.AIBehaviours;
using _Project.Codebase.Gameplay.World;
using _Project.Codebase.Modules;
using _Project.Codebase.NavigationMesh;
using DanonFramework.Runtime.Core.Utilities;

namespace _Project.Codebase.Gameplay
{
    public class EnemyCharacter : Character
    {
        private AIController m_AIController;
        private Navmesh m_navmesh;

        private List<AIDecision> m_turnDecisions = new();
        
        private const int c_default_action_points = 2;
        
        protected override void Start()
        {
            base.Start();

            m_AIController = new GruntAIController(this);
            m_navmesh = ModuleUtilities.Get<GameModule>().Navmesh;
        }

        public void DecideTurn(ref WorldScreenshot world)
        {
            for (int i = 0; i < c_default_action_points; i++)
            {
                m_AIController.Update(ref world);
            }
        }
    }
}