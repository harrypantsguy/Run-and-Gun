using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Codebase.Gameplay.AI
{
    public class MoveBehaviour : AIBehaviour
    {
        private readonly NavmeshAgent m_agent;
        public MoveBehaviour(NavmeshAgent agent, Vector2 pos)
        {
            m_agent = agent;
            agent.SetTargetPosition(pos);
        }

        public override async Task RunBehaviour()
        {
            await UniTask.WaitWhile(() => !m_agent.AtPathEnd);
        }
    }
}