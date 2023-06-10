using System.Collections.Generic;
using _Project.Codebase.Gameplay.Characters;
using _Project.Codebase.Gameplay.Player;
using _Project.Codebase.Modules;
using DanonFramework.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Codebase.UI
{
    public class PlayerSelectionUI : MonoBehaviour
    {
        [SerializeField, Required] private GameObject m_graphics;
        [SerializeField, Required] private List<ActionPointSlot> m_actionPointSlots;
        [SerializeField, Required] private GameObject m_actionButtonsParent;

        private Character m_selectedCharacter;
        private bool m_isRunnerSelected;

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
            else
                m_selectedCharacter = null;

            m_isRunnerSelected = m_selectedCharacter is Runner;
            
            m_actionButtonsParent.SetActive(m_isRunnerSelected);
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