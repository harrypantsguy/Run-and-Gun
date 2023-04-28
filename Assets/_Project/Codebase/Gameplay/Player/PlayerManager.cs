using System;
using _Project.Codebase.AssetGroups;
using _Project.Codebase.Gameplay.Shooter;
using DanonFramework.Runtime.Core.Utilities;
using UnityEngine;

namespace _Project.Codebase.Gameplay.Player
{
    public class PlayerManager : MonoBehaviour
    {
        public IPlayerSelectable Selection { get; private set; }

        public ShooterController ShooterController { get; private set; }

        public event Action<IPlayerSelectable> OnSelectSelectable; 
            
        private PlayerSelectionController m_selectionController;
        private void Awake()
        {
            m_selectionController = gameObject.AddComponent<PlayerSelectionController>();
            ShooterController = Instantiate(
                ContentUtilities.GetCachedAsset<GameObject>(PrefabAssetGroup.SHOOTER)).GetComponent<ShooterController>();
            
            gameObject.AddComponent<PlayerTurnController>();
        }
        
        public void SetSelection(IPlayerSelectable newSelection)
        {
            Selection?.SetPlayerSelectState(false);
            
            if (Selection != newSelection)
                OnSelectSelectable?.Invoke(newSelection);
            
            Selection = newSelection;
            Selection?.SetPlayerSelectState(true);
        }
    }
}