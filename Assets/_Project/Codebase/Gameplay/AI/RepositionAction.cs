using System.Collections.Generic;
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
            ActionPointCost = character.CalcActionPointCostOfMove(pathResults.distance);
        }
        
        public RepositionAction(Character character, WorldScreenshot worldContext, List<Vector2> path, PathResults pathResults, 
            int actionPointCost) : base(character, worldContext)
        {
            this.pathResults = pathResults;
            character.agent.ForceSetPath(path);
            ActionPointCost = actionPointCost;
        }

        public override async UniTask Update()
        {
            await new MoveBehaviour(character.agent).RunBehaviour();
        }
    }
}