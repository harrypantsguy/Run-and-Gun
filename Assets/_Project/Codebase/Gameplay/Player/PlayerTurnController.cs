using System.Threading.Tasks;
using _Project.Codebase.Gameplay.AI;
using _Project.Codebase.Gameplay.Characters;
using _Project.Codebase.Modules;
using _Project.Codebase.NavigationMesh;
using DanonFramework.Runtime.Core.Utilities;
using UnityEngine;

namespace _Project.Codebase.Gameplay.Player
{
    public class PlayerTurnController : MonoBehaviour
    {
        private TurnController m_turnController;
        private PlayerManager m_playerManager;

        private IPlayerSelectable Selection => m_playerManager.Selection;
        private bool m_isPlayerTurn;
        
        private void Awake()
        {
            m_turnController = ModuleUtilities.Get<GameModule>().TurnController;
            m_playerManager = GetComponent<PlayerManager>();
            
            m_turnController.OnTurnChange += OnChangeTurn;
        }

        private void OnChangeTurn(Turn turn)
        {
            m_isPlayerTurn = turn == Turn.Player;
            m_playerManager.ShooterController.gameObject.SetActive(m_isPlayerTurn);
            if (m_isPlayerTurn)
                m_playerManager.ShooterController.Reload();
        }

        private void Update()
        {
            if (!m_isPlayerTurn) return;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_playerManager.ShooterController.Fire();
            }

            if (Selection != null && Selection.SelectableType == PlayerSelectableType.Runner && Input.GetKeyDown(KeyCode.Mouse1))
            {
                Runner runner = (Runner)Selection;
                RepositionAction repositionAction = new RepositionAction(runner, ModuleUtilities.Get<GameModule>().World,
                    MiscUtilities.WorldMousePos);
                if (repositionAction.pathResults.type == PathResultType.FullPath && repositionAction.ActionPointCost <= runner.actionPoints)
                {
                    Task.Run(() => runner.PerformAction(repositionAction));
                }
            }
        }
    }
}