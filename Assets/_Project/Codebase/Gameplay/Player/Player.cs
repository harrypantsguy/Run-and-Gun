using _Project.Codebase.Gameplay.Shooter;
using DanonFramework.Runtime.Core.Utilities;
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

        private const float c_shooter_region_height = 10f;
        
        private void Start()
        {
            m_aimController = GetComponent<AimController>();
            m_cam = Camera.main;
            
            m_shooterClampRect = new Vector2(c_shooter_region_height * (1920f/1080f), c_shooter_region_height);
            m_aimTargetClampRect = new Vector2(m_shooterClampRect.x - .25f, m_shooterClampRect.y - .25f);

            MoveShooterToClosestEdge(new Vector2(-10, 0), m_shooterClampRect);
            m_aimController.SetAimTarget(Vector2.zero);
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
                    ShiftShooterAlongRectEdge(m_clampedWorldMousePos - m_oldMousePos, m_shooterClampRect);
                else
                {
                    if (Input.GetKey(KeyCode.LeftControl))
                        MoveShooterToClosestEdge(m_clampedWorldMousePos, m_shooterClampRect);
                    AimShooterAtMouse();
                }
            }
            
            m_oldMousePos = m_clampedWorldMousePos;
        }

        private void AimShooterAtMouse() => m_aimController.SetAimTarget(m_clampedWorldMousePos);
        
        private void MoveShooterToClosestEdge(Vector2 vector, Vector2 rect)
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

        private void ShiftShooterAlongRectEdge(Vector2 delta, Vector2 rect)
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