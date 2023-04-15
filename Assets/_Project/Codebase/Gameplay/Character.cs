using _Project.Codebase.Gameplay.World;
using _Project.Codebase.NavigationMesh;
using UnityEngine;

namespace _Project.Codebase.Gameplay
{
    public class Character : MonoBehaviour
    {
        private GameObject m_navTarget;
        private WorldSpacePathController m_pathController;
        
        private Building m_building;

        private void Start()
        {
            m_navTarget = new GameObject("Navmesh Target")
            {
                transform =
                {
                    position = transform.position,
                    parent = transform
                }
            };
        }
    }
}