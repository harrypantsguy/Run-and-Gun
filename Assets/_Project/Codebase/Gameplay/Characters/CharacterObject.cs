using UnityEngine;

namespace _Project.Codebase.Gameplay.Characters
{
    public abstract class CharacterObject : MonoBehaviour
    {
        [SerializeField] protected CharacterRenderer characterRenderer;
        public Character Character { get; protected set; }

        public virtual Character Initialize(Vector2Int position)
        {
            return Character = new Character(position, GetComponent<NavmeshAgent>(), characterRenderer, 
                0);
        }
    }
}