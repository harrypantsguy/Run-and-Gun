using _Project.Codebase.Modules;
using Cysharp.Threading.Tasks;
using DanonFramework.Runtime.Core.Utilities;
using UnityEngine;

namespace _Project.Codebase.UI.Menus
{
    public class ShooterMenu : MenuController
    {
        [SerializeField] private GameObject m_graphics;

        private void Start()
        {
            ModuleUtilities.Get<GameModule>().PlayerManager.OnShooterActivationStateChange += SetMenuOpenState;
        }

        protected override void Update()
        {
            /*
            if (GetKeyDown(KeyCode.F))
            {
                SetMenuOpenState(!menuOpen);
            }
            */
        }

        public override async UniTask<bool> AsyncSetMenuOpenState(bool state)
        {
            await base.AsyncSetMenuOpenState(state);
            
            m_graphics.SetActive(state);
            return true;
        }
    }
}