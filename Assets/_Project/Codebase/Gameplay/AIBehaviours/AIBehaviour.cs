
using System.Threading.Tasks;

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

        public virtual Task OnStartBehaviour() => Task.CompletedTask;

        public virtual Task Update() => Task.CompletedTask;

        public virtual Task OnEndBehaviour() => Task.CompletedTask;
    }
}