using System.Threading.Tasks;
using _Project.Codebase.Gameplay.Characters;

namespace _Project.Codebase.Gameplay.AI
{
    public abstract class CharacterBehaviour
    {
        protected readonly Character character; 
        public CharacterBehaviour(Character character)
        {
            this.character = character;
        }
        public virtual Task OnStartBehaviour() => Task.CompletedTask;
        public virtual Task RunBehaviour() => Task.CompletedTask;
        public virtual Task OnEndBehaviour() => Task.CompletedTask;
    }
}