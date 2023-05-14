using System.Linq;
using _Project.Codebase.Gameplay.Characters;
using DanonFramework.Runtime.Core.Utilities;
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
                Vector2Int newPos;
                if (character.nodesInRangeOfPlayer.Count > 0)
                {
                    newPos = character.nodesInRangeOfPlayer[0].pos;
                }
                else
                    newPos = character.FloorPos;
                return new RepositionAction(character, worldContext, newPos);
            }

            return new FireGunAction(character, worldContext, worldContext.runner.transform.position);
        }
    }
}
