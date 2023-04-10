using UnityEngine;

namespace _Project.Codebase.Gameplay
{
    public class Cell
    {
        public Vector2Int position;
        public bool traversable;
        public int pierceInfluence;
        public int ricochetInfluence;

        public Cell(Vector2Int position, bool traversable, int pierceInfluence = 0, int ricochetInfluence = 0)
        {
            this.position = position;
            this.traversable = traversable;
            this.pierceInfluence = pierceInfluence;
            this.ricochetInfluence = ricochetInfluence;
        }
    }
}