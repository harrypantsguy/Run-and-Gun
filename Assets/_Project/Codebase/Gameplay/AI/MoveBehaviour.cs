using System.Threading.Tasks;
using _Project.Codebase.Gameplay.Characters;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Codebase.Gameplay.AI
{
    public class MoveBehaviour : CharacterBehaviour
    {
        private readonly NavmeshAgent m_agent;
        public MoveBehaviour(NavmeshAgent agent, Vector2 pos)
        {
            m_agent = agent;
            agent.SetTargetPosition(pos);
        }
        
        public MoveBehaviour(NavmeshAgent agent, Vector2Int pos)
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