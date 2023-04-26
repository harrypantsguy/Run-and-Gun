using UnityEngine;

namespace _Project.Codebase.Gameplay.Characters
{
    public abstract class CharacterObject : MonoBehaviour
    {
        public abstract Character Character { get; }
    }
}