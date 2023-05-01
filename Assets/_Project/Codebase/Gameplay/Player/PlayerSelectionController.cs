using System.Collections.Generic;
using _Project.Codebase.AssetGroups;
using _Project.Codebase.Gameplay.Characters;
using _Project.Codebase.NavigationMesh;
using DanonFramework.Runtime.Core.Utilities;
using UnityEngine;

namespace _Project.Codebase.Gameplay.Player
{
    public class PlayerSelectionController : MonoBehaviour
    {
        private PlayerManager m_playerManager;
        private IPlayerSelectable Selection => m_playerManager.Selection;
        public readonly List<Vector2> desiredMovePath = new();
        public PathResults PathResults { get; private set; }
        public int PathActionPointCost { get; private set; }
        public bool IsValidSelectedPath { get; private set; }
        private NavPathRenderer m_pathRenderer;

        private void Awake()
        {
            m_playerManager = GetComponent<PlayerManager>();
            m_pathRenderer = ContentUtilities.Instantiate<GameObject>(PrefabAssetGroup.NAV_PATH_RENDERER)
                .GetComponent<NavPathRenderer>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                m_pathRenderer.Enabled = false;
                
                Collider2D mouseHitCollider = Physics2D.OverlapPoint(MiscUtilities.WorldMousePos);
                if (mouseHitCollider != null)
                {
                    if (mouseHitCollider.TryGetComponent(out CharacterObject characterObject))  
                    {
                        if (characterObject.Character.Selectable)
                        {
                            m_playerManager.SetSelection(characterObject.Character);
                            return;
                        }
                    }
                }
                
                m_playerManager.SetSelection(null);
            }
            
            
            if (Selection != null && Selection.SelectableType is PlayerSelectableType.Runner or PlayerSelectableType.Enemy)
            {
                Character character = (Character)Selection;
                character.UpdateDisplayedMovementRange();
                
                if (Selection.SelectableType is PlayerSelectableType.Runner)
                {
                    m_pathRenderer.Enabled = true;
                    Runner runner = (Runner)character;

                    PathResults = runner.agent.GeneratePathTo(MiscUtilities.WorldMousePos, desiredMovePath, true, 
                        character.LargestPossibleTravelDistance);
                    PathActionPointCost = runner.CalcActionPointCostOfMove(PathResults.distance);
                    IsValidSelectedPath = PathResults.type is not PathResultType.NoPath;
                    m_pathRenderer.SetPath(desiredMovePath, IsValidSelectedPath);
                }
            }
            else if (Selection == null && m_pathRenderer.Enabled)
                m_pathRenderer.Enabled = false;
        }
    }
}