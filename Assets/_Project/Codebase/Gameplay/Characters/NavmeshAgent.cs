using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Codebase.Gameplay.World;
using _Project.Codebase.Modules;
using _Project.Codebase.NavigationMesh;
using DanonFramework.Runtime.Core.Utilities;
using UnityEngine;

namespace _Project.Codebase.Gameplay.Characters
{
    public class NavmeshAgent : MonoBehaviour
    {
        [SerializeField] private bool m_debugPath;
        [SerializeField] private float m_moveSpeed;
        [SerializeField] private float m_nodeReachedDist;
        public WorldSpacePathController PathController { get; private set; } 
        public event Action<Vector2, Vector2Int> OnReachPathEnd;
        public Vector2 PathDir => PathController.DirToNextNode;
        [HideInInspector] public bool followPath;
        private Building m_building;
        
        public readonly Dictionary<Vector2Int, RangeFillTile> tilesInRange = new();

        public bool AtPathEnd => PathController.AtPathEnd;

        private void Awake()
        {
            GameModule gameModule = ModuleUtilities.Get<GameModule>();
            m_building = gameModule.Building;
            PathController = new WorldSpacePathController(m_building.navmesh, gameModule.Building.WorldToGrid,
                gameModule.Building.GridToWorld, false);
            PathController.onReachPathEnd += OnArriveAtPathEnd;
        }

        public void UpdateCalculateTilesInRange(Vector2 pos, float range)
        {
            UpdateCalculateTilesInRange(m_building.WorldToGrid(pos), range);
        }
        
        public void UpdateCalculateTilesInRange(Vector2Int gridPos, float range)
        {
            Queue<RangeFillTile> openTiles = new();
            tilesInRange.Clear();

            openTiles.Enqueue(new RangeFillTile(gridPos, 0f));
            while (openTiles.TryDequeue(out RangeFillTile tile))
            {
                tilesInRange.Add(tile.pos, tile);
                for (var x = -1; x <= 1; x++)
                for (var y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0) continue;
                    bool isDiagonal = Mathf.Abs(x) == 1 && Mathf.Abs(y) == 1;

                    Vector2Int newPosition = tile.pos + new Vector2Int(x, y);
                    if (m_building.navmesh.IsWalkableNode(newPosition) && m_building.navmesh.IsValidNode(newPosition) &&
                        !tilesInRange.ContainsKey(newPosition) && !openTiles.Contains(new RangeFillTile(newPosition, 0f)))
                    {
                        float distance = tile.distance + (isDiagonal ? 1.41421f : 1f);
                        if (distance < range)
                        {
                            openTiles.Enqueue(new RangeFillTile(newPosition, distance));
                        }
                    }
                }
            }
        }

        public Vector2 GetClosestTilePosInRange(Vector2 pos, out float distanceFromAgent)
        {
            Vector2Int gridPos = m_building.WorldToGrid(pos);
            if (tilesInRange.TryGetValue(gridPos, out RangeFillTile tile))
            {
                distanceFromAgent = tile.distance;
                return pos;
            }

            RangeFillTile closestTile = 
                tilesInRange.OrderBy(tilePos => 
                    Vector2.Distance(gridPos, tilePos.Key)).ToList()[0].Value;
            distanceFromAgent = closestTile.distance;
            return m_building.GridToWorld(closestTile.pos);
        }

        private void OnArriveAtPathEnd(Vector2 worldPos, Vector2Int gridPos)
        {
            OnReachPathEnd?.Invoke(worldPos, gridPos);
            transform.position = worldPos;
        }

        private void FixedUpdate()
        {
            if (!followPath) return;

            float distToNode = Vector2.Distance(transform.position, PathController.NextNode);
            if (distToNode < m_nodeReachedDist)
                PathController.TryProgressToNextNode();

            if (!PathController.AtPathEnd)
                transform.position = Vector2.MoveTowards(transform.position, PathController.NextNode,
                    Time.fixedDeltaTime * m_moveSpeed);
        }

        public PathResults SetTargetPosition(Vector2Int pos, bool startMoving = true, bool allowPartialPaths = false,
            float maxDistance = Mathf.Infinity)
        {
            PathResults results = PathController.GenerateAndSetPath(transform.position, pos, allowPartialPaths, maxDistance);
            followPath = startMoving;
            return results;
        }
        
        public PathResults SetTargetPosition(Vector2 pos, bool startMoving = true, bool allowPartialPaths = false,
            float maxDistance = Mathf.Infinity)
        {
            PathResults results = PathController.GenerateAndSetPath(transform.position, pos, allowPartialPaths,
                maxDistance);
            followPath = startMoving;
            return results;
        }

        public void ForceSetPath(in List<Vector2> positions) => PathController.ForceSetPath(positions);

        public PathResults GeneratePathTo(Vector2 pos, bool allowPartialPaths = false,
            float maxDistance = Mathf.Infinity)
        {
            return PathController.GeneratePath(transform.position, pos, allowPartialPaths, maxDistance);
        }

        public PathResults GeneratePathTo(Vector2 pos, in List<Vector2> positions, bool allowPartialPaths = false,
            float maxDistance = Mathf.Infinity)
        {
            return PathController.GeneratePath(transform.position, pos, positions, allowPartialPaths, maxDistance);
        }

        public PathResults GeneratePathTo(Vector2 pos, float range, in List<Vector2> positions)
        {
            UpdateCalculateTilesInRange(pos, range);
            return GeneratePathTo(pos, positions);
        }
        
        public PathResults GeneratePathTo(Vector2 pos, in List<Vector2> positions)
        {
            if (!PositionIsInTileRange(pos))
                return new PathResults(PathResultType.NoPath, 0f);
            return PathController.GeneratePath(transform.position, pos, positions);
        }

        private bool PositionIsInTileRange(Vector2 pos)
        {
            Vector2Int gridPos = ModuleUtilities.Get<GameModule>().Building.WorldToGrid(pos);
            return tilesInRange.ContainsKey(gridPos);
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying || !m_debugPath) return;

            if (PathController == null) return;
            
            for (var i = 1; i < PathController.Path.Count; i++)
            {
                Vector2 last = PathController.Path[i - 1];
                Vector2 current = PathController.Path[i];
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(last, current);
            }
        }
    }
}