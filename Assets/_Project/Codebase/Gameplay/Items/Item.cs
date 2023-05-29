using _Project.Codebase.Gameplay.World;

namespace _Project.Codebase.Gameplay.Items
{
    public abstract class Item : ICollectable
    {
        public virtual void Collect(ICollector collector)
        {
            collector.PickUpCollectable(this);
        }
    }
}