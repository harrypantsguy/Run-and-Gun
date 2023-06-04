using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using UnityEditor;
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

        // ReSharper disable once UnassignedField.Global
        public Dictionary<SpawnTileType, SpawnTileCollection> spawnTileLocations;

        // ReSharper disable once Unity.RedundantHideInInspectorAttribute
        [HideInInspector] public Building building;

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying || !m_debugNavmesh) return;
            
            int halfSize = Building.WORLD_SIZE / 2;
            for (int x = -halfSize; x < halfSize; x++)
            for (int y = -halfSize; y < halfSize; y++)
            {
                Vector2Int pos = new Vector2Int(x, y);
                bool isValidAndWalkable = building.navmesh.IsValidNode(pos) && building.navmesh.IsWalkableNode(pos);
                Gizmos.color = isValidAndWalkable
                    ? Color.green : Color.red;
                Vector2 worldPos = building.GridToWorld(pos);
                Gizmos.DrawWireCube(worldPos, new Vector3(.99f, .99f));
                
                if (building.IsFloorObjectAtPos(pos))
                {
                    Gizmos.color = Color.magenta;
                    Gizmos.DrawWireSphere(worldPos, .3f);
                }
            }
        }
    }
}