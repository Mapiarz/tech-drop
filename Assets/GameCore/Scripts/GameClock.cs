using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;

namespace TechDrop.Gameplay
{
    public class GameClock : MonoBehaviour
    {
        [SerializeField] float gameDuration;
        [SerializeField] GameBoard gameBoard;
        [SerializeField] TextMesh textMesh;
        [SerializeField] SpriteRenderer rendererComponent;

        float timeRemaining;
        bool gameStarted;

        string TimeRemainingString { get { return Mathf.CeilToInt( timeRemaining ).ToString( "00" ); } }

        void Awake()
        {
            Assert.IsNotNull( gameBoard );
            Assert.IsNotNull( textMesh );
            Assert.IsNotNull( rendererComponent );

            Assert.IsTrue( gameDuration > 0f );

            // Initialize game clock
            timeRemaining = gameDuration;
            textMesh.text = TimeRemainingString;
        }

        public void StartGame()
        {
            Assert.IsFalse( gameStarted );

            StartCoroutine( UpdateGameClock() );
        }

        IEnumerator UpdateGameClock()
        {
            gameStarted = true;

            while ( timeRemaining > 0f )
            {
                timeRemaining -= Time.deltaTime;
                textMesh.text = TimeRemainingString;
                yield return null;
            }

            timeRemaining = 0f;
            textMesh.text = TimeRemainingString;
        }

        void OnGUI()
        {
            if ( GUI.Button( new Rect( 100, 100, 100, 100 ), "START" ) )
            {
                StartGame();
            }
        }
    }
}
