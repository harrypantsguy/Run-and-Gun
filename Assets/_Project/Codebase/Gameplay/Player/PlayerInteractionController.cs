using _Project.Codebase.Gameplay.Characters;
using DanonFramework.Runtime.Core.Utilities;
using UnityEngine;

namespace _Project.Codebase.Gameplay.Player
{
    public class PlayerInteractionController : MonoBehaviour
    {
        private IPlayerSelectable m_selection;
        private Camera m_camera;

        private void Start()
        {
            m_camera = Camera.main;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Vector2 worldMousePos = MiscUtilities.WorldMousePos;
                Collider2D mouseHitCollider = Physics2D.OverlapPoint(worldMousePos);
                if (mouseHitCollider != null)
                {
                    if (mouseHitCollider.TryGetComponent(out CharacterObject characterObject))  
                    {
                        SetSelection(characterObject.Character);
                    }
                }
                else
                    SetSelection(null);
            }
        }

        private void SetSelection(IPlayerSelectable newSelection)
        {
            m_selection?.SetPlayerSelectState(false);
            
            m_selection = newSelection;
            m_selection?.SetPlayerSelectState(true);
        }
    }
}