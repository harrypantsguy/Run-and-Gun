using _Project.Codebase.Gameplay.World;
using _Project.Codebase.Modules;
using DanonFramework.Core;
using UnityEngine;

namespace _Project.Codebase.Gameplay.Characters
{
    public class NavmeshRangeRenderer : MonoBehaviour
    {
        private bool m_active;
        private Building m_building;

        public Character character;

        private void Start()
        {
            m_building = ModuleUtilities.Get<GameModule>().Building;
        }

        private void OnDrawGizmos()
        {
            if (!m_active) return;
            
            if (character.agent.PathTree == null) return;
            
            foreach (var tile in character.agent.PathTree.nodes)
            {
                Gizmos.color = Color.Lerp(Color.green, Color.red, tile.Value.distance / character.LargestPossibleTravelDistance);
                Gizmos.DrawWireCube(m_building.GridToWorld(tile.Key), Vector3.one);
                Gizmos.color = Color.white;
                //if (tile.Value.parent != null)
                // Gizmos.DrawLine(m_building.GridToWorld(tile.Value.pos), m_building.GridToWorld(tile.Value.parent.pos));
            }
        }

        public void SetDisplayedState(bool state)
        {
            m_active = state;
        }
    }
}