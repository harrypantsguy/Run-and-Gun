using UnityEngine;

namespace _Project.Codebase.Gameplay.Character
{
    public class AimController : MonoBehaviour
    {
        [HideInInspector] public Vector2 aimTarget;

        private void Update()
        {
            transform.right = ((Vector3)aimTarget - transform.position).normalized;
        }
    }
}