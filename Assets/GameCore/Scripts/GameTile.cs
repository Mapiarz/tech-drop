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

            var localPos = BoardPositionToLocalPosition( destination );
            var delta = localPos - transform.localPosition;
            if ( delta.magnitude > 0.01 )
            {
                Debug.Log( string.Format( "Position: {0},{1}; Destination: {2},{3}; Delta: {4}", BoardPosition.Column, BoardPosition.Row, destination.Column, destination.Row, delta.magnitude ) );
                BoardPosition = destination;
                desiredPosition = localPos;
                isMoving = true;

                RotateClockwise( gameBoard.BlockSpeed, delta.magnitude );
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
