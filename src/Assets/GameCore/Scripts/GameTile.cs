using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine.Assertions;

namespace TechDrop.Gameplay
{
    [RequireComponent( typeof( Collider2D ), typeof( SpriteRenderer ) )]
    public class GameTile : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] TileColor color;

        bool isMoving = false;

        GameBoard gameBoard;
        SpriteRenderer rendererComponent;

        public event TileEventHandler TileClicked;
        public event TileEventHandler MovingFinished;

        public TileColor Color
        {
            get
            {
                return color;
            }

            private set
            {
                color = value;
            }
        }

        public void Initialize( GameBoard game )
        {
            gameBoard = game;
            Assert.IsNotNull( gameBoard );
        }

        void Awake()
        {
            rendererComponent = transform.GetComponent<SpriteRenderer>();
            Assert.IsNotNull( rendererComponent );
        }

        public void SetColor( TileColor newColor, Sprite newSprite )
        {
            Color = newColor;
            rendererComponent.sprite = newSprite;
        }

        public void Teleport( Vector3 destination )
        {
            transform.localPosition = destination;
        }

        public void MoveTo( Vector3 localPosition, int rotations )
        {
            Assert.IsFalse( isMoving );

            // Rotate by 90 degress clockwise
            var targetRotation = transform.rotation * Quaternion.Euler( 0, 0, -90 * rotations );

            StartCoroutine( Move( localPosition, targetRotation ) );
        }

        IEnumerator Move( Vector3 targetPosition, Quaternion targetRotation )
        {
            isMoving = true;
            var timePerBlock = gameBoard.VerticalBlockSize / gameBoard.BlockSpeed;  // Time it takes to move by one block
            var degreesPerSecond = 90 / timePerBlock;  // Degrees a second, we need to rotate 90 per each block

            do
            {
                yield return null;
                transform.Translate( new Vector3( 0, Time.deltaTime * -gameBoard.BlockSpeed, 0 ), Space.World );
                transform.Rotate( Vector3.forward, Time.deltaTime * -degreesPerSecond, Space.World );

            } while ( transform.localPosition.y > targetPosition.y );

            transform.localPosition = targetPosition;
            transform.rotation = targetRotation;
            isMoving = false;
            if ( MovingFinished != null )
                MovingFinished( this );
        }

        public void OnPointerClick( PointerEventData eventData )
        {
            if ( TileClicked != null )
                TileClicked( this );
        }
    }
}
