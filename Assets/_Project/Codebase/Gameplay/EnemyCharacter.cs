using _Project.Codebase.Gameplay.AIBehaviours;

namespace _Project.Codebase.Gameplay
{
    public class EnemyCharacter : Character
    {
        private AIController m_AIController;
        
        protected override void Start()
        {
            base.Start();

            m_AIController = new GruntAIController(this);
        }
    }
}