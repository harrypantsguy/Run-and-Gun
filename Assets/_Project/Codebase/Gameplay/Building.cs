using System.Collections.Generic;
using _Project.Codebase.NavigationMesh;
using DanonFramework.Runtime.Core.Utilities;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace _Project.Codebase.Gameplay
{
    public class Building : MonoBehaviour
    { 
        [SerializeField] private Tilemap m_wallMap;
        [SerializeField] private Tilemap m_floorMap;
        [SerializeField] private Tilemap m_doorMap;

        private Navmesh m_navmesh;
        private WallCellCollection m_wallDataCollection;
        private Dictionary<Vector2Int, Cell> m_wallCells = new();
        private Dictionary<Vector2Int, Cell> m_floorCells = new();
        private Dictionary<Vector2Int, Cell> m_doorCells = new();

        private const int c_world_size = 100;

        private void Awake()
        {
            Destroy(m_wallDataCollection);
                
            m_wallDataCollection = Instantiate(ContentUtilities.GetCachedAsset<WallCellCollection>(ScriptableAssetGroup.WALL_COLLECTION));

            bool[,] walkableStates = new bool[c_world_size, c_world_size];

            int halfSize = c_world_size / 2;
            for (int x = -halfSize; x < halfSize; x++)
            for (int y = -halfSize; y < halfSize; y++)
            {
                Vector2Int pos = new Vector2Int(x, y);
                Tile tile = m_wallMap.GetTile<Tile>((Vector3Int)pos);
                bool walkable = false;
                if (tile != null)
                {
                    WallCellData wallData = m_wallDataCollection.GetData(tile);
                    m_wallCells.Add(pos, new Cell(pos, wallData.type, wallData.pierceInfluence, wallData.ricochetInfluence));
                    walkable = true;
                }

                walkableStates[x + halfSize, y + halfSize] = walkable;
            }

            m_navmesh = new Navmesh(new Vector2Int(c_world_size, c_world_size), walkableStates);
        }

        public Cell GetWallAtPos(Vector2 pos)
        {
            Vector2Int gridPos = (Vector2Int)m_wallMap.WorldToCell(pos);
            Vector2 center = m_wallMap.CellToWorld((Vector3Int)gridPos) + new Vector3(.5f, .5f);
            GizmoUtilities.DrawXAtPos(center, 1f, Color.yellow);
            m_wallCells.TryGetValue(gridPos, out Cell cell);
            return cell;
        }
    }
}