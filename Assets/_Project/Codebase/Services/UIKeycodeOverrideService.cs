using System.Collections.Generic;
using CHR.MenuControllers;
using DanonFramework.Runtime.Core.ServiceLayer;
using UnityEngine;

namespace _Project.Codebase.Services
{
    public sealed class UIKeycodeOverrideService : IService
    {
        private readonly Dictionary<KeyCode, List<GlobalKeyCodeOverride>> m_overrides;

        private const string c_uninitialized_override = "Attempting to remove uninitialized global keycode override";

        public UIKeycodeOverrideService()
        {
            m_overrides = new Dictionary<KeyCode, List<GlobalKeyCodeOverride>>();
        }
        
        public bool IsKeycodeOverriden(KeyCode key, int priority = 0)
        {
            if (!m_overrides.TryGetValue(key, out List<GlobalKeyCodeOverride> keycodeOverrides))
            {
                return false;
            }

            foreach (GlobalKeyCodeOverride keyOverride in keycodeOverrides)
            {
                if (keyOverride.priority <= priority) continue;

                if (keyOverride.overrides > 0) return true;
            }

            return false;
        }

        public void AddKeycodeOverride(KeyCode key, int priority)
        {
            List<GlobalKeyCodeOverride> keyOverrides;
            if (!m_overrides.TryGetValue(key, out keyOverrides))
            {
                keyOverrides = new List<GlobalKeyCodeOverride>();
                m_overrides.Add(key, keyOverrides);
            }

            foreach (GlobalKeyCodeOverride existingOverride in keyOverrides)
            {
                if (existingOverride.priority == priority)
                {
                    existingOverride.overrides++;
                    return;
                }
            }
            
            keyOverrides.Add(new GlobalKeyCodeOverride(priority));
        }

        public void RemoveKeycodeOverride(KeyCode key, int priority)
        {
            if (!m_overrides.TryGetValue(key, out List<GlobalKeyCodeOverride> keyOverrides))
            {
                Debug.LogWarning(c_uninitialized_override);
                return;
            }

            foreach (GlobalKeyCodeOverride existingOverride in keyOverrides)
            {
                if (existingOverride.priority == priority)
                {
                    existingOverride.overrides--;
                    return;
                }
            }
            
            Debug.LogWarning(c_uninitialized_override);
        }
    }
}