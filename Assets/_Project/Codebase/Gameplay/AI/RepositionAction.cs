using _Project.Codebase.Gameplay.Characters;
using _Project.Codebase.Gameplay.World;
using _Project.Codebase.NavigationMesh;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Codebase.Gameplay.AI
{
    public class RepositionAction : CharacterAction
    {
        private Vector2 m_targetPosition;
        public readonly PathResults pathResults; 
        public RepositionAction(Character character, WorldScreenshot worldContext, Vector2 newPosition) : base(character, worldContext)
        {
            pathResults = character.agent.SetTargetPosition(newPosition, false);
            ActionPointCost = Mathf.CeilToInt(pathResults.distance / character.moveDistancePerActionPoint);
        }

        public override async UniTask Update()
        {
            await new MoveBehaviour(character.agent).RunBehaviour();
        }
    }
}