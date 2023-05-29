using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace _Project.Codebase.Gameplay.Items
{
    //[CreateAssetMenu(menuName = "Items/MoneyBag", fileName = "MoneyBag")]
    public class MoneyBag : Item
    {
        public List<TileBase> Tiles { get; set; }
    }
}