using System.Threading.Tasks;
using _Project.Codebase.Gameplay.Characters;
using _Project.Codebase.Gameplay.World;

namespace _Project.Codebase.Gameplay.AI
{
    public abstract class AIController
    {
        public Character Character { get; }

        protected AIController(Character character)
        {
            Character = character;
        }

        public async Task TakeTurn(WorldScreenshot worldContext)
        {
            TurnAction action = DetermineAction(worldContext);
            if (action != null)
            {
                await action.OnStartAction();
                await action.Update();
                await action.OnEndAction();
            }
        }

        protected abstract TurnAction DetermineAction(WorldScreenshot worldContext);
    }
}