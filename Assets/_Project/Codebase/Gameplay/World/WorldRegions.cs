using DanonFramework.Core.Utilities;
using UnityEngine;

namespace _Project.Codebase.Gameplay.World
{
    public class WorldRegions
    {
        private const float c_shooter_region_height = 10f;
        
        public readonly Vector2 shooterRegionExtents;
        public readonly Vector2 aimTargetRegionExtents;
        public WorldRegions()
        {
            shooterRegionExtents = new Vector2(c_shooter_region_height * (1920f/1080f), c_shooter_region_height);
            aimTargetRegionExtents = new Vector2(shooterRegionExtents.x - .25f, shooterRegionExtents.y - .25f);
        }

        public Vector2 ClampVectorInRegion(Vector2 pos, Vector2 regionExtents) =>
            MathUtilities.ClampVector(pos, -regionExtents, regionExtents);

        public bool IsPointInsideRegion(Vector2 pos, Vector2 regionExtents)
        {
            return pos.x >= -regionExtents.x && pos.x <= regionExtents.x && pos.y < regionExtents.y && pos.y > -regionExtents.y;
        }

        public Vector2 ProjectVectorOntoRegionEdge(Vector2 start, Vector2 vector, Vector2 regionExtents)
        {
           // Vector2 insetRect = GetInsetRect(start, regionExtents);

            float slope = Mathf.Abs(vector.y) / Mathf.Abs(vector.x);
            float regionSlope = regionExtents.y / regionExtents.x;
            if (slope > regionSlope)
                return new Vector2(Mathf.Sign(vector.x) * regionExtents.y / slope,Mathf.Sign(vector.y) * regionExtents.y);
            return new Vector2(Mathf.Sign(vector.x) * regionExtents.x, Mathf.Sign(vector.y) * regionExtents.x * slope);
        }

        public Vector2 GetInsetRect(Vector2 pos, Vector2 regionExtents)
        {
            return regionExtents - new Vector2(Mathf.Abs(pos.x), Mathf.Abs(pos.y));
        }
    }
}