using System.Collections.Generic;
using _Project.Codebase.Gameplay.World;
using DanonFramework.Runtime.Core.Utilities;
using UnityEngine;

namespace _Project.Codebase.Gameplay
{
    public class EnemyController
    {
        private List<EnemyCharacter> m_enemies = new();
        private TurnController m_turnController;

        public EnemyController(TurnController turnController, Building building)
        {
            m_turnController = turnController;
            
            m_turnController.OnTurnChange += OnTurnChange;
            
            for (int i = 0; i < 3; i++)
            {
                EnemyObject enemyObj = 
                    ContentUtilities.Instantiate<GameObject>(PrefabAssetGroup.ENEMY).GetComponent<EnemyObject>();
                m_enemies.Add(enemyObj.Initialize(building.GetRandomOpenFloor().position));
            }
        }

        private async void OnTurnChange(Turn turn)
        {
            if (turn == Turn.Enemy)
            {
                //WorldScreenshot worldScreenshot = new WorldScreenshot(new Building(ModuleUtilities.Get<GameModule>().Building));
                foreach (EnemyCharacter enemy in m_enemies)
                {
                   await enemy.TakeTurn();
                }
                
                m_turnController.NextTurn();
            }
        }
    }
}