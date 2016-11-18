using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;

namespace TechDrop.Gameplay
{
    public class GameGun : MonoBehaviour
    {
        [SerializeField] float coolDown;
        [SerializeField] BoolMatrix effectArray;
        [SerializeField] GameBoard gameBoard;

        bool isInitialized;
        float timeToCoolDown;

        bool IsCoolingDown { get { return timeToCoolDown > 0f; } }

        public void Initialize( GameBoard gameBoard )
        {
            Assert.IsNotNull( gameBoard );

            this.gameBoard = gameBoard;

            isInitialized = true;
        }

        public void Fire()
        {
            // TODO: This method could possibly return bool

            if ( IsCoolingDown )
            {
                return;
            }

            var fireSuccessful = gameBoard.FireGun( effectArray );
            timeToCoolDown = coolDown;
            Debug.Log( string.Format( "Fire successful {0}", fireSuccessful ) );
        }

        void Update()
        {
            // TODO: Move to coroutine?
            // Cool the gun down
            if ( IsCoolingDown )
            {
                timeToCoolDown -= Time.deltaTime;
            }
        }

        void OnGUI()
        {
            if ( GUI.Button( new Rect( 100, 100, 100, 100 ), "Fire" ) )
            {
                Fire();
            }
        }
    }
}