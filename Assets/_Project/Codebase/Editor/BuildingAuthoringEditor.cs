using _Project.Codebase.Gameplay.World;
using DanonFramework.Core.Utilities;
using UnityEditor;
using UnityEngine;

namespace _Project.CodeBase.Editor
{
    [CustomEditor(typeof(BuildingAuthoring))]
    public class BuildingAuthoringEditor : CustomEditor<BuildingAuthoring>
    {
        private Grid m_grid;
        
        protected override void OnSceneGUI()
        {
            base.OnSceneGUI();

            if (m_grid == null) m_grid = CastedTarget.GetComponent<Grid>(); 
            
            foreach (SpawnTile spawnTile in CastedTarget.spawnTileLocations)
            {
                Handles.color = spawnTile.debugColor;
                
                for (var i = 0; i < spawnTile.locations.Count; i++)
                {
                    Vector2 spawnTileLocation = m_grid.CellToWorld(spawnTile.locations[i].ToVector3Int()) + m_grid.cellSize / 2f;
                    DrawPositionHandle(ref spawnTileLocation);
                    DrawWireCube(spawnTileLocation, m_grid.cellSize, 10f);
                    //Handles.DrawWireCube(spawnTileLocation, m_grid.cellSize);
                    spawnTile.locations[i] = m_grid.WorldToCell(spawnTileLocation).ToVector2Int();
                }
            }
        }
    }
}