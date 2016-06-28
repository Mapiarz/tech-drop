using UnityEngine;
using System.Collections;
using System;

public class GameTile : MonoBehaviour
{
    [SerializeField]
    TileColor color;
    [SerializeField]
    Vector2 boardPosition;

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




    public void Move( Vector2 destination )
    {
        //TODO: Consider PositionChanged in property setter
        //throw new NotImplementedException();
        var dimension = gameBoard.Dimensions.y;
        var delta = BoardPosition - destination;
        var size = GetComponent<SpriteRenderer>().bounds.size;

        transform.Translate( new Vector3( delta.x * -1, delta.y ) * size.y );
        BoardPosition = destination;
    }

    void Awake()
    {
        gameBoard = transform.parent.GetComponent<GameBoard>();
        Move( new Vector2( 0, 0 ) );
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
        if ( GUI.Button( new Rect( 100, 100, 200, 100 ), "Move me" ) )
        {
            Move( new Vector2( 0, BoardPosition.y ) + Vector2.one );
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
