using System;
using _Project.Codebase.Gameplay.AI;
using _Project.Codebase.Gameplay.Player;
using _Project.Codebase.Gameplay.Projectiles;
using _Project.Codebase.Gameplay.World;
using _Project.Codebase.Modules;
using Cysharp.Threading.Tasks;
using DanonFramework.Runtime.Core.Utilities;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _Project.Codebase.Gameplay.Characters
{
    public class Character : IFloorObject, IProjectileHittable, IPlayerSelectable, IDamageable
    {
        public event Action OnCharacterDeath;
        public readonly NavmeshAgent agent;
        public readonly Transform transform;

        public Vector2Int FloorPos { get; set; }
        public int actionPoints = DEFAULT_MAX_ACTION_POINTS;
        public int moveDistancePerActionPoint = DEFAULT_MOVE_DISTANCE_PER_ACTION_POINT;
        public int MaxMaxActionPoints => DEFAULT_MAX_ACTION_POINTS;
        public int Health { get; private set; }
        public int MaxHealth { get; private set; }

        private CharacterRenderer m_renderer;
        
        protected const int DEFAULT_MAX_ACTION_POINTS = 1;
        protected const int DEFAULT_MOVE_DISTANCE_PER_ACTION_POINT = 6;

        public Character(Vector2Int position, NavmeshAgent agent, CharacterRenderer characterRenderer, int maxHealth)  
        {
            this.agent = agent;
            m_renderer = characterRenderer;
            transform = agent.transform;
            agent.onReachPathEnd += OnReachPathEnd;
            MaxHealth = maxHealth;
            Health = MaxHealth;
            UpdateFloorPosition(position, true);
        }

        public async UniTask PerformAction(CharacterAction action)
        {
            if (action != null)
            {
                actionPoints -= action.ActionPointCost;
                await action.OnStartAction();
                await action.Update();
                await action.OnEndAction();
            }
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
            UpdateFloorPosition(gridPos, true);
        }
        
        public int CalcActionPointCostOfMove(float distance) => Mathf.CeilToInt(distance / moveDistancePerActionPoint);

        public void OnProjectileHit(int damage)
        {
            TakeDamage(damage);
        }
        
        public void SetPlayerSelectState(bool state)
        {
            m_renderer.SelectionRenderer.SetSelectionState(state);
            m_renderer.RangeRenderer.SetDisplayedState(state);
        }

        public void UpdateDisplayedMovementRange()
        {
            m_renderer.RangeRenderer.CalculateAtPositionWithRange(FloorPos, moveDistancePerActionPoint * actionPoints);
        }

        public virtual PlayerSelectableType SelectableType { get; set; }
        public void TakeDamage(int damage)
        {
            Health = Mathf.Max(Health - damage, 0);
            m_renderer.HealthRenderer.SetValue((float)Health / MaxHealth);
            if (Health == 0)
                Die();
        }

        public void Die()
        {
            OnCharacterDeath?.Invoke();
            Object.Destroy(transform.gameObject);
        }
    }
}