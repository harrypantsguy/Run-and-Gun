using UnityEngine;

namespace _Project.Codebase.Gameplay.Projectile
{
    public class ProjectileEvent
    {
        public readonly ProjectileEventType type;
        public readonly Vector2 location;
        public readonly Vector2 normal;
        public readonly float time;

        public ProjectileEvent(ProjectileEventType type, Vector2 location, float time)
        {
            this.type = type;
            this.location = location;
            this.time = time;
            normal = Vector2.zero;
        }
        
        public ProjectileEvent(ProjectileEventType type, Vector2 location, Vector2 normal, float time)
        {
            this.type = type;
            this.normal = normal;
            this.location = location;
            this.time = time;
        }
    }
}