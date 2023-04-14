using UnityEngine;

namespace _Project.Codebase.Gameplay.Character
{
    public class AimController : MonoBehaviour
    {
        public void SetAimTarget(Vector2 aimTarget)
        {
            transform.right = ((Vector3)aimTarget - transform.position).normalized;
        }
    }
}