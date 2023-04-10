using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace _Project.Codebase.Gameplay
{
    [CreateAssetMenu(menuName = "Cell Collection/WallCellCollection")]
    public class WallCellCollection : ScriptableObject
    {
        [SerializeField] private List<WallCellData> m_tiles = new ();
        private Dictionary<TileBase, WallCellData> m_dictionary = new();
        private bool m_initializedDictionary;

        public WallCellData GetData(TileBase tileBase)
        {
            TryInitializeDictionary();
            if (m_dictionary.TryGetValue(tileBase, out WallCellData data))
                return data;
            
            Debug.LogError($"Failed to locate tileBase {tileBase.name} scriptable");
            return default;
        }

        private void TryInitializeDictionary()
        {
            if (!m_initializedDictionary)
            {
                m_initializedDictionary = true;
                m_dictionary = new Dictionary<TileBase, WallCellData>();
                foreach (WallCellData data in m_tiles)
                    m_dictionary.Add(data.tileBase, data);
            }
        }
    }
}