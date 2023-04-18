using UnityEngine;

namespace _Project.Codebase.Gameplay.World
{
    public class Floor : Cell
    {
        public IFloorObject floorObject;
        
        public Floor(Vector2Int position) : 
            base(position)
        {
            
        }
    }
}