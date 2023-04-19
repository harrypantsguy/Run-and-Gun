using System;

namespace _Project.Codebase.Gameplay
{
    public class TurnController
    {
        public Turn Turn { get; private set; }

        public event Action<Turn> OnTurnChange;
        
        private readonly int m_numTurns;

        public TurnController()
        {
            Turn = Turn.Enemy;
            m_numTurns = Enum.GetNames(typeof(Turn)).Length;
            
            OnTurnChange = turn => { };
        }

        public void StartGame()
        {
            OnTurnChange?.Invoke(Turn);
        }

        public void NextTurn()
        {
            Turn = (Turn)(((int)Turn + 1) % m_numTurns);
            OnTurnChange?.Invoke(Turn);
        }
    }
}