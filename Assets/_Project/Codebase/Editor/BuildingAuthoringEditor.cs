using System.Collections.Generic;
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
            
            foreach (KeyValuePair<SpawnTileType, SpawnTileCollection> tileCollectionPair in CastedTarget.spawnTileLocations)
            {
                SpawnTileCollection collection = tileCollectionPair.Value;
                Handles.color = collection.debugColor;
                if (!collection.debug) continue;
                
                for (var i = 0; i < collection.locations.Count; i++)
                {
                    Vector2 spawnTileLocation = m_grid.CellToWorld(collection.locations[i].ToVector3Int()) + m_grid.cellSize / 2f;
                    DrawPositionHandle(ref spawnTileLocation);
                    DrawWireCube(spawnTileLocation, m_grid.cellSize, 10f);
                    //Handles.DrawWireCube(spawnTileLocation, m_grid.cellSize);
                    collection.locations[i] = m_grid.WorldToCell(spawnTileLocation).ToVector2Int();
                }
            }
        }
    }
}