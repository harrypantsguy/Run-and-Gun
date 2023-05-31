using System.Collections.Generic;
using UnityEngine;

namespace _Project.Codebase.Gameplay.World
{
    [CreateAssetMenu(menuName = "BuildingCollection")]
    public class BuildingCollection : ScriptableObject
    {
        public List<GameObject> buildings = new();
    }
}