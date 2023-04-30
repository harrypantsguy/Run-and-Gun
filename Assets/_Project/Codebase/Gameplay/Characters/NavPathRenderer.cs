using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Codebase.Gameplay.Characters
{
    public class NavPathRenderer : MonoBehaviour
    {
        private LineRenderer m_lineRenderer;
        private Converter<Vector2, Vector3> m_vector2ToVector3Converter;
        public bool Enabled
        {
            get => m_lineRenderer.enabled;
            set => m_lineRenderer.enabled = value;
        }
        
        private void Awake()
        {
            m_lineRenderer = GetComponent<LineRenderer>();
            m_vector2ToVector3Converter = (input => (Vector3)input);
        }

        public void SetPath(List<Vector2> points, bool isValid = true)
        {
            if (points == null)
            {
                m_lineRenderer.positionCount = 0;
                return;
            }

            Vector3[] positions = points.ConvertAll(m_vector2ToVector3Converter).ToArray();
            m_lineRenderer.positionCount = positions.Length;
            m_lineRenderer.SetPositions(positions);
            Color color = isValid ? Color.white : Color.red;
            m_lineRenderer.startColor = color;
            m_lineRenderer.endColor = color;
        }
    }
}