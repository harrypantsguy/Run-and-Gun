using System;
using System.Collections.Generic;
using _Project.Codebase.Gameplay.Characters;
using _Project.Codebase.Gameplay.Player;
using _Project.Codebase.Modules;
using DanonFramework.Runtime.Core.Utilities;
using UnityEngine;

namespace _Project.Codebase.UI
{
    public class PlayerSelectionUI : MonoBehaviour
    {
        [SerializeField] private GameObject m_graphics;
        [SerializeField] private List<ActionPointSlot> m_actionPointSlots;

        private Character m_selectedCharacter;

        private void Start()
        {
            ModuleUtilities.Get<GameModule>().PlayerManager.OnSelectSelectable += OnSelectableChange;
        }

        private void OnSelectableChange(IPlayerSelectable selection)
        {
            m_graphics.SetActive(selection != null);
            if (selection != null)
            {
                if (selection.SelectableType is PlayerSelectableType.Runner or PlayerSelectableType.Enemy)
                {
                    m_selectedCharacter = (Character)selection;
                     
                    for (var i = 0; i < m_actionPointSlots.Count; i++)
                    {
                        var slot = m_actionPointSlots[i];
                        slot.gameObject.SetActive(i < m_selectedCharacter.MaxActionPoints);
                        slot.fillImage.enabled = i < m_selectedCharacter.actionPoints;
                    }
                }
            }
        }

        private void Update()
        {
            if (m_selectedCharacter != null)
            {
                for (var i = 0; i < m_actionPointSlots.Count; i++)
                {
                    var slot = m_actionPointSlots[i];
                    slot.fillImage.enabled = i < m_selectedCharacter.actionPoints;
                }
            }
        }
    }
}