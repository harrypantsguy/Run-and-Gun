using System.Collections.Generic;
using _Project.Codebase.Gameplay.AI;
using _Project.Codebase.Gameplay.Player;
using _Project.Codebase.Gameplay.Shooter;
using _Project.Codebase.Gameplay.World;
using _Project.Codebase.Modules;
using _Project.Codebase.NavigationMesh;
using Cysharp.Threading.Tasks;
using DanonFramework.Core.Utilities;
using UnityEngine;  

namespace _Project.Codebase.Gameplay.Characters
{
    public class EnemyCharacter : Character
    {
        private readonly AIController m_AIController;
        public readonly float firingRange;
        public override PlayerSelectableType SelectableType => PlayerSelectableType.Enemy;
        private readonly Transform m_projectileSpawnPos;
        private readonly Weapon m_weapon;
        public readonly List<PathNode> nodesInRangeOfPlayer = new();

        public EnemyCharacter(Vector2Int position, NavmeshAgent agent, CharacterRenderer renderer, int maxHealth, 
            float firingRange, Transform projectileSpawnPos)
            : base(position, agent, renderer, maxHealth)
        {
            m_AIController = new GuardAIController(this);
            this.firingRange = firingRange;
            m_projectileSpawnPos = projectileSpawnPos;
            m_weapon = new Weapon(1);
        }

        protected override void OnAgentGeneratePathTree(ShortestPathTree tree)
        {
            base.OnAgentGeneratePathTree(tree);
            if (tree.source != FloorPos) return;
            WorldRef world = ModuleUtilities.Get<GameModule>().World;
            nodesInRangeOfPlayer.Clear();
            foreach (PathNode node in tree.nodes.Values)
            {
                Vector2 worldPos = world.building.GridToWorld(node.pos);
                RaycastHit2D hit =
                    Physics2D.Raycast(worldPos, ((Vector2)world.runner.transform.position - worldPos).normalized, firingRange);
                if (hit.collider == null || !hit.collider.CompareTag("Runner"))
                    continue;
                nodesInRangeOfPlayer.Add(node);
            }
        }

        public async UniTask TakeTurn(WorldRef worldContext)
        {
            agent.CalculateAllPathsFromSource(FloorPos, LargestPossibleTravelDistance);
            await m_AIController.TakeTurn(worldContext);
        }

        public async UniTask FireGun(Vector2 target)
        {
            m_weapon.Fire(m_projectileSpawnPos.position, (target - (Vector2)m_projectileSpawnPos.position).normalized);
            await UniTask.WaitWhile(() => m_weapon.IsProjectileActive);
        }
    }
}