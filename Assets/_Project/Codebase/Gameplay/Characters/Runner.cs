using _Project.Codebase.Gameplay.Player;
using UnityEngine;

namespace _Project.Codebase.Gameplay.Characters
{
    public class Runner : Character
    {
        public override PlayerSelectableType SelectableType => PlayerSelectableType.Runner;

        public Runner(Vector2Int position, NavmeshAgent agent, CharacterRenderer renderer, int maxHealth) 
            : base(position, agent, renderer, maxHealth)
        {
            
        }
    }
}