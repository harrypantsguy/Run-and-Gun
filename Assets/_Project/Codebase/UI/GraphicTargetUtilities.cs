using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CHR.UI
{
    public static class GraphicTargetUtilities
    {
        public static bool IsGraphicInVision(GraphicRaycaster raycaster, List<GameObject> graphicObjects, Vector2 point)
        {
            if (raycaster == null) return true;
            if (graphicObjects.Count == 0)
            {
                Debug.LogWarning("Checking if empty graphics list is in vision. FOOL!");
            }

            PointerEventData data = new PointerEventData(null);
            data.position = point;
            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(data, results);
            //foreach (RaycastResult result in results)
             //   Debug.Log($"{result.depth} {result.displayIndex} {result.index} {result.sortingLayer}", result.gameObject);
            
            if (results.Count == 0) return false;

            foreach (GameObject obj in graphicObjects)
            {
                if (ReferenceEquals(results[0].gameObject, obj))
                    return true;
            }

            return false;
        }
    }
}