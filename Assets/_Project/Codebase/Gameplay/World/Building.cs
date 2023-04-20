using System.Collections.Generic;
using System.Linq;
using _Project.Codebase.Modules;
using _Project.Codebase.NavigationMesh;
using DanonFramework.Runtime.Core.Utilities;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace _Project.Codebase.Gameplay.World
{
    public class Building
    {
        public readonly Navmesh navmesh;
        private readonly Grid m_grid;
        private readonly Dictionary<Vector2Int, Wall> m_wallCells = new();
        private readonly Dictionary<Vector2Int, Floor> m_floorCells = new();
        private readonly Dictionary<Vector2Int, Cell> m_doorCells = new();
        
        public const int WORLD_SIZE = 50;

        public Building(Building buildingToCopy)
        {
            m_grid = buildingToCopy.m_grid;
            navmesh = new Navmesh(buildingToCopy.navmesh);
            m_wallCells = new Dictionary<Vector2Int, Wall>(buildingToCopy.m_wallCells);
            m_floorCells = new Dictionary<Vector2Int, Floor>(buildingToCopy.m_floorCells);
            m_doorCells = new Dictionary<Vector2Int, Cell>(buildingToCopy.m_doorCells);
        }
        
        public Building(Tilemap wallMap, Tilemap floorMap, Tilemap doorMap)
        {
            m_grid = wallMap.layoutGrid;

            var wallDataCollection = ContentUtilities.Instantiate<WallCellCollection>(ScriptableAssetGroup.WALL_COLLECTION);

            Dictionary<Vector2Int, bool> nodes = new();
            
            int halfSize = WORLD_SIZE / 2;
            for (int x = -halfSize; x < halfSize; x++)
            for (int y = -halfSize; y < halfSize; y++)
            {
                Vector2Int pos = new Vector2Int(x, y);
                Tile wallTile = wallMap.GetTile<Tile>((Vector3Int)pos);
                Tile floorTile = floorMap.GetTile<Tile>((Vector3Int)pos);
                bool walkable = false;
                bool tileAtPos = true;
                if (wallTile != null)
                {
                    WallCellData wallData = wallDataCollection.GetData(wallTile);
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

            navmesh = new Navmesh(nodes);
        }

        public Floor GetRandomOpenFloor()
        {
            List<Vector2Int> floorPositions = m_floorCells.Keys.ToList();
            int its = 0;
            while (its < 300)
            {
                m_floorCells.TryGetValue(floorPositions.GetRandom(), out Floor floor);
                if (floor != null && floor.floorObject == null)
                {
                    return floor;
                }
                its++;
            }

            return null;
        }
        
        public Wall GetWallAtPos(Vector2 pos)
        {
            Vector2Int gridPos = WorldToGrid(pos);
            //Vector2 center = m_grid.CellToWorld((Vector3Int)gridPos) + new Vector3(.5f, .5f);
            //GizmoUtilities.DrawXAtPos(center, 1f, Color.yellow);
            m_wallCells.TryGetValue(gridPos, out Wall wall);
            return wall;
        }

        public bool TryGetFloorAtPos(Vector2Int pos, out Floor floor) => m_floorCells.TryGetValue(pos, out floor);
        public bool IsFloorAtPos(Vector2 pos) => m_floorCells.ContainsKey(WorldToGrid(pos));

        public Vector2Int WorldToGrid(Vector2 pos) => (Vector2Int)m_grid.WorldToCell(pos);
        public Vector2 GridToWorld(Vector2Int pos) => m_grid.CellToWorld((Vector3Int)pos) + new Vector3(.5f, .5f);
    }
}