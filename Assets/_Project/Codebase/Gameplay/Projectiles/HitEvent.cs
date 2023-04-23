using UnityEngine;

namespace _Project.Codebase.Gameplay.Projectiles
{
    public class HitEvent : ProjectileEvent
    {
        public readonly IProjectileHittable hitTarget;
        public readonly SurfaceType surfaceType;
        public readonly Vector2 normal;
        public HitEvent(ProjectileEventType type, Vector2 location, float time, 
            IProjectileHittable hitTarget, SurfaceType surfaceType, Vector2 normal, Vector2 travelDir, bool terminate = false) :
            base(type, location, time, travelDir, terminate)
        {
            this.hitTarget = hitTarget;
            this.surfaceType = surfaceType;
            this.normal = normal;
        }
    }
}