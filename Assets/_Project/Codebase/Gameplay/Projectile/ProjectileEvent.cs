using UnityEngine;

namespace _Project.Codebase.Gameplay
{
    public struct ProjectileEvent
    {
        public ProjectileEventType type;
        public Vector2 location;
        public float time;

        public ProjectileEvent(ProjectileEventType type, Vector2 location, float time)
        {
            this.type = type;
            this.location = location;
            this.time = time;
        }
    }
}