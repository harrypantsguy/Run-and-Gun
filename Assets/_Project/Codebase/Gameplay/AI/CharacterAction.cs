using System.Threading.Tasks;
using _Project.Codebase.Gameplay.Characters;
using _Project.Codebase.Gameplay.World;

namespace _Project.Codebase.Gameplay.AI
{
    public abstract class CharacterAction
    {
        protected readonly Character character;
        protected readonly WorldScreenshot worldContext;
        public int ActionPointCost { get; protected set; }
        
        public CharacterAction(Character character, WorldScreenshot worldContext)
        {
            this.worldContext = worldContext;
            this.character = character;
            ActionPointCost = 0;
        }
        
        public virtual Task OnStartAction() => Task.CompletedTask;

        public virtual Task Update() => Task.CompletedTask;

        public virtual Task OnEndAction() => Task.CompletedTask;
    }
}