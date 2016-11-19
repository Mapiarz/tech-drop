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
        [SerializeField] Sprite tickSprite;
        [SerializeField] Sprite tockSprite;

        float timeRemaining;
        bool gameStarted;

        void Awake()
        {
            Assert.IsNotNull( gameBoard );
            Assert.IsNotNull( textMesh );
            Assert.IsNotNull( rendererComponent );
            Assert.IsNotNull( tickSprite );
            Assert.IsNotNull( tockSprite );

            Assert.IsTrue( gameDuration > 0f );

            // Initialize game clock
            timeRemaining = gameDuration;
            textMesh.text = Mathf.CeilToInt( timeRemaining ).ToString( "00" );
            rendererComponent.sprite = tickSprite;
        }

        void Update()
        {
            // For accuracy time gets updated on every frame but text mesh and sprites get updated in coroutine
            if ( gameStarted && timeRemaining != 0f )
            {
                timeRemaining -= Time.deltaTime;

                if ( timeRemaining < 0f )  // For clarity, set the time to cool down to 0
                {
                    timeRemaining = 0f;
                }
            }
        }

        public void StartGame()
        {
            Assert.IsFalse( gameStarted );

            StartCoroutine( UpdateGameClock() );
        }

        IEnumerator UpdateGameClock()
        {
            gameStarted = true;
            var lastKnownTime = Mathf.CeilToInt( timeRemaining );

            while ( timeRemaining > 0f )
            {
                var secondsRemaining = Mathf.CeilToInt( timeRemaining );

                if ( secondsRemaining != lastKnownTime )
                {
                    if ( rendererComponent.sprite == tickSprite )
                    {
                        rendererComponent.sprite = tockSprite;
                    }
                    else
                    {
                        rendererComponent.sprite = tickSprite;
                    }

                    textMesh.text = secondsRemaining.ToString( "00" );

                    lastKnownTime = secondsRemaining;
                }

                yield return null;
            }

            textMesh.text = "00";
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
