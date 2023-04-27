using UnityEngine;

namespace _Project.Codebase.Gameplay.Characters
{
    public class CharacterRenderer : MonoBehaviour
    {
        [field: SerializeField] public SelectionRenderer SelectionRenderer { get; private set; }
    }
}