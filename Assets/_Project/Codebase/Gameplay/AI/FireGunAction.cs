using _Project.Codebase.Gameplay.Characters;
using _Project.Codebase.Gameplay.World;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Codebase.Gameplay.AI
{
    public class FireGunAction : CharacterAction
    {
        private readonly EnemyCharacter m_enemy;
        private readonly Vector2 m_target;
        public FireGunAction(Character character, WorldRef worldContext, Vector2 target) : base(character, worldContext)
        {
            m_enemy = (EnemyCharacter)character;
            m_target = target;
        }

        protected override async UniTask OnStartAction()
        {
            m_enemy.SetFacingDir(m_target - (Vector2)m_enemy.transform.position);
            await m_enemy.FireGun(m_target);
        }
    }
}