using System.Threading.Tasks;

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
            AIBehavior = behavior;
            if (behavior == null) return;
        }

        public async Task TakeTurn()
        {
            await AIBehavior.OnStartBehaviour();
            await AIBehavior.Update();
            await AIBehavior.OnEndBehaviour();
        }
    }
}