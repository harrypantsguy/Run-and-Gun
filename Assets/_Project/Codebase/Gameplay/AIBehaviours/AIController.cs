using _Project.Codebase.Gameplay.World;

namespace _Project.Codebase.Gameplay.AIBehaviours
{
    public abstract class AIController
    {
        public AIBehavior AIBehavior { get; private set; }
        public Character Character { get; }

        protected AIController(Character character)
        {
            Character = character;
        }
        
        public void SetBehaviour(AIBehavior behavior)
        {
            AIBehavior?.OnExit();
            
            AIBehavior = behavior;
            if (behavior == null) return;

            AIBehavior.OnEnter();
        }

        public AIDecision Decide(WorldScreenshot worldScreenshot)
        {
            return AIBehavior?.MakeDecision(worldScreenshot);
        }
    }
}