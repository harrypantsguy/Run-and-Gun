using System.Threading.Tasks;

namespace _Project.Codebase.Gameplay.AI
{
    public abstract class AIBehaviour
    {
        public virtual Task OnStartBehaviour() => Task.CompletedTask;
        public virtual Task RunBehaviour() => Task.CompletedTask;
        public virtual Task OnEndBehaviour() => Task.CompletedTask;
    }
}