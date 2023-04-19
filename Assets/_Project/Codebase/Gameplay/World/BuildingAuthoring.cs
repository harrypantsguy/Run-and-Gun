using UnityEngine;
using UnityEngine.Tilemaps;

namespace _Project.Codebase.Gameplay.World
{
    public class BuildingAuthoring : MonoBehaviour, IInititializer<Building>
    {
        [SerializeField] private Tilemap m_wallMap;
        [SerializeField] private Tilemap m_floorMap;
        [SerializeField] private Tilemap m_doorMap;
        [SerializeField] private bool m_debugNavmesh;

        private Building m_building;
        
        public Building Initialize()
        {
            m_building = new Building(m_wallMap, m_floorMap, m_doorMap);
            return m_building;
        }
        
        private void OnDrawGizmos()
        {
            if (!Application.isPlaying || !m_debugNavmesh) return;

            /*
            int halfSize = c_world_size / 2;
            for (int x = -halfSize; x < halfSize; x++)
            for (int y = -halfSize; y < halfSize; y++)
            {
                Vector2Int pos = new Vector2Int(x, y);
                Gizmos.color = m_navmesh.IsValidNode(pos) && m_navmesh.IsWalkableNode(pos) ? Color.green : Color.red;
                Gizmos.DrawWireCube(pos + new Vector2(.5f, .5f), Vector3.one);
            }
            */
        }
    }
}