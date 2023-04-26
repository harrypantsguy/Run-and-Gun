using _Project.Codebase.Gameplay.Projectiles;
using _Project.Codebase.Gameplay.World;
using _Project.Codebase.Modules;
using DanonFramework.Runtime.Core.Utilities;
using UnityEngine;

namespace _Project.Codebase.Gameplay.Characters
{
    public class Character : IFloorObject, IProjectileHittable
    {
        public readonly NavmeshAgent agent;
        public readonly Transform transform;

        public Vector2Int FloorPos { get; set; }

        public Character(NavmeshAgent agent, Vector2Int position)
        {
            this.agent = agent;
            transform = agent.transform;
            agent.onReachPathEnd += OnReachPathEnd;
            UpdateFloorPosition(position, true);
        }

        private void UpdateFloorPosition(Vector2Int gridPos, bool teleportToPos = false)
        {
            FloorPos = gridPos;
            Building building = ModuleUtilities.Get<GameModule>().Building;
            building.SetFloorObjectAtPos(gridPos, this);
            if (teleportToPos)
                transform.position = building.GridToWorld(FloorPos);
        }

        protected virtual void OnReachPathEnd(Vector2 worldPos, Vector2Int gridPos)
        {
            Debug.Log("party");
            UpdateFloorPosition(gridPos, true);
        }

        public void OnProjectileHit() {}
    }
}