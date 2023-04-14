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
        private ProjectileEvent m_lastEvent;
        private ProjectileEvent m_nextEvent;
        private float m_currentEventLength;
        private int m_eventCount;
        private float m_timeScale;
        private float m_slowdownRate;
        private float m_slowdownDist;
        
        public bool AtEndOfSim { get; private set; }

        private const float c_pierce_slowdown_rate = .01f;
        private const float c_pierce_slowdown_dist = 4f;
        private const float c_ricochet_slowdown_rate = 1f;
        private const float c_ricochet_slowdown_dist = 1.5f;

        public ProjectileSimReenactor(List<ProjectileEvent> events)
        {
            m_events = events;
            m_projectile = 
                Object.Instantiate(ContentUtilities.GetCachedAsset<GameObject>(PrefabAssetGroup.BASIC_PROJECTILE));
            m_eventCount = 0;
            m_lastEvent = events[0];
            m_nextEvent = events[1];
            OnEventChange(m_lastEvent, m_nextEvent);
            m_currentEventLength = m_nextEvent.time;
            m_projectile.transform.position = events[0].location;
            MatchDirectionToNextEvent();
        }
        
        private Vector2 GetDirectionToNextEvent() => m_nextEvent.location - m_lastEvent.location;

        private void MatchDirectionToNextEvent() => m_projectile.transform.right = GetDirectionToNextEvent();

        public void Update(float deltaTime)
        {
            if (AtEndOfSim) return;
            
            float distanceToEvent = Vector2.Distance(m_projectile.transform.position, m_nextEvent.location);
            if (m_lastEvent.type == ProjectileEventType.EndPierce && m_currEventTime < .01f)
                Time.timeScale = c_pierce_slowdown_rate;
            else
                Time.timeScale = Mathf.SmoothStep(m_slowdownRate, 1f, distanceToEvent / m_slowdownDist);
                
            m_projectile.transform.position = Vector2.Lerp(m_lastEvent.location, m_nextEvent.location, 
                m_currEventTime / m_currentEventLength);
            m_currEventTime += deltaTime;
            if (m_currEventTime > m_currentEventLength)
                ProgressToNextEvent();
        }

        private void OnEventChange(ProjectileEvent eventEnding, ProjectileEvent nextEvent)
        {
            switch (eventEnding.type)
            {
                case ProjectileEventType.Position:
                    break;
                case ProjectileEventType.StartPierce:
                    break;
                case ProjectileEventType.EndPierce:
                    GameObject newParticleSystem = 
                        Object.Instantiate(
                            ContentUtilities.GetCachedAsset<GameObject>(PrefabAssetGroup.PIERCE_PARTICLE_SYSTEM));
                    newParticleSystem.transform.position = eventEnding.location + (Vector2)m_projectile.transform.right * .001f;
                    newParticleSystem.transform.right = GetDirectionToNextEvent();
                    break;
                case ProjectileEventType.Ricochet:
                    break;
                case ProjectileEventType.Termination:
                    break;
            }

            Time.timeScale = 1f;
            m_slowdownDist = 0f;
            m_slowdownRate = 1f;
            switch (nextEvent.type)
            {
                case ProjectileEventType.Position:
                    break;
                case ProjectileEventType.StartPierce:
                    m_slowdownRate = c_pierce_slowdown_rate;
                    m_slowdownDist = c_pierce_slowdown_dist;
                    break;
                case ProjectileEventType.EndPierce:
                    m_slowdownRate = c_pierce_slowdown_rate;
                    m_slowdownDist = c_pierce_slowdown_dist;
                    break;
                case ProjectileEventType.Ricochet:
                    m_slowdownRate = c_ricochet_slowdown_rate;
                    m_slowdownDist = c_ricochet_slowdown_dist;
                    break;
                case ProjectileEventType.Termination:
                    break;
            }
        }

        private void ProgressToNextEvent()
        {
            m_eventCount++;
            if (m_eventCount < m_events.Count - 1)
                OnEventChange(m_nextEvent, m_events[m_eventCount + 1]);
            
            if (m_eventCount == m_events.Count - 1)
            {
                AtEndOfSim = true;
                Object.Destroy(m_projectile);
                return;
            }
            
            m_lastEvent = m_nextEvent;
            m_nextEvent = m_events[m_eventCount + 1];
            MatchDirectionToNextEvent();
            
            m_currEventTime %= m_currentEventLength;
            m_currentEventLength = m_nextEvent.time - m_lastEvent.time;
            if (m_currEventTime > m_currentEventLength)
                ProgressToNextEvent();
        }
    }
}