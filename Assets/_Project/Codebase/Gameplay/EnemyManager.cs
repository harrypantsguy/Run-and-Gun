using System.Collections.Generic;
using _Project.Codebase.Gameplay.World;
using DanonFramework.Runtime.Core.Utilities;
using UnityEngine;

namespace _Project.Codebase.Gameplay
{
    public class EnemyManager
    {
        private List<EnemyCharacter> m_enemies = new();
        
        public EnemyManager(TurnController turnController)
        {
            ContentUtilities.Instantiate<GameObject>(PrefabAssetGroup.CHARACTER);
            turnController.OnTurnChange += OnTurnChange; 
        }

        private void OnTurnChange(Turn turn)
        {
            if (turn == Turn.Enemy)
            {
                WorldScreenshot worldScreenshot = new WorldScreenshot();
                foreach (EnemyCharacter enemy in m_enemies)
                {
                    enemy.DecideTurn(ref worldScreenshot);
                }
            }
        }
    }
}