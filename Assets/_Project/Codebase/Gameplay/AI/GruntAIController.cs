using _Project.Codebase.Gameplay.Characters;
using _Project.Codebase.Gameplay.World;

namespace _Project.Codebase.Gameplay.AI
{
    public class GruntAIController : AIController
    {
        public GruntAIController(Character character) : base(character)
        {
        }

        protected override CharacterAction DetermineAction(WorldScreenshot worldContext)
        {
            bool playerInVision = false;
            if (!playerInVision)
                return new PatrolAction(this, worldContext);
            return null;
        }
    }
}
