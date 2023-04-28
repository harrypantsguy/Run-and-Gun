using System;
using _Project.Codebase.Gameplay.AI;
using _Project.Codebase.Gameplay.Characters;
using _Project.Codebase.Modules;
using _Project.Codebase.NavigationMesh;
using Cysharp.Threading.Tasks;
using DanonFramework.Runtime.Core.Utilities;
using UnityEngine;

namespace _Project.Codebase.Gameplay.Player
{
    public class PlayerTurnController : MonoBehaviour
    {
        private TurnController m_turnController;
        private PlayerManager m_playerManager;

        private bool m_queueEndTurn;
        private IPlayerSelectable Selection => m_playerManager.Selection;
        private bool m_isPlayerTurn;
        private bool m_fireKeyDown;
        private bool m_repositionKeyDown;

        private void Awake()
        {
            m_turnController = ModuleUtilities.Get<GameModule>().TurnController;
            m_playerManager = GetComponent<PlayerManager>();

            m_turnController.OnTurnChange += OnChangeTurn;
        }

        public void QueueEndTurn() => m_queueEndTurn = true;
        
        private void OnChangeTurn(Turn turn)
        {
            m_isPlayerTurn = turn == Turn.Player;
            m_playerManager.ShooterController.SetActivityState(false);
            if (m_isPlayerTurn)
            {
                m_playerManager.ShooterController.Reload();
                PlayerTurnAsync().Forget();
            }
        }

        private async UniTask PlayerTurnUninhibitedAsync()
        {
            while (m_turnController.Turn == Turn.Player)
            {
                await UniTask.Yield(PlayerLoopTiming.Update);
                
                if (Input.GetKeyDown(KeyCode.F))
                {
                    m_playerManager.SetShooterActiveState(!m_playerManager.ShooterActive);
                }
            }
        }
        
        private async UniTask PlayerTurnAsync()
        {
            PlayerTurnUninhibitedAsync().Forget();
            while (m_turnController.Turn == Turn.Player)
            {
                await UniTask.Yield(PlayerLoopTiming.Update);

                if (m_playerManager.ShooterActive && Input.GetKeyDown(KeyCode.Space))
                {
                    m_playerManager.ShooterController.Fire();
                    await UniTask.WaitWhile(() => m_playerManager.ShooterController.IsProjectileActive);
                }
                
                if (Selection != null && Selection.SelectableType == PlayerSelectableType.Runner && Input.GetKeyDown(KeyCode.Mouse1))
                {
                    Runner runner = (Runner)Selection;
                    RepositionAction repositionAction = new RepositionAction(runner, ModuleUtilities.Get<GameModule>().World,
                        MiscUtilities.WorldMousePos);
                    if (repositionAction.pathResults.type == PathResultType.FullPath &&
                        repositionAction.ActionPointCost <= runner.actionPoints)
                    {
                        await runner.PerformAction(repositionAction);
                    }
                }

                if (m_queueEndTurn)
                {
                    m_turnController.NextTurn();
                    m_queueEndTurn = false;
                }
            }
        }
    }
}