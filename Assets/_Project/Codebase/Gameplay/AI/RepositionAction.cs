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
        public RepositionAction(Character character, WorldScreenshot worldContext, Vector2 newPosition, 
            bool pickClosestPoint = false) : base(character, worldContext)
        {
            PathResults pathResults = character.agent.SetTargetPosition(newPosition, false, pickClosestPoint, 
                character.CurrentLargestPossibleTravelDistance);
            ActionPointCost = character.CalcActionPointCostOfMove(pathResults.distance);
        }
        
        public RepositionAction(Character character, WorldScreenshot worldContext, List<Vector2> path, 
            int actionPointCost) : base(character, worldContext)
        {
            character.agent.ForceSetPath(path);
            ActionPointCost = actionPointCost;
        }

        protected override async UniTask Update()
        {
            await new MoveBehaviour(character).RunBehaviour();
        }
    }
}