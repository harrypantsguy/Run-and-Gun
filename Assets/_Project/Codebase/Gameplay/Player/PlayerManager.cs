using UnityEngine;

namespace _Project.Codebase.Gameplay.Player
{
    public class PlayerManager : MonoBehaviour
    {
        private void Start()
        {
            gameObject.AddComponent<PlayerInteractionController>();
        }
    }
}