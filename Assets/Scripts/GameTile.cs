using UnityEngine;
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
    SpriteRenderer rendererComponent;
    //TODO: Expose property getter
    //TODO: Expose events finishedMoving
    bool isMoving = false;

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

        private set
        {
            color = value;
        }
    }

    public void Move( Vector2 destination )
    {
        //TODO: Make this MoveTo and this should depend on Dimensions
        if ( isMoving )
            return;

        var delta = destination - boardPosition;
        var size = GetComponent<SpriteRenderer>().bounds.size;
        var localCoordinatesDelta = new Vector3( delta.x, delta.y * -1 ) * size.y;
        desiredPosition = transform.localPosition + localCoordinatesDelta;
        BoardPosition = destination;
        isMoving = true;

        RotateClockwise( gameBoard.BlockSpeed, localCoordinatesDelta.y );
    }

    public void SetColor( TileColor newColor, Sprite newSprite )
    {
        Color = newColor;

        rendererComponent.sprite = newSprite;
    }

    private void Teleport( Vector2 destination )
    {
        transform.localPosition = Vector3.Scale( gameBoard.Anchor, ( new Vector3( destination.x, destination.y ) + Vector3.one ) );
    }

    void Awake()
    {
        gameBoard = transform.parent.GetComponent<GameBoard>();
        animatorComponent = transform.GetComponent<Animator>();
        rendererComponent = transform.GetComponent<SpriteRenderer>();
        Assert.IsNotNull( gameBoard );
        Assert.IsNotNull( animatorComponent );
        Assert.IsNotNull( rendererComponent );

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
