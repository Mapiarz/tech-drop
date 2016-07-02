﻿using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine.Assertions;

[RequireComponent( typeof( Collider2D ), typeof( SpriteRenderer ) )]
public class GameTile : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    TileColor color;
    [SerializeField]
    Vector2 boardPosition;
    Vector3 desiredPosition;

    GameBoard gameBoard;
    Animator animatorComponent;

    public Vector2 BoardPosition
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

        set
        {
            color = value;
            //TODO: ColorChanged();
        }
    }


    //TODO: Expose property getter
    //TODO: Expose events finishedMoving
    bool isMoving = false;

    public void Move( Vector2 destination )
    {
        if ( isMoving )
            return;
        //TODO: Consider PositionChanged in property setter
        var delta = destination - boardPosition;
        var size = GetComponent<SpriteRenderer>().bounds.size;
        desiredPosition = transform.localPosition + ( new Vector3( delta.x, delta.y * -1 ) * size.y );
        BoardPosition = destination;
        isMoving = true;

        RotateClockwise();
    }

    private void Teleport( Vector2 destination )
    {
        transform.localPosition = Vector3.Scale( gameBoard.Anchor, ( new Vector3( destination.x, destination.y ) + Vector3.one ) );
    }

    void Awake()
    {
        gameBoard = transform.parent.GetComponent<GameBoard>();
        animatorComponent = transform.GetComponent<Animator>();
        Assert.IsNotNull( gameBoard );
        Assert.IsNotNull( animatorComponent );

        Teleport( new Vector2( 0, 0 ) );
    }

    void Update()
    {
        if ( isMoving )
        {
            transform.localPosition = Vector3.MoveTowards( transform.localPosition, desiredPosition, Time.deltaTime * gameBoard.BlockSpeed );
            if ( desiredPosition == transform.localPosition )
                isMoving = false;
        }
    }

    public void OnPointerClick( PointerEventData eventData )
    {
        //Debug.Log( "Clicked " + gameObject.name );
        Move( new Vector2( 0, BoardPosition.y + 1 ) );
    }

    private void RotateClockwise()
    {
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

    public enum TileColor
    {
        Red,
        Purple,
        Green,
        Blue,
        Grey
    }
}
