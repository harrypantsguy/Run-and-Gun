using System.Threading.Tasks;
using _Project.Codebase.Gameplay.Characters;
using _Project.Codebase.Gameplay.World;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Codebase.Gameplay.AI
{
    public class PatrolAction : CharacterAction
    {
        private const int MAX_PATROL_RANGE = 4; 
        
        public PatrolAction(Character character, WorldScreenshot worldContext) 
            : base(character, worldContext)
        {
            ActionPointCost = 1;
        }
        
        protected override async UniTask Update()
        {
            Vector2Int targetPos = worldContext.building.GetRandomOpenFloorInRadius(
                character.transform.position, MAX_PATROL_RANGE, true).position;
            
            await new MoveBehaviour(character, targetPos).RunBehaviour();
        }
    }
}

