using UnityEngine;

namespace _Project.Codebase.Gameplay.World
{
    public abstract class Cell
    {
        public Vector2Int position;

        public Cell(Vector2Int position)
        {
            this.position = position;
        }
    }
}