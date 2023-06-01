using _Project.Codebase.Gameplay.World;
using UnityEngine;

namespace _Project.Codebase.Gameplay.Items
{
    public abstract class KeyItem : ICollectable
    {
        public KeyItemType type;
        public Vector2Int pos;

        public KeyItem(Vector2Int pos, KeyItemType type)
        {
            this.type = type;
        }
        
        public virtual void Collect(ICollector collector)
        {
            collector.PickUpCollectable(this);
        }
    }
}