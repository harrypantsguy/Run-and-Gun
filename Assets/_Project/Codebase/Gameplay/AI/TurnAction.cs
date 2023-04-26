using System.Threading.Tasks;
using _Project.Codebase.Gameplay.Characters;

namespace _Project.Codebase.Gameplay.AI
{
    public abstract class TurnAction
    {
        protected readonly AIController controller;

        public Character Character { get; }
        public TurnAction(AIController controller)
        {
            this.controller = controller;
            Character = controller.Character;
        }

        public virtual Task OnStartAction() => Task.CompletedTask;

        public virtual Task Update() => Task.CompletedTask;

        public virtual Task OnEndAction() => Task.CompletedTask;
    }
}