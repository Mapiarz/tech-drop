using UnityEngine;
using System.Collections;
using System;

public class GameTile : MonoBehaviour
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


    bool isMoving = false;

    public void Move( Vector2 destination )
    {
        if ( isMoving )
            return;
        //TODO: Consider PositionChanged in property setter
        var dimension = gameBoard.Dimensions.y;
        var delta = destination - boardPosition;
        var size = GetComponent<SpriteRenderer>().bounds.size;
        startPos = transform.localPosition;
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


    Vector3 startPos = Vector3.zero;

    void Update()
    {
        if ( isMoving )
        {
            //var distanceToCover = desiredPosition - startPos;
            //var distanceCovered = desiredPosition - transform.localPosition;
            //var completion = distanceCovered.y / distanceToCover.y;
            //Debug.Log( completion );
            var deltaTime = Time.deltaTime;
            var delta = desiredPosition - transform.localPosition;
            if ( delta.magnitude <= 0.01 * gameBoard.BlockSpeed )
            {
                transform.localPosition = desiredPosition;
                isMoving = false;
            }
            else
                //transform.localEulerAngles = Vector3.MoveTowards( transform.localPosition, desiredPosition, deltaTime * gameBoard.BlockSpeed );
                //transform.localPosition = Vector3.MoveTowards( transform.localPosition, desiredPosition, deltaTime * gameBoard.BlockSpeed * gameBoard.MoveCurve.Evaluate( completion ) );
                transform.Translate( new Vector3( delta.x, delta.y ).normalized * gameBoard.BlockSpeed * deltaTime );
        }
    }

    void OnGUI()
    {
        if ( GUI.Button( new Rect( 100, 100, 200, 100 ), "Move me" ) )
        {
            Move( new Vector2( 0, BoardPosition.y + 1 ) );
        }
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
