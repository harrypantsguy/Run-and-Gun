using System.Collections;
using System.Collections.Generic;
using _Project.Codebase.Gameplay.World;
using _Project.Codebase.Modules;
using Cysharp.Threading.Tasks;
using DanonFramework.Runtime.Core.Utilities;
using UnityEngine;

namespace _Project.Codebase.Gameplay
{
    public class EnemyController
    {
        private List<EnemyCharacter> m_enemies = new();
        private TurnController m_turnController;
        
        public EnemyController(TurnController turnController)
        {
            m_turnController = turnController;
            
            ContentUtilities.Instantiate<GameObject>(PrefabAssetGroup.ENEMY);
            m_turnController.OnTurnChange += OnTurnChange;

            for (int i = 0; i < 3; i++)
            {
                EnemyObject enemyObj = 
                    ContentUtilities.Instantiate<GameObject>(PrefabAssetGroup.ENEMY).GetComponent<EnemyObject>();
                m_enemies.Add(enemyObj.Initialize());
            }
        }

        private void OnTurnChange(Turn turn)
        {
            if (turn == Turn.Enemy)
            {
                WorldScreenshot worldScreenshot = new WorldScreenshot(new Building(ModuleUtilities.Get<GameModule>().Building));
                foreach (EnemyCharacter enemy in m_enemies)
                    enemy.DecideTurn(worldScreenshot);
                
            }
        }
    }
}