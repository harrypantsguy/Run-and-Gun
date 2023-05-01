using System.Threading.Tasks;
using _Project.Codebase.Gameplay.Characters;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Codebase.Gameplay.AI
{
    public class MoveBehaviour : CharacterBehaviour
    {
        private readonly NavmeshAgent m_agent;
        public MoveBehaviour(Character character, Vector2 pos) : base(character)
        {
            m_agent = character.agent;
            m_agent.SetTargetPosition(pos);
        }
        
        public MoveBehaviour(Character character, Vector2Int pos) : base(character)
        {
            m_agent = character.agent;
            m_agent.SetTargetPosition(pos);
        }

        public MoveBehaviour(Character character) : base(character)
        {
            m_agent = character.agent;
        }

        public override async Task RunBehaviour()
        {
            m_agent.followPath = true;
            while (!m_agent.AtPathEnd)
            {
                character.SetFacingDir(m_agent.PathDir);
                await UniTask.Yield();
            }
            m_agent.followPath = false;
        }
    }
}