using JetBrains.Annotations;
using NaughtyAttributes;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
#endif

namespace _Project.Codebase.UI
{
    public class TintButtonController : ButtonGraphicsController
    {
        [OnValueChanged(c_test_color_func)] [SerializeField]
        private Image m_image;

        [SerializeField] private TMP_Text m_label;

        [OnValueChanged(c_test_color_func)] [BoxGroup(c_color_group_label)] [SerializeField]
        private Color m_normalColor;

        [OnValueChanged(c_test_color_func)] [BoxGroup(c_color_group_label)] [SerializeField]
        private Color m_hoverColor;

        [OnValueChanged(c_test_color_func)] [BoxGroup(c_color_group_label)] [SerializeField]
        private Color m_pressedColor;

        [OnValueChanged(c_test_color_func)] [BoxGroup(c_color_group_label)] [SerializeField]
        private Color m_uninteractableColor;

        [OnValueChanged(c_test_color_func)] [BoxGroup(c_color_group_label)] [SerializeField]
        private Color m_normalLabelColor;
        
        [OnValueChanged(c_test_color_func)] [BoxGroup(c_color_group_label)] [SerializeField]
        private Color m_hoverLabelColor;

        [OnValueChanged(c_test_color_func)] [BoxGroup(c_color_group_label)] [SerializeField]
        private Color m_uninteractableLabelColor;

#if UNITY_EDITOR
        [OnValueChanged(c_test_color_func)] [BoxGroup(c_color_group_label)] [SerializeField]
        private bool m_testColors;

        [OnValueChanged(c_test_color_func)] [BoxGroup(c_color_group_label)] [ShowIf("m_testColors")] [SerializeField]
        private ColorTestType m_colorTestType;
#endif
        
        private const string c_color_group_label = "Color Settings";
        private const string c_test_color_func = "SetColorForTest";
        
#if UNITY_EDITOR
        [UsedImplicitly]
        private void SetColorForTest()
        {
            if (m_testColors)
            {
                switch (m_colorTestType)
                {
                    case ColorTestType.Normal:
                        if (m_image != null)
                            m_image.color = m_normalColor;
                        if (m_label != null)
                            m_label.color = m_normalLabelColor;
                        break;
                    case ColorTestType.Hover:
                        if (m_image != null)
                            m_image.color = m_hoverColor;
                        if (m_label != null)
                            m_label.color = m_hoverLabelColor;
                        break;
                    case ColorTestType.Pressed:
                        if (m_image != null)
                            m_image.color = m_pressedColor;
                        if (m_label != null)
                            m_label.color = m_normalLabelColor;
                        break;
                    case ColorTestType.Uninteractable:
                        if (m_image != null)
                            m_image.color = m_uninteractableColor;
                        if (m_label != null)
                            m_label.color = m_uninteractableLabelColor;
                        break;
                }
            }
            else
            {
                if (m_image != null)
                    m_image.color = m_normalColor;
                if (m_label != null)
                    m_label.color = m_normalLabelColor;
            }

            if (m_image != null)
                PrefabUtility.RecordPrefabInstancePropertyModifications(m_image);
            if (m_label != null)
                PrefabUtility.RecordPrefabInstancePropertyModifications(m_label);
        }
#endif

        public override void OnStartHover()
        {
            if (m_image != null)
                m_image.color = m_hoverColor;
            if (m_label != null)
                m_label.color = m_hoverLabelColor;
        }

        public override void OnEndHover()
        {
            if (m_image != null)
                m_image.color = m_normalColor;
            if (m_label != null)
                m_label.color = m_normalLabelColor;
        }

        public override void OnPress()
        {
            if (m_image != null)
                m_image.color = m_pressedColor;
        }

        public override void OnRelease()
        {
            if (m_image != null)
                m_image.color = button.PointerInside ? m_hoverColor : m_normalColor;
        }

        public override void OnButtonEnable()
        {
            if (m_image != null) 
                m_image.color = m_normalColor;
            if (m_label != null)
                m_label.color = m_normalLabelColor;
        }

        public override void OnButtonDisable()
        {
            if (m_image != null)
                m_image.color = m_uninteractableColor;
            if (m_label != null)
                m_label.color = m_uninteractableLabelColor;
        }

        public override void OnInteractableStateChange(bool state)
        {
            if (m_image != null) 
                m_image.color = state ? m_normalColor : m_uninteractableColor;
            if (m_label != null)
                m_label.color = state ? m_normalLabelColor : m_uninteractableLabelColor;
        }

#if UNITY_EDITOR
        private enum ColorTestType
        {
            Normal,
            Hover,
            Pressed,
            Uninteractable
        }
#endif
    }
}