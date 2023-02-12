using System;
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
            var tilesCount = e.Count();
            Score += (int)( ( tilesCount * 10 ) * ( 1 + 0.1f * tilesCount ) );
            ScoreUpdated?.Invoke( this, Score );
        }
    }
}
