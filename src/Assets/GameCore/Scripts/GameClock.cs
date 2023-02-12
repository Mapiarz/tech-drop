using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;

namespace TechDrop.Gameplay
{
    public class GameClock : MonoBehaviour
    {
        [SerializeField] float gameDuration;
        [SerializeField] GameBoard gameBoard;

        public float TimeRemaining { get; private set; }
        bool gameStarted;

        void Awake()
        {
            Assert.IsNotNull( gameBoard );
            Assert.IsTrue( gameDuration > 0f );

            // Initialize game clock
            TimeRemaining = gameDuration;
        }

        void Update()
        {
            // For accuracy time gets updated on every frame
            if ( gameStarted && TimeRemaining != 0f )
            {
                TimeRemaining -= Time.deltaTime;

                if ( TimeRemaining < 0f )  // For clarity, set the remaining time to 0 and disable game
                {
                    TimeRemaining = 0f;
                    gameBoard.GameEnabled = false;
                }
            }
        }

        public void StartGame()
        {
            Assert.IsFalse( gameStarted );

            gameBoard.GameEnabled = true;
            gameStarted = true;
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
