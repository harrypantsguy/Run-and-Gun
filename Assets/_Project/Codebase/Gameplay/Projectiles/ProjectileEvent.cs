using UnityEngine;

namespace _Project.Codebase.Gameplay.Projectiles
{
    public class ProjectileEvent
    {
        public readonly ProjectileEventType type;
        public readonly Vector2 location;
        public readonly float time;
        public readonly bool terminate;

        public ProjectileEvent(ProjectileEventType type, Vector2 location, float time, bool terminate = false)
        {
            this.type = type;
            this.location = location;
            this.time = time;
            this.terminate = terminate;
        }
    }
}