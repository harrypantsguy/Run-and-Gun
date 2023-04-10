using UnityEngine;

namespace _Project.Codebase.Gameplay.Character
{
    public class SpriteFlipperFromAngle : MonoBehaviour
    {
        private SpriteRenderer m_sprite;

        private void Start()
        {
            m_sprite = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            m_sprite.flipY = transform.right.x < 0;
        }
    }
}