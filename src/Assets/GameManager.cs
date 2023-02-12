using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace TechDrop.Gameplay
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] GameBoard gameBoard;
        [SerializeField] GameClock gameClock;
        // TODO: GameScorer;

        public GameBoard Board { get => gameBoard; }
        public GameClock Clock { get => gameClock; }

        void Awake()
        {
            Assert.IsNotNull( gameBoard );
            Assert.IsNotNull( gameClock );
            gameClock.ClockFinished += GameClock_ClockFinished;
        }

        private void GameClock_ClockFinished( object sender, System.EventArgs e )
        {
            gameBoard.StopGame();
        }

        public void StartGame()
        {
            Assert.IsFalse( gameBoard.GameEnabled );
            Assert.IsFalse( gameClock.IsClockRunning );
            gameBoard.StartGame();
            gameClock.StartClock();
        }

        public void ResetGame()
        {
            if ( gameBoard.GameEnabled )
            {
                gameBoard.StopGame();
            }

            if ( gameClock.IsClockRunning )
            {
                gameClock.StopClock();
            }

            gameClock.ResetClock();
        }
    }
}
