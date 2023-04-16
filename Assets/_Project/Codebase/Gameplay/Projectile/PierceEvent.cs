using UnityEngine;

namespace _Project.Codebase.Gameplay.Projectile
{
    public class PierceEvent : ProjectileEvent
    {
        public readonly SurfaceType surfaceType;
        public PierceEvent(ProjectileEventType type, Vector2 location, float time, SurfaceType surfaceType) : base(type, location, time)
        {
            this.surfaceType = surfaceType;
        }

        public PierceEvent(ProjectileEventType type, Vector2 location, Vector2 normal, float time, SurfaceType surfaceType) : base(type, location, normal, time)
        {
            this.surfaceType = surfaceType;
        }
    }
}