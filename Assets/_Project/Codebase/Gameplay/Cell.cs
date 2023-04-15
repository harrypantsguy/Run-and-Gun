using UnityEngine;

namespace _Project.Codebase.Gameplay
{
    public class Cell
    {
        public Vector2Int position;
        public CellType type;
        public int pierceInfluence;
        public int ricochetInfluence;

        public Cell(Vector2Int position, CellType type, int pierceInfluence = 0, int ricochetInfluence = 0)
        {
            this.position = position;
            this.type= type;
            this.pierceInfluence = pierceInfluence;
            this.ricochetInfluence = ricochetInfluence;
        }
    }
}