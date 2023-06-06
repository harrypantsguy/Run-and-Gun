using UnityEngine;

namespace _Project.Codebase.UI
{
    public class GraphicMouseBlocker : MonoBehaviour
    {
        [SerializeField] private RectTransform m_graphicsRect;

        public static bool IsMouseOverUI { get; private set; }
        private static int s_mouseOverlapCount = 0;

        private bool m_mouseInside = false;
        
        private void Update()
        {
            bool mouseInRect = m_graphicsRect != null &&
                               m_graphicsRect.gameObject.activeInHierarchy &&
                               RectTransformUtility.RectangleContainsScreenPoint(m_graphicsRect, Input.mousePosition);
            
            if (mouseInRect && !m_mouseInside)
            {
                m_mouseInside = true;
                s_mouseOverlapCount++;
                IsMouseOverUI = true;
            }
            else if (!mouseInRect && m_mouseInside)
            {
                m_mouseInside = false;
                s_mouseOverlapCount--;
                IsMouseOverUI = s_mouseOverlapCount > 0;
            }
        }
    }
}