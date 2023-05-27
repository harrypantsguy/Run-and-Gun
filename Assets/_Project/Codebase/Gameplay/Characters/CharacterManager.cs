using System.Collections.Generic;
using _Project.Codebase.AssetGroups;
using _Project.Codebase.Gameplay.World;
using _Project.Codebase.Modules;
using DanonFramework.Core.Utilities;
using UnityEngine;

namespace _Project.Codebase.Gameplay.Characters
{
    public class CharacterManager
    {
        private readonly List<EnemyCharacter> m_enemies = new();
        private readonly TurnController m_turnController;
        private readonly WorldRef m_world;

        public CharacterManager(TurnController turnController, in WorldRef world)
        {
            m_turnController = turnController;
            
            m_turnController.OnTurnChange += OnTurnChange;

            m_world = world;
            
            RunnerObject runnerObj = 
                ContentUtilities.Instantiate<GameObject>(PrefabAssetGroup.RUNNER).GetComponent<RunnerObject>();
            m_world.runner = (Runner)runnerObj.Initialize(Vector2Int.zero);
            
            for (int i = 0; i < 3; i++)
            {
                EnemyObject enemyObj = 
                    ContentUtilities.Instantiate<GameObject>(PrefabAssetGroup.ENEMY).GetComponent<EnemyObject>();
                EnemyCharacter enemy = (EnemyCharacter)enemyObj.Initialize(world.building.GetRandomOpenFloor().position);
                m_enemies.Add(enemy);
            }
        }

        private async void OnTurnChange(Turn turn)
        {
            if (turn == Turn.Enemy)
            {
                foreach (EnemyCharacter enemy in m_enemies)
                {
                    enemy.ResetActionPointsToMax();
                }

                WorldRef world = ModuleUtilities.Get<GameModule>().World;
                foreach (EnemyCharacter enemy in m_enemies)
                {
                   await enemy.TakeTurn(world);
                }
                
                m_turnController.NextTurn();
            }
            else
            {
                m_world.runner.ResetActionPointsToMax();
            }
        }
    }
}