namespace _Project.Codebase.Gameplay.AIBehaviours
{
    public abstract class AIBehavior
    {
        protected readonly AIController controller;

        public Character Character { get; }
        public float ElapsedTime { get; private set; }
        public AIBehavior(AIController controller)
        {
            this.controller = controller;
            Character = controller.Character;
        }

        public virtual void OnEnter() {}

        public virtual void Tick(float deltaTime)
        {
            ElapsedTime += deltaTime;
        }

        public virtual void OnExit()
        {
            
        }
    }
}