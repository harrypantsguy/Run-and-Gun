using _Project.Codebase.Gameplay.Characters;
using _Project.Codebase.Gameplay.World;
using Cysharp.Threading.Tasks;
using UnityEngine;

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
            int loops = 0;
            while (character.actionPoints > 0)
            {
                loops++;
                if (loops > 20)
                {
                    Debug.LogWarning("AI Controller is looping infinitely trying to determine turn.");
                    return;
                } 
                CharacterAction action = DetermineAction(worldContext);
                if (action == null) return;
                await character.PerformAction(action);
            }
        }

        protected abstract CharacterAction DetermineAction(WorldRef worldContext);
    }
}