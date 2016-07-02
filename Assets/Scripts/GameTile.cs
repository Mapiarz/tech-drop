using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using UnityEngine.EventSystems;

public class GameTile : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    TileColor color;
    [SerializeField]
    Vector2 boardPosition;
    Vector3 desiredPosition;

    GameBoard gameBoard;

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
    }

    private void Teleport( Vector2 destination )
    {
        transform.localPosition = Vector3.Scale( gameBoard.Anchor, ( new Vector3( destination.x, destination.y ) + Vector3.one ) );
    }

    void Awake()
    {
        gameBoard = transform.parent.GetComponent<GameBoard>();
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

    public enum TileColor
    {
        Red,
        Purple,
        Green,
        Blue,
        Grey
    }
}
