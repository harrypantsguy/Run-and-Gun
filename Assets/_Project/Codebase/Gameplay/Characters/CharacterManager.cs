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
        private readonly List<EnemyCharacter> m_enemies = new();
        public readonly Runner runner;
        private TurnController m_turnController;

        public CharacterManager(TurnController turnController, Building building)
        {
            m_turnController = turnController;
            
            m_turnController.OnTurnChange += OnTurnChange;
            
            for (int i = 0; i < 3; i++)
            {
                EnemyObject enemyObj = 
                    ContentUtilities.Instantiate<GameObject>(PrefabAssetGroup.ENEMY).GetComponent<EnemyObject>();
                EnemyCharacter enemy = (EnemyCharacter)enemyObj.Initialize(building.GetRandomOpenFloor().position);
                m_enemies.Add(enemy);
            }

            RunnerObject runnerObj = 
                ContentUtilities.Instantiate<GameObject>(PrefabAssetGroup.RUNNER).GetComponent<RunnerObject>();
            runner = (Runner)runnerObj.Initialize(Vector2Int.zero);
        }

        private async void OnTurnChange(Turn turn)
        {
            if (turn == Turn.Enemy)
            {
                foreach (EnemyCharacter enemy in m_enemies)
                {
                    enemy.actionPoints = enemy.MaxActionPoints;
                }

                WorldScreenshot world = ModuleUtilities.Get<GameModule>().World;
                foreach (EnemyCharacter enemy in m_enemies)
                {
                   await enemy.TakeTurn(world);
                }
                
                m_turnController.NextTurn();
            }
            else
            {
                runner.actionPoints = runner.MaxActionPoints;
            }
        }
    }
}