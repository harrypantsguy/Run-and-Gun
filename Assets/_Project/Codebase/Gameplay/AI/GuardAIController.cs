using _Project.Codebase.Gameplay.Characters;
using _Project.Codebase.NavigationMesh;
using UnityEngine;

namespace _Project.Codebase.Gameplay.AI
{
    public class GuardAIController : AIController
    {
        public GuardAIController(Character character) : base(character)
        {
        }

        protected override CharacterAction DetermineAction(World.WorldRef worldContext)
        {
            RaycastHit2D hit = Physics2D.Raycast(character.transform.position,
                worldContext.runner.transform.position - character.transform.position, character.firingRange);
            bool runnerCanBeShot = false;
            if (hit.collider != null)
            {
                runnerCanBeShot = hit.collider.CompareTag("Runner");
            }

            if (!runnerCanBeShot)
            {
                Vector2Int newPos = character.FloorPos;
                bool foundPos = false;
                if (character.nodesInRangeOfPlayer.Count > 0)
                {
                    for (int i = 0; i < character.nodesInRangeOfPlayer.Count; i++)
                    {
                        PathNode node = character.nodesInRangeOfPlayer[i];
                        if (node.distance > character.CurrentLargestPossibleTravelDistance || 
                            worldContext.building.IsFloorObjectAtPos(node.pos))
                            continue;
                        newPos = character.nodesInRangeOfPlayer[i].pos;
                        foundPos = true;
                        break;
                    }
                }

                if (!foundPos)
                {
                    
                }
                
                return new RepositionAction(character, worldContext, newPos);
            }

            return new FireGunAction(character, worldContext, worldContext.runner.transform.position);
        }
    }
}
