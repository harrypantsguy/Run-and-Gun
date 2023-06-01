using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace _Project.Codebase.Gameplay.Items
{
    [CreateAssetMenu(menuName = "Item Collection/KeyItemCollection")]
    public class KeyItemCollection : SerializedScriptableObject
    {
        public Dictionary<KeyItemType, TileBase> keyItemTiles = new();
    }
}