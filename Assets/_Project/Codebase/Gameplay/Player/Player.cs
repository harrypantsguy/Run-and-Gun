using _Project.Codebase.Gameplay.Character;
using DanonFramework.Runtime.Core.Utilities;
using UnityEditor;
using UnityEngine;

namespace _Project.Codebase.Gameplay.Player
{
    public class Player : MonoBehaviour
    {
        private AimController m_aimController;
        [SerializeField] private Weapon m_weapon;
        private Camera m_cam;
        private bool m_shooterOnSide;
        private Vector2 m_oldMousePos;
        private Vector2 m_shooterClampRect;
        private Vector2 m_aimTargetClampRect;
        private Vector2 m_clampedWorldMousePos;

        private void Start()
        {
            m_aimController = GetComponent<AimController>();
            m_cam = Camera.main;
            
            m_shooterClampRect = new Vector2(m_cam.orthographicSize * m_cam.aspect, m_cam.orthographicSize);
            m_aimTargetClampRect = new Vector2(m_shooterClampRect.x - .25f, m_shooterClampRect.y - .25f);
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

            if (Input.GetKeyDown(KeyCode.Mouse0))
                m_weapon.Fire();

            m_clampedWorldMousePos = MathUtilities.ClampVector(MiscUtilities.WorldMousePos, -m_aimTargetClampRect,
                m_aimTargetClampRect);

            bool rightClicking = Input.GetKey(KeyCode.Mouse1);
            
            if (rightClicking)
            {
                if (Input.GetKey(KeyCode.LeftShift))
                    ShiftCameraAlongRectEdge(m_clampedWorldMousePos - m_oldMousePos, m_shooterClampRect);
                else
                {
                    if (Input.GetKey(KeyCode.LeftControl))
                        MoveCameraToClosestRectEdge(m_clampedWorldMousePos, m_shooterClampRect);
                    AimShooterAtMouse();
                }
            }
            
            m_oldMousePos = m_clampedWorldMousePos;
        }

        private void AimShooterAtMouse() => m_aimController.SetAimTarget(m_clampedWorldMousePos);
        
        private void MoveCameraToClosestRectEdge(Vector2 vector, Vector2 rect)
        {
            float x = vector.x;
            float y = vector.y;
            if (rect.x - Mathf.Abs(x) < rect.y - Mathf.Abs(y))
            {
                m_shooterOnSide = true;
                transform.position = new Vector2(rect.x * Mathf.Sign(vector.x), y);
                return;
            }

            m_shooterOnSide = false;
            transform.position = new Vector2(x, rect.y * Mathf.Sign(vector.y));
        }

        private void ShiftCameraAlongRectEdge(Vector2 delta, Vector2 rect)
        {
            Vector2 pos = transform.position;
            float newX = pos.x, newY = pos.y;
            if (m_shooterOnSide)// || Mathf.Abs(delta.x) > .25f && rect.x - Mathf.Abs(pos.x) < .5)
            {
                newX = rect.x * Mathf.Sign(pos.x);
                newY = Mathf.Clamp(pos.y + delta.y, -rect.y, rect.y);
                m_shooterOnSide = true;
            }
            else if (!m_shooterOnSide)// || Mathf.Abs(delta.y) > .25f && rect.y - Mathf.Abs(pos.y) < .5)
            {
                newY = rect.y * Mathf.Sign(pos.y);
                newX = Mathf.Clamp(pos.x + delta.x, -rect.x, rect.x);
                m_shooterOnSide = false;
            }

            transform.position = new Vector3(newX, newY);
        }
    }
}