using _Project.Codebase.Gameplay.Characters;
using _Project.Codebase.Gameplay.World;
using Cysharp.Threading.Tasks;

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
        
        public virtual UniTask OnStartAction() => UniTask.CompletedTask;

        public virtual UniTask Update() => UniTask.CompletedTask;

        public virtual UniTask OnEndAction() => UniTask.CompletedTask;
    }
}