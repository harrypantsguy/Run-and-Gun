using System.Collections.Generic;
using _Project.Codebase.Gameplay.Player;
using _Project.Codebase.Gameplay.World;
using UnityEngine;

namespace _Project.Codebase.Gameplay.Characters
{
    public class Runner : Character, ICollector
    {
        public override PlayerSelectableType SelectableType => PlayerSelectableType.Runner;

        private List<ICollectable> m_collectables = new();
        
        public Runner(Vector2Int position, NavmeshAgent agent, CharacterRenderer renderer, int maxHealth) 
            : base(position, agent, renderer, maxHealth)
        {
            agent.CalculateAllPathsFromSource(FloorPos, LargestPossibleTravelDistance);
        }

        public void PickUpCollectable(ICollectable collectable)
        {
            m_collectables.Add(collectable);
        }
    }
}