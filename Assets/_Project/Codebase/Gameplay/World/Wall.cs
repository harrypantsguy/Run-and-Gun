using _Project.Codebase.Gameplay.Projectiles;
using UnityEngine;

namespace _Project.Codebase.Gameplay.World
{
    public class Wall : Cell, IProjectileHittable
    {
        public WallType type;
        public int pierceInfluence;
        public int ricochetInfluence;

        public Wall(Vector2Int position, WallType type, int pierceInfluence = 0, int ricochetInfluence = 0) :
            base(position)
        {
            this.type = type;
            this.pierceInfluence = pierceInfluence;
            this.ricochetInfluence = ricochetInfluence;
        }

        public void OnProjectileHit() {}
    }
}