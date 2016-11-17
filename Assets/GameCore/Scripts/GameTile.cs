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
        Vector3 desiredPosition;
        Quaternion desiredRotation;

        GameBoard gameBoard;
        Animator animatorComponent;
        SpriteRenderer rendererComponent;
        bool isMoving = false;

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

        public void MoveTo( Vector3 localPosition, int rotations )
        {
            Assert.IsFalse( isMoving );
            // TODO: Assert isMoving == false?
            if ( isMoving )
                return;

            //Debug.Log( string.Format( "Moving from: {0},{1} to {2},{3}", BoardPosition.Column, BoardPosition.Row, destination.Column, destination.Row ) );
            // TODO: Clean this code up
            //var targetLocalPosition = BoardPositionToLocalPosition( destination );
            //var delta = targetLocalPosition - transform.localPosition;
            //if ( delta.magnitude > 0.01f ) // Could be 0 but we want to avoid float precision errors
            //{
                //var rotations = destination.Row - BoardPosition.Row;
                desiredPosition = localPosition;
                desiredRotation = transform.rotation * Quaternion.Euler( 0, 0, -90 * rotations ); // Rotate by 90 degress clockwise

                //BoardPosition = destination;

                isMoving = true;
            //}
        }

        public void SetColor( TileColor newColor, Sprite newSprite )
        {
            Color = newColor;
            rendererComponent.sprite = newSprite;
        }

        public void Teleport( Vector3 destination )
        {
            //BoardPosition = destination;
            //transform.localPosition = BoardPositionToLocalPosition( destination );
            transform.localPosition = destination;
        }

        public void Initialize( GameBoard game )
        {
            gameBoard = game;
            Assert.IsNotNull( gameBoard );
        }

        void Awake()
        {
            //animatorComponent = transform.GetComponent<Animator>();
            rendererComponent = transform.GetComponent<SpriteRenderer>();
            //Assert.IsNotNull( animatorComponent );
            Assert.IsNotNull( rendererComponent );
        }

        void Update()
        {
            // TODO: Move this to a coroutine
            if ( isMoving )
            {
                var delta = desiredPosition - transform.localPosition;

                // TODO: Change 0.75f to BlockSize or something, accounting for margins and padding
                var timePerBlock = 0.75f / gameBoard.BlockSpeed;  // Time it takes to move by one block
                var degreesPerSecond = 90 / timePerBlock;  // Degrees a second, we need to rotate 90 per each block

                // TODO: Rework to use Lerp instead (possibly having a Mathf.Clamp or something)
                // TODO: The above isn't needed, this works fine.
                //transform.position = Vector3.Lerp ()

                transform.Translate( new Vector3( 0, Time.deltaTime * -gameBoard.BlockSpeed, 0 ), Space.World );
                transform.Rotate( Vector3.forward, Time.deltaTime * -degreesPerSecond, Space.World );


                // 0.75

                // 90 - 1
                // x - 0.75

                // x - 1
                // 90 - 0.75
                if ( transform.localPosition.y <= desiredPosition.y )
                {
                    transform.localPosition = desiredPosition;
                    transform.rotation = desiredRotation;
                    isMoving = false;
                    if ( MovingFinished != null )
                        MovingFinished( this );


                }
            }
        }

        public void OnPointerClick( PointerEventData eventData )
        {
            if ( TileClicked != null )
                TileClicked( this );
        }
    }
}
