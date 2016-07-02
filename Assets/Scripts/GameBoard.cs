using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;
using System.Collections.Generic;
using System;
using System.Linq;

[Serializable]
public class GameBoard : MonoBehaviour
{
    [SerializeField]
    Vector2 dimensions;
    [SerializeField]
    Vector3 anchor;
    [SerializeField]
    float blockSpeed = 1f;
    [SerializeField]
    List<TileSprite> TileColors = new List<TileSprite>();

    public Vector2 Dimensions
    {
        get
        {
            return dimensions;
        }

        set
        {
            dimensions = value;
        }
    }

    public Vector3 Anchor
    {
        get
        {
            return anchor;
        }

        set
        {
            anchor = value;
        }
    }

    public float BlockSpeed
    {
        get
        {
            return blockSpeed;
        }

        set
        {
            blockSpeed = value;
        }
    }

    void Awake()
    {
        Assert.IsTrue( BlockSpeed > 0f );
    }

    void OnGUI()
    {
        if ( GUI.Button( new Rect( 100, 100, 200, 100 ), "Change Color" ) )
        {
            transform.FindChild( "Game Tile" ).GetComponent<GameTile>().SetColor( TileColor.Red, TileColors.First( x => x.Color == TileColor.Red ).Sprite );
        }
    }
}
