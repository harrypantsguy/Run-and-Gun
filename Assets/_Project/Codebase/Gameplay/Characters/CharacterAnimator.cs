using Animancer;
using UnityEngine;

namespace _Project.Codebase.Gameplay.Characters
{
    public class CharacterAnimator : MonoBehaviour
    {
        [SerializeField] private AnimationClip m_legWalk; 
        private SpriteRenderer m_legsRenderer;
        private AnimancerComponent m_animancer;
        private Character m_character;
        
        public void Initialize(Character character)
        {
            m_character = character;
        }
        
        private void Start()
        {
            m_animancer = GetComponent<AnimancerComponent>();
            m_legsRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            bool followingPath = m_character.agent.followPath;
            m_legsRenderer.enabled = followingPath;
            if (followingPath)
                m_animancer.Play(m_legWalk);
            else
                m_animancer.Stop(m_legWalk);
        }

        public void SetLegsAnimationState(bool state)
        {
            if (state)
                m_animancer.Play(m_legWalk);
            else
                m_animancer.Stop(m_legWalk);
        }
    }
}