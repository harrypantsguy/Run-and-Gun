
namespace _Project.Codebase.Gameplay
{
    public class Character
    {
        protected readonly NavmeshAgent agent;

        public Character(NavmeshAgent agent)
        {
            this.agent = agent;
        }
    }
}