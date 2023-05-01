using System;
using System.Collections.Generic;
using _Project.Codebase.EditorUtilities;
using _Project.Codebase.Gameplay.World;
using _Project.Codebase.Modules;
using _Project.Codebase.NavigationMesh;
using DanonFramework.Runtime.Core.Utilities;
using UnityEngine;

namespace _Project.Codebase.Gameplay.Characters
{
    public class NavmeshRangeRenderer : MonoBehaviour
    {
        private bool m_active;
        private Navmesh m_navmesh;
        private Building m_building;
        private readonly Queue<Tile> m_openTiles = new();
        private readonly List<Vector2Int> m_closedPositions = new();
        private void Awake()
        {
            m_building = ModuleUtilities.Get<GameModule>().Building;
            m_navmesh = m_building.navmesh;
        }

        public void CalculateAtPositionWithRange(Vector2Int position, int range)
        {
            return;
            m_closedPositions.Clear();

            m_openTiles.Enqueue(new Tile(position, 0f));
            while (m_openTiles.TryDequeue(out Tile tile))
            {
                m_closedPositions.Add(tile.pos);
                for (var x = -1; x <= 1; x++)
                {
                    for (var y = -1; y <= 1; y++)
                    {
                        if (x == 0 && y == 0) continue;
                        bool isDiagonal = Mathf.Abs(x) == 1 && Mathf.Abs(y) == 1;

                        Vector2Int newPosition = tile.pos + new Vector2Int(x, y);
                        if (m_navmesh.IsWalkableNode(newPosition) && m_navmesh.IsValidNode(newPosition) && 
                            !m_closedPositions.Contains(newPosition) && !m_openTiles.Contains(new Tile(newPosition, 0f)))
                        {
                            float distance = tile.distance + (isDiagonal ? 1.41421f : 1f);
                            if (distance < range)
                            {
                                //if (distance > range - 1)
                                // navmesh path to point and see if is reached
                                m_openTiles.Enqueue(new Tile(newPosition, distance));
                            }
                        }
                    }
                }
            }
            
            foreach (Vector2Int pos in m_closedPositions)
                GizmoUtilities.DrawXAtPos(m_building.GridToWorld(pos), 1f, Color.blue);
        }

        private readonly struct Tile : IEquatable<Tile>
        {
            public readonly Vector2Int pos;
            public readonly float distance;

            public Tile(Vector2Int pos, float distance)
            {
                this.pos = pos;
                this.distance = distance;
            }

            public bool Equals(Tile other)
            {
                return pos.Equals(other.pos);
            }

            public override bool Equals(object obj)
            {
                return obj is Tile other && Equals(other);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(pos);
            }
        }

        public void SetDisplayedState(bool state)
        {
            m_active = state;
        }
    }
}