﻿using UnityEngine;
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
        Quaternion desiredRotation;

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

        Vector3 destinationRotation;
        Vector3 positionDelta;

        public void MoveTo( BoardPosition destination )
        {
            if ( isMoving )
                return;

            //Debug.Log( string.Format( "Moving from: {0},{1} to {2},{3}", BoardPosition.Column, BoardPosition.Row, destination.Column, destination.Row ) );

            var targetLocalPosition = BoardPositionToLocalPosition( destination );
            var delta = targetLocalPosition - transform.localPosition;
            if ( delta.magnitude > 0.01f ) // Could be 0 but we want to avoid float precision errors
            {
                var rotations = destination.Row - BoardPosition.Row;
                BoardPosition = destination;
                desiredPosition = targetLocalPosition;
                positionDelta = desiredPosition - transform.localPosition;
                destinationRotation = new Vector3( 0, 0, -90 * rotations );
                desiredRotation = transform.rotation * Quaternion.Euler( 0, 0, -90 * rotations ); // Rotate by 90 degress clockwise
                isMoving = true;
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
            transform.localPosition = BoardPositionToLocalPosition( destination );
        }

        private Vector3 BoardPositionToLocalPosition( BoardPosition pos )
        {
            var size = rendererComponent.bounds.size;
            float columnPadding = ( gameBoard.GameBoardArea.width - ( gameBoard.BoardDimensions.Column * size.x ) ) / ( gameBoard.BoardDimensions.Column + 1 );
            float rowPadding = ( gameBoard.GameBoardArea.height - ( gameBoard.BoardDimensions.Row * size.x ) ) / ( gameBoard.BoardDimensions.Row + 1 );
            Vector3 padding = new Vector3( columnPadding * ( pos.Column + 1 ), rowPadding * ( pos.Row + 1 ) * -1 );
            Vector3 boardTopLeftAnchor = new Vector3( gameBoard.GameBoardArea.position.x, gameBoard.GameBoardArea.position.y ) - gameBoard.transform.localPosition;
            Vector3 boardPosition = new Vector3( pos.Column * size.x, pos.Row * size.y * -1 );
            Vector3 tileSizeOffset = new Vector3( size.x / 2, -1 * size.x / 2 );
            Vector3 localPosition = boardTopLeftAnchor + boardPosition + padding + tileSizeOffset;

            return localPosition;
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
                transform.Translate( new Vector3( 0, Time.deltaTime * -gameBoard.BlockSpeed, 0 ), Space.World );
                transform.Rotate( Vector3.forward, Time.deltaTime * -degreesPerSecond, Space.World );


                // 0.75

                // 90 - 1
                // x - 0.75

                // x - 1
                // 90 - 0.75

                //transform.localPosition = Vector3.MoveTowards( transform.localPosition, desiredPosition, Time.deltaTime * gameBoard.BlockSpeed );

                // 90 degrees over the distance to cover, multiplied by block speed and finally by deltaTime.
                // TODO: replace 0.75 with a proper formula
                //var speed = 90f * gameBoard.BlockSpeed;
                //var speed = 90 * gameBoard.BlockSpeed;

                //transform.Rotate( Vector3.forward, speed * Time.deltaTime, Space.Self );
                //transform.Rotate( desiredRotation.eulerAngles * speed * Time.deltaTime );
                //transform.rotation = Quaternion.RotateTowards( transform.rotation, desiredRotation, speed * Time.deltaTime );
                if ( transform.localPosition.y <= desiredPosition.y )
                {
                    transform.localPosition = desiredPosition;
                    transform.rotation = desiredRotation;
                    isMoving = false;
                    if ( MovingFinished != null )
                        MovingFinished( this );


                }
                // If we finished rotating by 90 degrees, rotate again
                //if ( transform.rotation == desiredRotation )
                //{

                //    desiredRotation *= Quaternion.Euler( 0, 0, -90 );
                //    //transform.rotation = Quaternion.RotateTowards( transform.rotation, desiredRotation, speed * Time.deltaTime );
                //    //transform.rotation = Quaternion.RotateTowards( transform.rotation, desiredRotation, speed * Time.deltaTime );
                //}


            }
            //else
            //{
            //    var speed = ( 90 / 0.75f ) * gameBoard.BlockSpeed;
            //    //Debug.Log( string.Format( "Speed: {0}", speed ) );
            //    transform.rotation = Quaternion.RotateTowards( transform.rotation, desiredRotation, speed * Time.deltaTime );
            //}
        }

        public void OnPointerClick( PointerEventData eventData )
        {
            if ( TileClicked != null )
                TileClicked( this );
        }
    }
}
