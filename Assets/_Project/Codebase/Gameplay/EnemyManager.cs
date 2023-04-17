using System.Collections.Generic;
using DanonFramework.Runtime.Core.Utilities;

namespace _Project.Codebase.Gameplay
{
    public class EnemyManager
    {
        private List<EnemyCharacter> m_enemies = new();
        
        public EnemyManager(TurnController turnController)
        {
            ContentUtilities.Instantiate(PrefabAssetGroup.CHARACTER);
            turnController.OnTurnChange += OnTurnChange; 
        }

        private void OnTurnChange(Turn turn)
        {
            if (turn == Turn.Enemy)
            {
                foreach (EnemyCharacter enemy in m_enemies)
                {
                    
                }
            }
        }
    }
}