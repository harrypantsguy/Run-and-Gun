using UnityEngine;

namespace _Project.Codebase.Gameplay.Characters
{
    public abstract class CharacterObject : MonoBehaviour
    {
        [SerializeField] protected CharacterRenderer characterRenderer;
        public Character Character { get; protected set; }

        public abstract Character Initialize(Vector2Int position);
    }
}