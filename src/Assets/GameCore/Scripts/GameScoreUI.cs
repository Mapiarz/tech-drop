using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace TechDrop.Gameplay
{
    public class GameScoreUI : MonoBehaviour
    {
        [SerializeField] TextMesh textMesh;
        [SerializeField] GameScorer gameScorer;

        void Awake()
        {
            Assert.IsNotNull( textMesh );
            Assert.IsNotNull( gameScorer );

            gameScorer.ScoreUpdated += GameScorer_ScoreUpdated;
        }

        void GameScorer_ScoreUpdated( object sender, int e )
        {
            textMesh.text = $"{e:0000000}";    
        }
    }
}
