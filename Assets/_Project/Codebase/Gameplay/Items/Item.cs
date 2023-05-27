using _Project.Codebase.Gameplay.World;
using UnityEngine;

namespace _Project.Codebase.Gameplay.Items
{
    public abstract class Item : ScriptableObject, ICollectable
    {
        public virtual void Collect(ICollector collector)
        {
            collector.PickUpCollectable(this);
        }
    }
}