using System.Collections.Generic;
using _Project.Codebase.Services;
using Cysharp.Threading.Tasks;
using DanonFramework.Core;
using JetBrains.Annotations;
using UnityEngine;

namespace _Project.Codebase.UI.Menus
{
    public class MenuController : MonoBehaviour
    {
        private readonly Dictionary<KeyCode, int> m_keyCodeOverrides = new();
        private UIKeycodeOverrideService m_keycodeOverrideService;
        
        protected bool menuOpen;

        protected virtual void Awake() {}
        
        protected virtual void Update() {}
        protected virtual void LateUpdate() {}
        
        [UsedImplicitly]
        public static void SwapOpenMenu(MenuController menuToDeactivate, MenuController menuToActivate)
        {
            AsyncSwapOpenMenu(menuToDeactivate, menuToActivate).Forget();
        }
        
        public static async UniTask AsyncSwapOpenMenu(MenuController menuToDeactivate, MenuController menuToActivate)
        {
            bool deactivated = await menuToDeactivate.AsyncSetMenuOpenState(false);
            if (!deactivated) return;
            await menuToActivate.AsyncSetMenuOpenState(true);
        }

        public void SetMenuOpenState(bool state) => AsyncSetMenuOpenState(state).Forget();

        public virtual UniTask<bool> AsyncSetMenuOpenState(bool state)
        {
            menuOpen = state;
            return new UniTask<bool>(true);
        }

        protected bool GetKey(KeyCode keyCode, int priority = 0) => Input.GetKey(keyCode) && !IsKeyOverriden(keyCode, priority);

        protected bool GetKeyDown(KeyCode keyCode, int priority = 0) => Input.GetKeyDown(keyCode) && !IsKeyOverriden(keyCode, priority);

        private bool IsKeyOverriden(KeyCode keyCode, int priority = 0)
        {
            if (m_keycodeOverrideService == null)
                m_keycodeOverrideService = ServiceUtilities.Get<UIKeycodeOverrideService>();
            
            if (m_keycodeOverrideService.IsKeycodeOverriden(keyCode, priority)) 
                return true;
            
            if (!m_keyCodeOverrides.TryGetValue(keyCode, out int overrides))
                return false;

            return overrides != 0;
        }

        protected void AddGlobalKeyCodeOverride(KeyCode keyCode, int priority) => 
            m_keycodeOverrideService.AddKeycodeOverride(keyCode, priority);
        
        protected void RemoveGlobalKeyCodeOverride(KeyCode keyCode, int priority) => 
            m_keycodeOverrideService.RemoveKeycodeOverride(keyCode, priority);
        
        public void AddKeyCodeOverride(KeyCode keyCode)
        {
            if (!m_keyCodeOverrides.ContainsKey(keyCode))
                m_keyCodeOverrides.Add(keyCode, 1);
            else
                m_keyCodeOverrides[keyCode] += 1;
        }
        
        public void RemoveKeyCodeOverride(KeyCode keyCode)
        {
            if (!m_keyCodeOverrides.ContainsKey(keyCode))
            {
                Debug.LogError("Attempting to remove override with uninitialized keycode");
                return;
            }
            
            m_keyCodeOverrides[keyCode] -= 1;
        }
    }
}