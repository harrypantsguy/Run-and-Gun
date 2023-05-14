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
        public RepositionAction(Character character, WorldRef worldContext, Vector2 newPosition) : base(character, worldContext)
        {
            PathResults pathResults = character.agent.SetPathTo(newPosition, false);
            ActionPointCost = character.CalcActionPointCostOfMove(pathResults.distance);
        }
        
        public RepositionAction(Character character, WorldRef worldContext, Vector2Int newPosition) : base(character, worldContext)
        {
            PathResults pathResults = character.agent.SetPathTo(newPosition, false);
            ActionPointCost = character.CalcActionPointCostOfMove(pathResults.distance);
        }
        
        public RepositionAction(Character character, WorldRef worldContext, List<Vector2> path, 
            int actionPointCost) : base(character, worldContext)
        {
            character.agent.SetPathTo(path, false);
            ActionPointCost = actionPointCost;
        }

        protected override async UniTask Update()
        {
            await new MoveBehaviour(character).RunBehaviour();
        }
    }
}