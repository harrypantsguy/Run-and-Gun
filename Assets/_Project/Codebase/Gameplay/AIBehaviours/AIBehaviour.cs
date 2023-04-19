using _Project.Codebase.Gameplay.World;

namespace _Project.Codebase.Gameplay.AIBehaviours
{
    public abstract class AIBehavior
    {
        protected readonly AIController controller;

        public Character Character { get; }
        public AIBehavior(AIController controller)
        {
            this.controller = controller;
            Character = controller.Character;
        }

        public virtual void OnEnter() {}

        public virtual AIDecision MakeDecision(WorldScreenshot worldScreenshot)
        {
            return new MoveDecision();
        }

        public virtual void OnExit()
        {
            
        }
    }
}