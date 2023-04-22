using _Project.Codebase.Gameplay.Projectiles;
using _Project.Codebase.Gameplay.World;
using _Project.Codebase.Modules;
using DanonFramework.Runtime.Core.Utilities;
using UnityEngine;

namespace _Project.Codebase.Gameplay
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
            agent.OnReachPathEnd += OnReachPathEnd;
            UpdateFloorPosition(position, true);
        }

        private void UpdateFloorPosition(Vector2Int gridPos, bool teleportToPos = false)
        {
            FloorPos = gridPos;
            Building building = ModuleUtilities.Get<GameModule>().Building;
            building.TryGetFloorAtPos(gridPos, out Floor floor);
            floor.floorObject = this;
            transform.position = building.GridToWorld(FloorPos);
        }

        protected void OnReachPathEnd(Vector2 worldPos, Vector2Int gridPos)
        {
            UpdateFloorPosition(gridPos);
        }

        public void OnProjectileHit() {}
    }
}