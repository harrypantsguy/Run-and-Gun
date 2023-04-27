using System.Threading.Tasks;
using _Project.Codebase.Gameplay.Characters;
using _Project.Codebase.Gameplay.World;

namespace _Project.Codebase.Gameplay.AI
{
    public abstract class CharacterAction
    {
        protected readonly AIController controller;

        protected readonly Character character;
        protected readonly WorldScreenshot worldContext;
        public CharacterAction(AIController controller, WorldScreenshot worldContext)
        {
            this.controller = controller;
            this.worldContext = worldContext;
            character = controller.Character;
        }
        
        public virtual Task OnStartAction() => Task.CompletedTask;

        public virtual Task Update() => Task.CompletedTask;

        public virtual Task OnEndAction() => Task.CompletedTask;
    }
}