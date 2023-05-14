using _Project.Codebase.Gameplay.Characters;
using _Project.Codebase.Gameplay.World;
using Cysharp.Threading.Tasks;

namespace _Project.Codebase.Gameplay.AI
{
    public abstract class AIController
    {
        protected readonly EnemyCharacter character;

        protected AIController(Character character)
        {
            this.character = (EnemyCharacter)character;
        }

        public async UniTask TakeTurn(WorldRef worldContext)
        {
            CharacterAction action = DetermineAction(worldContext);
            await character.PerformAction(action);
        }

        protected abstract CharacterAction DetermineAction(WorldRef worldContext);
    }
}