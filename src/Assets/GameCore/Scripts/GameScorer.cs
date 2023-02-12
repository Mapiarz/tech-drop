using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace TechDrop.Gameplay
{
    public class GameScorer : MonoBehaviour
    {
        [SerializeField] GameBoard gameBoard;

        public int Score { get; private set; }

        public event EventHandler<int> ScoreUpdated;

        void Awake()
        {
            Assert.IsNotNull( gameBoard );

            gameBoard.TilesDestroyed += GameBoard_TilesDestroyed;
        }

        public void ResetScore()
        {
            Score = 0;
            ScoreUpdated?.Invoke( this, Score );
        }

        void GameBoard_TilesDestroyed( object sender, IEnumerable<BoardPosition> e )
        {
            Score += e.ToList().Count;
            ScoreUpdated?.Invoke( this, Score );
        }
    }
}
