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
        [SerializeField]
        TileColor color;
        [SerializeField]
        BoardPosition boardPosition;
        Vector3 desiredPosition;

        GameBoard gameBoard;
        Animator animatorComponent;
        SpriteRenderer rendererComponent;
        bool isMoving = false;

        public event TileEventHandler TileClicked;
        public event TileEventHandler MovingFinished;

        public BoardPosition BoardPosition
        {
            get
            {
                return boardPosition;
            }
            private set
            {
                boardPosition = value;

            }
        }

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

        public void MoveTo( BoardPosition destination )
        {
            if ( isMoving )
                return;

            var delta = destination - boardPosition;
            var size = rendererComponent.bounds.size;
            var localCoordinatesDelta = new Vector3( delta.X, delta.Y * -1 ) * size.y;
            if ( localCoordinatesDelta.magnitude > 0 )
            {
                desiredPosition = transform.localPosition + localCoordinatesDelta;
                BoardPosition = destination;
                isMoving = true;

                RotateClockwise( gameBoard.BlockSpeed, localCoordinatesDelta.magnitude );
            }
        }

        public void SetColor( TileColor newColor, Sprite newSprite )
        {
            Color = newColor;
            rendererComponent.sprite = newSprite;
        }

        public void Teleport( BoardPosition destination )
        {
            BoardPosition = destination;
            transform.localPosition = gameBoard.Anchor + new Vector3( destination.X, destination.Y * -1 ) * rendererComponent.bounds.size.y;
        }

        public void Initialize( GameBoard game )
        {
            gameBoard = game;
            Assert.IsNotNull( gameBoard );
        }

        void Awake()
        {
            animatorComponent = transform.GetComponent<Animator>();
            rendererComponent = transform.GetComponent<SpriteRenderer>();
            Assert.IsNotNull( animatorComponent );
            Assert.IsNotNull( rendererComponent );
        }

        void Update()
        {
            if ( isMoving )
            {
                transform.localPosition = Vector3.MoveTowards( transform.localPosition, desiredPosition, Time.deltaTime * gameBoard.BlockSpeed );
                if ( desiredPosition == transform.localPosition )
                {
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

        private void RotateClockwise( float speed, float distance )
        {
            var animationDuration = speed / Mathf.Abs( distance ); // Time it takes to cover the distance
            animatorComponent.SetFloat( "AnimationDuration", animationDuration );

            if ( transform.rotation.eulerAngles.z == 0 )
                animatorComponent.Play( "RotateClockWiseTo90", 0, 0 );
            else if ( transform.rotation.eulerAngles.z == 360 - 90 )
                animatorComponent.Play( "RotateClockWiseTo180", 0, 0 );
            else if ( transform.rotation.eulerAngles.z == 360 - 180 )
                animatorComponent.Play( "RotateClockWiseTo270", 0, 0 );
            else if ( transform.rotation.eulerAngles.z == 360 - 270 )
                animatorComponent.Play( "RotateClockWiseTo360", 0, 0 );
            else
                Debug.LogWarning( "Rotation of the GameTile % 90 != 0" );
        }
    }
}
