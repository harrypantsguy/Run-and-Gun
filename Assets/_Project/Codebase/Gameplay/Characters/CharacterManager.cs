using System.Collections.Generic;
using _Project.Codebase.AssetGroups;
using _Project.Codebase.Gameplay.World;
using _Project.Codebase.Modules;
using DanonFramework.Runtime.Core.Utilities;
using UnityEngine;

namespace _Project.Codebase.Gameplay.Characters
{
    public class CharacterManager
    {
        private List<EnemyCharacter> m_enemies = new();
        private Runner m_runner;
        private TurnController m_turnController;

        public CharacterManager(TurnController turnController, Building building)
        {
            m_turnController = turnController;
            
            m_turnController.OnTurnChange += OnTurnChange;
            
            for (int i = 0; i < 3; i++)
            {
                EnemyObject enemyObj = 
                    ContentUtilities.Instantiate<GameObject>(PrefabAssetGroup.ENEMY).GetComponent<EnemyObject>();
                m_enemies.Add((EnemyCharacter)enemyObj.Initialize(building.GetRandomOpenFloor().position));
            }

            RunnerObject runnerObj = 
                ContentUtilities.Instantiate<GameObject>(PrefabAssetGroup.RUNNER).GetComponent<RunnerObject>();
            m_runner = (Runner)runnerObj.Initialize(Vector2Int.zero);
        }

        private async void OnTurnChange(Turn turn)
        {
            if (turn == Turn.Enemy)
            {
                foreach (EnemyCharacter enemy in m_enemies)
                {
                    enemy.actionPoints = enemy.MaxMaxActionPoints;
                }
                
                WorldScreenshot worldScreenshot = new WorldScreenshot(ModuleUtilities.Get<GameModule>().Building);
                foreach (EnemyCharacter enemy in m_enemies)
                {
                   await enemy.TakeTurn(worldScreenshot);
                }
                
                m_turnController.NextTurn();
            }
            else
            {
                m_runner.actionPoints = m_runner.MaxMaxActionPoints;
            }
        }
    }
}