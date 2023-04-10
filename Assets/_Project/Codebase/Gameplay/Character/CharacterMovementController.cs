using UnityEngine;

namespace _Project.Codebase.Gameplay.Player
{
    public class CharacterMovementController : MonoBehaviour
    {
        [HideInInspector] public Vector2 moveInput;
        
        private Rigidbody2D m_rigidbody;
        private Vector2 m_dampedVelocity;

        private const float m_default_move_speed = 4f;

        private void Start()
        {
            m_rigidbody = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate() 
        {
            Vector2 velocity = m_rigidbody.velocity;

            Vector2 desiredVelocity = moveInput * m_default_move_speed;

            velocity = Vector2.SmoothDamp(velocity, desiredVelocity, ref m_dampedVelocity, .05f, 
                Mathf.Infinity, Time.fixedDeltaTime);
            
            m_rigidbody.velocity = Vector2.ClampMagnitude(velocity, m_default_move_speed);
        }
    }
}