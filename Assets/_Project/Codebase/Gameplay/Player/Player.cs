using _Project.Codebase.Gameplay.Character;
using DanonFramework.Runtime.Core.Utilities;
using UnityEngine;

namespace _Project.Codebase.Gameplay.Player
{
    public class Player : MonoBehaviour
    {
        private AimController m_aimController;
        [SerializeField] private Weapon m_weapon;

        private void Start()
        {
            m_aimController = GetComponent<AimController>();
            Time.timeScale = .25f;
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
        }
    }
}