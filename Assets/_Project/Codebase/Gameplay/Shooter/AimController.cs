using UnityEngine;

namespace _Project.Codebase.Gameplay.Shooter
{
    public class AimController : MonoBehaviour
    {
        public void SetAimTarget(Vector2 aimTarget)
        {
            transform.right = ((Vector3)aimTarget - transform.position).normalized;
        }
    }
}