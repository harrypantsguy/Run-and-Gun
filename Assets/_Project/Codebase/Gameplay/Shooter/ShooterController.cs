using _Project.Codebase.Gameplay.World;
using _Project.Codebase.Modules;
using DanonFramework.Core.Utilities;
using UnityEngine;

namespace _Project.Codebase.Gameplay.Shooter
{
    public class ShooterController : MonoBehaviour
    {
        [SerializeField] private LaserSight m_laserSight;
        public bool IsProjectileActive => m_weapon.IsProjectileActive;
        private Weapon m_weapon;
        private bool m_active;
        private AimController m_aimController;
        private bool m_shooterOnSide;
        private Vector2 m_oldMousePos;

        private Vector2 m_clampedWorldMousePos;
        private WorldRegions m_worldRegions;
        private int m_shotsRemaining;

        public int ShotsRemaining => m_shotsRemaining;
        public int MaxShots => c_max_shots;
        
        private const int c_max_shots = 1;
        
        private void Start()
        {
            m_aimController = GetComponent<AimController>();

            m_worldRegions = ModuleUtilities.Get<GameModule>().WorldRegions;
            m_weapon = new Weapon();

            MoveShooterToClosestEdge(new Vector2(-10, 0), m_worldRegions.shooterRegionExtents);
            m_aimController.SetAimTarget(Vector2.zero);
        }

        private void Update()
        {
            if (!m_active) return;
            
            m_clampedWorldMousePos = MathUtilities.ClampVector(MiscUtilities.WorldMousePos, -m_worldRegions.aimTargetRegionExtents,
                m_worldRegions.aimTargetRegionExtents);

            bool rightClicking = Input.GetKey(KeyCode.Mouse1);
            
            if (rightClicking)
            {
                if (Input.GetKey(KeyCode.LeftShift))
                    ShiftShooterAlongRectEdge(m_clampedWorldMousePos - m_oldMousePos, m_worldRegions.shooterRegionExtents);
                else
                {
                    if (Input.GetKey(KeyCode.LeftControl))
                        MoveShooterToClosestEdge(m_clampedWorldMousePos, m_worldRegions.shooterRegionExtents);
                    AimShooterAtMouse();
                }
            }
            
            m_oldMousePos = m_clampedWorldMousePos;
        }

        public void SetActivityState(bool state)
        {
            m_active = state;
            m_laserSight.SetActivityState(state);
        }

        public void Reload() => m_shotsRemaining = c_max_shots;
        
        public void Fire()
        {
            if (m_shotsRemaining == 0) return;
            
            m_weapon.Fire(transform.position, transform.right);
            m_shotsRemaining--;
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