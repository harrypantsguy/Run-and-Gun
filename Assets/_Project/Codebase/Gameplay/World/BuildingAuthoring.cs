using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;

[assembly: InternalsVisibleTo("EditorAssembly")]
namespace _Project.Codebase.Gameplay.World
{
    public class BuildingAuthoring : SerializedMonoBehaviour
    {
        public Tilemap wallMap;
        public Tilemap floorMap;
        public Tilemap doorMap;
        public Tilemap decorationMap;
        public Tilemap itemMap;
        [SerializeField] private bool m_debugNavmesh;

        // ReSharper disable once CollectionNeverUpdated.Global
        public readonly Dictionary<SpawnTileType, SpawnTileCollection> spawnTileLocations = new();

        private Building m_building;

        private void OnValidate()
        {
            spawnTileLocations.Remove(0);   
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