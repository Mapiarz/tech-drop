using UnityEngine;
using UnityEngine.Assertions;

namespace TechDrop.Gameplay
{
    public class StartGameButton : MonoBehaviour
    {
        [SerializeField] string gameStoppedText = "start";
        [SerializeField] string gameRunningText = "reset";
        [SerializeField] TextMesh textMesh;
        [SerializeField] GameManager gameManager;

        bool gameRunning = false;

        void Awake()
        {
            Assert.IsNotNull( gameManager );
            Assert.IsNotNull( textMesh );
        }

        public void ButtonPressed()
        {
            if ( !gameRunning )
            {
                gameRunning = true;
                gameManager.StartGame();
                textMesh.text = gameRunningText;
            }
            else
            {
                gameRunning = false;
                gameManager.ResetGame();
                textMesh.text = gameStoppedText;

            }
        }
    }
}
