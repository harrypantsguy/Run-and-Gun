using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Codebase.Gameplay.AIBehaviours
{
    public class PatrolBehaviour : AIBehavior
    {
        public PatrolBehaviour(AIController controller) : base(controller)
        {
        }

        public override Task OnStartBehaviour()
        {
            Character.agent.SetTargetPosition(Character.transform.position + new Vector3(2f, 0f));
            return Task.CompletedTask;
        }

        public override async Task Update()
        {
            await UniTask.WaitWhile(() => !Character.agent.AtPathEnd);
        }
    }
}

