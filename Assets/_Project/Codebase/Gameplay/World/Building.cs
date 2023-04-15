using System.Collections.Generic;
using _Project.Codebase.Modules;
using _Project.Codebase.NavigationMesh;
using DanonFramework.Runtime.Core.Utilities;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace _Project.Codebase.Gameplay.World
{
    public class Building : MonoBehaviour
    { 
        [SerializeField] private Tilemap m_wallMap;
        [SerializeField] private Tilemap m_floorMap;
        [SerializeField] private Tilemap m_doorMap;
        [SerializeField] private bool m_debugNavmesh;

        private Navmesh m_navmesh;
        private WallCellCollection m_wallDataCollection;
        private Dictionary<Vector2Int, Wall> m_wallCells = new();
        private Dictionary<Vector2Int, Floor> m_floorCells = new();
        private Dictionary<Vector2Int, Cell> m_doorCells = new();
        private bool m_initialized = false;
        
        private const int c_world_size = 50;

        private void Awake()
        {
            Initialize();
        }

        public void Initialize()
        {
            if (m_initialized) return;
            m_initialized = true;
            
            Destroy(m_wallDataCollection);
                
            m_wallDataCollection = Instantiate(ContentUtilities.GetCachedAsset<WallCellCollection>(ScriptableAssetGroup.WALL_COLLECTION));

            Dictionary<Vector2Int, bool> nodes = new();
            
            int halfSize = c_world_size / 2;
            for (int x = -halfSize; x < halfSize; x++)
            for (int y = -halfSize; y < halfSize; y++)
            {
                Vector2Int pos = new Vector2Int(x, y);
                Tile wallTile = m_wallMap.GetTile<Tile>((Vector3Int)pos);
                Tile floorTile = m_floorMap.GetTile<Tile>((Vector3Int)pos);
                bool walkable = false;
                bool tileAtPos = true;
                if (wallTile != null)
                {
                    WallCellData wallData = m_wallDataCollection.GetData(wallTile);
                    m_wallCells.Add(pos, new Wall(pos, wallData.type, wallData.pierceInfluence, wallData.ricochetInfluence));
                }
                else if (floorTile != null)
                {
                    walkable = true;
                    m_floorCells.Add(pos, new Floor(pos));
                }
                else
                    tileAtPos = false;

                if (tileAtPos)
                    nodes[new Vector2Int(x, y)] = walkable;
            }

            m_navmesh = new Navmesh(nodes);

            ModuleUtilities.Get<GameModule>().SetNavmesh(m_navmesh);
        }

        public Wall GetWallAtPos(Vector2 pos)
        {
            Vector2Int gridPos = WorldToGrid(pos);
            Vector2 center = m_wallMap.CellToWorld((Vector3Int)gridPos) + new Vector3(.5f, .5f);
            //GizmoUtilities.DrawXAtPos(center, 1f, Color.yellow);
            m_wallCells.TryGetValue(gridPos, out Wall wall);
            return wall;
        }

        public bool IsFloorAtPos(Vector2 pos) => m_floorCells.ContainsKey(WorldToGrid(pos));

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying || !m_debugNavmesh) return;

            int halfSize = c_world_size / 2;
            for (int x = -halfSize; x < halfSize; x++)
            for (int y = -halfSize; y < halfSize; y++)
            {
                Vector2Int pos = new Vector2Int(x, y);
                Gizmos.color = m_navmesh.IsValidNode(pos) && m_navmesh.IsWalkableNode(pos) ? Color.green : Color.red;
                Gizmos.DrawWireCube(pos + new Vector2(.5f, .5f), Vector3.one);
            }
        }

        public Vector2Int WorldToGrid(Vector2 pos) => (Vector2Int)m_wallMap.WorldToCell(pos);
        public Vector2 GridToWorld(Vector2Int pos) => m_wallMap.CellToWorld((Vector3Int)pos) + new Vector3(.5f, .5f);
    }
}