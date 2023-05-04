using System.Linq;
using _Project.Codebase.Gameplay.World;
using _Project.Codebase.Modules;
using DanonFramework.Runtime.Core.Utilities;
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
            
            Gizmos.color = Color.blue;

            if (character.agent.pathTrees.Values.Count == 0) return;
            foreach (var tile in character.agent.pathTrees.Values.ToList()[0].nodes)
            {
                Gizmos.DrawWireCube(m_building.GridToWorld(tile.Key), Vector3.one);
            }
        }

        public void SetDisplayedState(bool state)
        {
            m_active = state;
        }
    }
}