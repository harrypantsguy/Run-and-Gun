using UnityEngine;

namespace _Project.Codebase
{
    public static class GizmoUtilities
    {
        public static void DrawXAtPos(Vector2 pos, float width, float time = 0f) => DrawXAtPos(pos, width, Color.red, time);
        
        public static void DrawXAtPos(Vector2 pos, float width, Color color, float time = 0f)
        {
            float dist = width / 2f;
            Debug.DrawLine(pos + new Vector2(-dist, dist), pos + new Vector2(dist, -dist),
                color, time);
            Debug.DrawLine(pos + new Vector2(-dist, -dist), pos + new Vector2(dist, dist), 
                color, time);
        }
    }
}