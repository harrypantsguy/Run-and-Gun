using System.Collections.Generic;
using _Project.Codebase.Modules;
using _Project.Codebase.NavigationMesh;
using DanonFramework.Runtime.Core.Utilities;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace _Project.Codebase.Gameplay.World
{
    public class Building
    {
        private Tilemap m_wallMap;
        private Tilemap m_floorMap;
        private Tilemap m_doorMap;
        
        private Navmesh m_navmesh;
        private WallCellCollection m_wallDataCollection;
        private Dictionary<Vector2Int, Wall> m_wallCells = new();
        private Dictionary<Vector2Int, Floor> m_floorCells = new();
        private Dictionary<Vector2Int, Cell> m_doorCells = new();
        
        private const int c_world_size = 50;

        public Building(Tilemap wallMap, Tilemap floorMap, Tilemap doorMap)
        {
            m_wallMap = wallMap;
            m_floorMap = floorMap;
            m_doorMap = doorMap;

            m_wallDataCollection = ContentUtilities.Instantiate<WallCellCollection>(ScriptableAssetGroup.WALL_COLLECTION);

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

        public Vector2Int WorldToGrid(Vector2 pos) => (Vector2Int)m_wallMap.WorldToCell(pos);
        public Vector2 GridToWorld(Vector2Int pos) => m_wallMap.CellToWorld((Vector3Int)pos) + new Vector3(.5f, .5f);
    }
}