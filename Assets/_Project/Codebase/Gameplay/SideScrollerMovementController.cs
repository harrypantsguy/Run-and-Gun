using System.Collections;
using UnityEngine;

namespace _Project.Codebase.Gameplay.Player
{
    public class SideScrollerMovementController : MonoBehaviour
    {
        [HideInInspector] public Vector2 dirInput;
        
        private Rigidbody2D m_rigidbody;
        private float m_dampedXVel;
        private bool m_isGrounded;
        private float m_lastGroundedTime;
        private Coroutine m_jumpRoutine;
        private bool m_startedJump;

        private const float c_default_move_speed = 3f;
        private const float c_max_velocity = 12f;
        private const float c_gravity_strength = 13f;
        private const float c_default_jump_strength = 8f;
        private const float c_extra_ground_cast_dist = .075f;
        private const float c_coyote_time = .15f;
        private const float c_jump_queue_time = .15f;

        private void Start()
        {
            m_rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            RaycastHit2D hit = Physics2D.BoxCast(transform.position, 
                new Vector2(.5f, 1f), 0f, Vector2.down, c_extra_ground_cast_dist);
            
            bool newGroundedState = hit.collider != null;

            if (!m_isGrounded && newGroundedState)
                m_startedJump = false;
            else if (m_isGrounded && !newGroundedState)
                m_lastGroundedTime = Time.time;

            m_isGrounded = newGroundedState;
        }

        private void FixedUpdate()
        {
            float desiredX = dirInput.x * c_default_move_speed;

            Vector2 velocity = m_rigidbody.velocity;

            velocity.x = Mathf.SmoothDamp(velocity.x, desiredX, ref m_dampedXVel, .05f,
                Mathf.Infinity, Time.fixedDeltaTime);
            velocity.y -= c_gravity_strength * Time.fixedDeltaTime;

            m_rigidbody.velocity = Vector2.ClampMagnitude(velocity, c_max_velocity);
        }

        private void OnDrawGizmos()
        {
            if (m_rigidbody != null)
                Gizmos.DrawRay(transform.position, m_rigidbody.velocity);
        }

        private void OnGUI()
        {
            GUI.Label(new Rect(0f, 0f, 400, 400), m_rigidbody.velocity.ToString());
        }

        public void Jump()
        {
            if (m_isGrounded || Time.time < m_lastGroundedTime + c_coyote_time && !m_startedJump)
            {
                ApplyJumpForce();
            }
            else
            {
                if (m_jumpRoutine != null)
                    StopCoroutine(m_jumpRoutine);
                m_jumpRoutine = StartCoroutine(JumpQueue(Time.time));
            }
        }

        private IEnumerator JumpQueue(float jumpStartTime)
        {
            while (!m_isGrounded && Time.time < jumpStartTime + c_jump_queue_time) yield return null;
            
            if (m_isGrounded) 
                ApplyJumpForce();
            
            m_jumpRoutine = null;
        }

        private void ApplyJumpForce()
        {
            m_startedJump = true;
            m_rigidbody.velocity = new Vector2(m_rigidbody.velocity.x, c_default_jump_strength);
        }
    }
}