using System;
using System.Collections.Generic;
using _Project.Codebase.Gameplay.Projectile;
using DanonFramework.Runtime.Core.Utilities;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _Project.Codebase.Gameplay
{
    public class ProjectileSimReenactor
    {
        private GameObject m_projectile;
        private float m_currEventTime;
        private List<ProjectileEvent> m_events;
        private ProjectileEvent m_currentEvent;
        private ProjectileEvent m_nextEvent;
        private float m_currentEventLength;
        private int m_currEventIndex;
        
        public bool AtEndOfSim { get; private set; }

        public ProjectileSimReenactor(List<ProjectileEvent> events)
        {
            m_events = events;
            m_projectile = 
                Object.Instantiate(ContentUtilities.GetCachedAsset<GameObject>(PrefabAssetGroup.BASIC_PROJECTILE));
            m_currEventIndex = 0;
            m_currentEvent = events[0];
            m_nextEvent = events[1];
            m_currentEventLength = m_nextEvent.time;
            m_projectile.transform.position = events[0].location;
            MatchDirectionToNextEvent();
        }
        
        private Vector2 GetDirectionToNextEvent() => m_nextEvent.location - m_currentEvent.location;

        private void MatchDirectionToNextEvent() => m_projectile.transform.right = GetDirectionToNextEvent();

        public void Update(float deltaTime)
        {
            if (AtEndOfSim) return;
            
            m_projectile.transform.position = Vector2.Lerp(m_currentEvent.location, m_nextEvent.location,
                m_currEventTime / m_currentEventLength);
            m_currEventTime += deltaTime;
            if (m_currEventTime > m_currentEventLength)
                ProgressToNextEvent();
        }

        private void ProgressToNextEvent()
        {
            switch (m_nextEvent.type)
            {
                case ProjectileEventType.Position:
                    break;
                case ProjectileEventType.StartPierce:
                    break;
                case ProjectileEventType.EndPierce:
                    GameObject newParticleSystem = 
                        Object.Instantiate(ContentUtilities.GetCachedAsset<GameObject>(PrefabAssetGroup.RICOCHET_PARTICLE_SYSTEM));
                    newParticleSystem.transform.position = m_currentEvent.location;
                    newParticleSystem.transform.right = GetDirectionToNextEvent();
                    break;
                case ProjectileEventType.Ricochet:
                    break;
                case ProjectileEventType.Termination:
                    break;
            }
            
            if (m_currEventIndex == m_events.Count - 2)
            {
                AtEndOfSim = true;
                Object.Destroy(m_projectile);
                return;
            }
            
            m_currentEvent = m_nextEvent;
            m_nextEvent = m_events[++m_currEventIndex + 1];
            MatchDirectionToNextEvent();
            
            m_currEventTime %= m_currentEventLength;
            m_currentEventLength = m_nextEvent.time - m_currentEvent.time;
            if (m_currEventTime > m_currentEventLength)
                ProgressToNextEvent();
        }
    }
}