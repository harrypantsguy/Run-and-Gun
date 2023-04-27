using UnityEngine;

namespace _Project.Codebase.Gameplay.Characters
{
    public class SelectionRenderer : MonoBehaviour
    {
        private SpriteRenderer m_spriteRenderer;

        private void Start()
        {
            m_spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void SetSelectionState(bool state)
        {
            //Color color = m_spriteRenderer.color;
            m_spriteRenderer.enabled = state;
            //m_spriteRenderer.color = new Color(color.r, color.g, color.b, state ? 1f : 0f);
        }
    }
}