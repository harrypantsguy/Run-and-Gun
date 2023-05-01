using _Project.Codebase.Gameplay.Characters;
using _Project.Codebase.Gameplay.World;
using UnityEngine;

namespace _Project.Codebase.Gameplay.AI
{
    public class GuardAIController : AIController
    {
        public GuardAIController(Character character) : base(character)
        {
        }

        protected override CharacterAction DetermineAction(WorldScreenshot worldContext)
        {
            RaycastHit2D hit = Physics2D.Raycast(character.transform.position,
                worldContext.runner.transform.position - character.transform.position, character.firingRange);
            bool runnerCanBeShot = false;
            if (hit.collider != null)
            {
                runnerCanBeShot = hit.collider.CompareTag("Player");
            }

            if (!runnerCanBeShot)
            {
                return new RepositionAction(character, worldContext, worldContext.runner.transform.position, true);
            }

            return new FireGunAction(character, worldContext, worldContext.runner.transform.position);

            return null;
        }
    }
}
