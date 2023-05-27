using _Project.Codebase.Gameplay;
using _Project.Codebase.Gameplay.Player;
using _Project.Codebase.Modules;
using Cysharp.Threading.Tasks;
using DanonFramework.Core.Utilities;

namespace _Project.Codebase.UI
{
    public class EndTurnButton : ToggleButton
    {
        private TurnController m_turnController;
        private PlayerTurnController m_playerTurnController;
        protected override async void Start()
        {
            base.Start();

            onClick += OnClick;
            await UniTask.WaitWhile(() => !ModuleUtilities.TryGet(out GameModule gameModule));
            m_turnController = ModuleUtilities.Get<GameModule>().TurnController;
            SetEnabledState(m_turnController.Turn == Turn.Player);
            m_playerTurnController = ModuleUtilities.Get<GameModule>().PlayerManager.PlayerTurnController;
            m_playerTurnController.OnQueueEndTurn += OnQueueEndTurn;
            m_turnController.OnTurnChange += OnTurnChange;
        }

        private void OnQueueEndTurn()
        {
            if (!Toggled)
                ForceSetToggleState(true);
        }

        private void OnTurnChange(Turn turn)
        {
            if (Toggled && turn == Turn.Player)
                ForceSetToggleState(false);
        }

        protected override void OnDestroy()
        {
            onClick -= OnClick;
        }

        private void OnClick()
        {
            if (m_turnController.Turn == Turn.Player)
                m_playerTurnController.QueueEndTurn();
        }
    }
}