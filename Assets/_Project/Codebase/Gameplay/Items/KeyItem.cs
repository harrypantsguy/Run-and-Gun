using _Project.Codebase.Gameplay.World;
using UnityEngine;

namespace _Project.Codebase.Gameplay.Items
{
    public abstract class KeyItem : ICollectable, IFloorObject
    {
        public KeyItemType type;
        public Vector2Int FloorPos { get; set; }

        public KeyItem(Vector2Int pos, KeyItemType type)
        {
            this.type = type;
            FloorPos = pos;
        }
        
        public virtual void Collect(ICollector collector)
        {
            collector.PickUpCollectable(this);
        }
    }
}