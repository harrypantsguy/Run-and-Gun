using UnityEngine;
using UnityEngine.Tilemaps;

namespace _Project.Codebase.Gameplay
{
    [CreateAssetMenu(fileName = "data")]
    public class TileScriptable : ScriptableObject
    {
        public TileBase tileBase;
    }
}