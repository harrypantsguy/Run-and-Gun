using System.Threading.Tasks;
using _Project.Codebase.Gameplay.Characters;
using _Project.Codebase.Gameplay.World;
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
        
        public override async Task Update()
        {
            Vector2Int targetPos = worldContext.building.GetRandomOpenFloorInRadius(
                character.transform.position, MAX_PATROL_RANGE, true).position;
            
            await new MoveBehaviour(character.agent, targetPos).RunBehaviour();
        }
    }
}

