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

        public void Initialize( GameBoard gameBoard )
        {
            Assert.IsNotNull( gameBoard );

            this.gameBoard = gameBoard;

            isInitialized = true;
        }

        public void Fire()
        {
            //Assert.IsTrue( isInitialized );
            // TODO: Check cooldown
            gameBoard.FireGun( effectArray );
            Debug.Break();
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