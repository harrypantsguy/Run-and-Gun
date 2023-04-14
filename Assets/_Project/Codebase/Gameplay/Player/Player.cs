using _Project.Codebase.Gameplay.Character;
using DanonFramework.Runtime.Core.Utilities;
using UnityEngine;

namespace _Project.Codebase.Gameplay.Player
{
    public class Player : MonoBehaviour
    {
        private AimController m_aimController;
        [SerializeField] private Weapon m_weapon;
        private Camera m_cam;

        private void Start()
        {
            m_aimController = GetComponent<AimController>();
            m_cam = Camera.main;
        }

        private void Update()
        {
            Vector2 input = Vector2.zero;
            if (Input.GetKey(KeyCode.A))
                input.x -= 1f;
            if (Input.GetKey(KeyCode.D))
                input.x += 1f;
            if (Input.GetKey(KeyCode.W))
                input.y += 1f;
            if (Input.GetKey(KeyCode.S))
                input.y -= 1f;
            
            m_aimController.aimTarget = MiscUtilities.WorldMousePos;
            
            if (Input.GetKeyDown(KeyCode.Mouse0))
                m_weapon.Fire();

            if (Input.GetKey(KeyCode.LeftShift))
            {
                transform.position = ClampVectorToClosestRectEdge(MiscUtilities.WorldMousePos,
                    new Vector2(m_cam.orthographicSize * m_cam.aspect, m_cam.orthographicSize));
            }
        }
        
        private Vector2 ClampVectorToClosestRectEdge(Vector2 vector, Vector2 rect)
        {
            float x = vector.x;
            float y = vector.y;
            if (Mathf.Abs(x) / rect.x > Mathf.Abs(y) / rect.y)
                return new Vector2(rect.x * Mathf.Sign(vector.x), y);
            return new Vector2(vector.x, rect.y * Mathf.Sign(vector.y));
            
        }
    }
}