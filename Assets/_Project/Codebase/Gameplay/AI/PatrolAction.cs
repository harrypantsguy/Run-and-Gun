using System.Threading.Tasks;
using _Project.Codebase.Gameplay.World;
using UnityEngine;

namespace _Project.Codebase.Gameplay.AI
{
    public class PatrolAction : CharacterAction
    {
        private const int MAX_PATROL_RANGE = 4; 
        
        public PatrolAction(AIController controller, WorldScreenshot worldContext) : base(controller, worldContext)
        {
        }
        
        public override async Task Update()
        {
            Vector2Int targetPos = worldContext.building.GetRandomOpenFloorInRadius(
                controller.Character.transform.position, MAX_PATROL_RANGE, true).position;
            MoveBehaviour moveBehaviour = new MoveBehaviour(controller.Character.agent, targetPos);
            await moveBehaviour.RunBehaviour();
        }
    }
}

