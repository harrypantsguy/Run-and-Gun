using UnityEngine;
using UnityEngine.Tilemaps;

namespace _Project.Codebase.Gameplay.World
{
    public class BuildingAuthoring : MonoBehaviour
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
            
            int halfSize = Building.WORLD_SIZE / 2;
            for (int x = -halfSize; x < halfSize; x++)
            for (int y = -halfSize; y < halfSize; y++)
            {
                Vector2Int pos = new Vector2Int(x, y);
                bool isValidAndWalkable = m_building.navmesh.IsValidNode(pos) && m_building.navmesh.IsWalkableNode(pos);
                Gizmos.color = isValidAndWalkable
                    ? Color.green : Color.red;
                Vector2 worldPos = m_building.GridToWorld(pos);
                Gizmos.DrawWireCube(worldPos, new Vector3(.99f, .99f));
                if (m_building.IsFloorObjectAtPos(pos))
                {
                    Gizmos.color = Color.magenta;
                    Gizmos.DrawWireSphere(worldPos, .3f);
                }
            }
        }
    }
}