using System.Collections.Generic;
using UnityEngine;

namespace _Project.Codebase.UI
{
    public class ToggleButtonGroupController : MonoBehaviour
    {
        [SerializeField] private List<ToggleButton> m_buttons = new List<ToggleButton>();
        private ToggleButton m_toggledButton;

        private void Start()
        {
            foreach (ToggleButton button in m_buttons)
            {
                button.onClick += () => OnButtonClick(button);
            }
        }

        private void OnButtonClick(ToggleButton buttonPressed)
        {
            Debug.Log($"{buttonPressed.name} + {buttonPressed.Toggled}");
            if (m_toggledButton != null)
                m_toggledButton.ForceSetToggleState(false);
            m_toggledButton = buttonPressed;
        }
    }
}