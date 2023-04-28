using _Project.Codebase.Gameplay;
using _Project.Codebase.Modules;
using CHR.UI;
using Cysharp.Threading.Tasks;
using DanonFramework.Runtime.Core.Utilities;

namespace _Project.Codebase.UI
{
    public class EndTurnButton : CustomButton
    {
        private TurnController m_turnController;
        protected override async void Start()
        {
            base.Start();

            onClick += OnClick;
            await UniTask.WaitWhile(() => !ModuleUtilities.TryGet(out GameModule gameModule));
            m_turnController = ModuleUtilities.Get<GameModule>().TurnController;
            m_turnController.OnTurnChange += OnTurnChange;
            SetEnabledState(m_turnController.Turn == Turn.Player);
        }

        protected override void OnDestroy()
        {
            onClick -= OnClick;
        }

        private async void OnClick()
        {
            if (m_turnController.Turn == Turn.Player)
            {
                m_turnController.NextTurn();
            }
        }

        private void OnTurnChange(Turn turn)
        {
            SetEnabledState(turn == Turn.Player);
        }
    }
}