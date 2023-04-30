using UnityEngine;
using UnityEngine.UI;

namespace _Project.Codebase.Gameplay.Characters
{
    public class HealthRenderer : MonoBehaviour
    {
        [SerializeField] private Image m_image;
        public void SetValue(float value)
        {
            value = Mathf.Clamp01(value);
            m_image.fillAmount = value;
        }
    }
}