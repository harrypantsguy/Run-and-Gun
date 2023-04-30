using UnityEngine;

namespace _Project.Codebase.Gameplay.Characters
{
    [RequireComponent(typeof(CharacterRenderer))]
    public abstract class CharacterObject : MonoBehaviour
    {
        public Character Character { get; protected set; }

        public virtual Character Initialize(Vector2Int position)
        {
            return Character = new Character(position, GetComponent<NavmeshAgent>(), GetComponent<CharacterRenderer>(), 
                0);
        }
    }
}