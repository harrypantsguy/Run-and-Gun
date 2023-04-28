using System.Threading.Tasks;
using _Project.Codebase.Gameplay.AI;
using _Project.Codebase.Gameplay.Characters;
using _Project.Codebase.Modules;
using Cysharp.Threading.Tasks;
using DanonFramework.Runtime.Core.Utilities;
using UnityEngine;

namespace _Project.Codebase.Gameplay.Player
{
    public class PlayerSelectionController : MonoBehaviour
    {
        private PlayerManager m_playerManager;
        private IPlayerSelectable Selection => m_playerManager.Selection;
        
        private void Start()
        {
            m_playerManager = GetComponent<PlayerManager>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Collider2D mouseHitCollider = Physics2D.OverlapPoint(MiscUtilities.WorldMousePos);
                if (mouseHitCollider != null)
                {
                    if (mouseHitCollider.TryGetComponent(out CharacterObject characterObject))  
                    {
                        m_playerManager.SetSelection(characterObject.Character);
                        return;
                    }
                }
                
                m_playerManager.SetSelection(null);
            }
        }
    }
}