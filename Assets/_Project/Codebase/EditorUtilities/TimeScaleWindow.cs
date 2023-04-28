using UnityEditor;

namespace _Project.Codebase.EditorUtilities
{
    public class TimeScaleWindow : EditorWindow
    {
        /*
        private float m_timeScale = 1.0f;
        private float m_timeScaleBefore;
        private float m_minTimeScale;
        private float m_maxTimeScale = 1f;
        
        [MenuItem("Tools/Time Scale Window")]
        public static void OpenWindow()
        {
            var window = GetWindow(typeof(TimeScaleWindow));
            window.titleContent = new GUIContent("Time Scale Window");
        }

        private void OnEnable()
        {
            EditorApplication.playModeStateChanged += OnPlaymodeStateChange;
        }
        private void OnDisable()
        {
            EditorApplication.playModeStateChanged -= OnPlaymodeStateChange;
        }

        private void OnGUI()
        {
            m_timeScale = EditorGUILayout.Slider("Time Scale", m_timeScale, m_minTimeScale, m_maxTimeScale);
            m_minTimeScale = EditorGUILayout.FloatField(m_minTimeScale);
            m_maxTimeScale = EditorGUILayout.FloatField(m_maxTimeScale);
            ChangeTimeScale(m_timeScale);
        }

        private void OnPlaymodeStateChange(PlayModeStateChange obj)
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode && EditorApplication.isPlaying)
            {
                m_timeScaleBefore = m_timeScale;
            }
            else if (!EditorApplication.isPlayingOrWillChangePlaymode && !EditorApplication.isPlaying)
            {
                m_timeScale = m_timeScaleBefore;
                ChangeTimeScale(m_timeScale);
                Repaint();
            }
        }

        private void ChangeTimeScale(float value)
        {
            if (Math.Abs(m_timeScale - Time.timeScale) > .001f) { Time.timeScale = m_timeScale; }
        }
        */
    }
}