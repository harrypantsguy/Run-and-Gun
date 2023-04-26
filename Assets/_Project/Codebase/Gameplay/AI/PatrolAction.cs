using System.Threading.Tasks;
using UnityEngine;

namespace _Project.Codebase.Gameplay.AI
{
    public class PatrolAction : TurnAction
    {
        public PatrolAction(AIController controller) : base(controller)
        {
        }
        
        public override async Task Update()
        {
            MoveBehaviour moveBehaviour = new MoveBehaviour(controller.Character.agent, Vector2.zero);
            await moveBehaviour.RunBehaviour();
        }
    }
}

