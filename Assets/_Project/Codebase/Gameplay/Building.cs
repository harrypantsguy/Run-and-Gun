using System.Collections.Generic;
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

        private WallCellCollection m_wallDataCollection;
        private Dictionary<Vector2Int, Cell> m_wallCells = new();
        private Dictionary<Vector2Int, Cell> m_floorCells = new();
        private Dictionary<Vector2Int, Cell> m_doorCells = new();

        private void Awake()
        {
            Destroy(m_wallDataCollection);
                
            m_wallDataCollection = Instantiate(ContentUtilities.GetCachedAsset<WallCellCollection>(ScriptableAssetGroup.WALL_COLLECTION));
            
            for (int x = -100; x <= 100; x++)
            for (int y = -100; y <= 100; y++)
            {
                Vector2Int pos = new Vector2Int(x, y);
                Tile tile = m_wallMap.GetTile<Tile>((Vector3Int)pos);
                if (tile != null)
                {
                    WallCellData wallData = m_wallDataCollection.GetData(tile);
                    m_wallCells.Add(pos, new Cell(pos, false, wallData.pierceInfluence, wallData.ricochetInfluence));
                }
            }
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