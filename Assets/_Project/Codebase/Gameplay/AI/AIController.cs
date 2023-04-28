using System.Threading.Tasks;
using _Project.Codebase.Gameplay.Characters;
using _Project.Codebase.Gameplay.World;
using Cysharp.Threading.Tasks;

namespace _Project.Codebase.Gameplay.AI
{
    public abstract class AIController
    {
        public Character Character { get; }

        protected AIController(Character character)
        {
            Character = character;
        }

        public async UniTask TakeTurn(WorldScreenshot worldContext)
        {
            CharacterAction action = DetermineAction(worldContext);
            await Character.PerformAction(action);
        }

        protected abstract CharacterAction DetermineAction(WorldScreenshot worldContext);
    }
}