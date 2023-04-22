using UnityEngine;

namespace _Project.Codebase.Gameplay.Projectiles
{
    public class HitEvent : ProjectileEvent
    {
        public readonly IProjectileHittable hitTarget;
        public readonly SurfaceType surfaceType;
        public HitEvent(ProjectileEventType type, Vector2 location, float time, 
            IProjectileHittable hitTarget, SurfaceType surfaceType, bool terminate = false) : base(type, location, time, terminate)
        {
            this.hitTarget = hitTarget;
            this.surfaceType = surfaceType;
        }
    }
}