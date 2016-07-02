using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;
using System.Collections.Generic;
using System;
using System.Linq;

namespace TechDrop.Gameplay
{
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

        GameTile[,] tiles;

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

            tiles = new GameTile[( int )Dimensions.x, ( int )Dimensions.y];

            for ( int i = 0; i < Dimensions.x; i++ )
            {
                for ( int j = 0; j < Dimensions.y; j++ )
                {
                    var tileGameObject = UnityEngine.Object.Instantiate( Resources.Load( "Game Tile" ) ) as GameObject;
                    tileGameObject.transform.SetParent( transform );

                    tiles[i, j] = tileGameObject.GetComponent<GameTile>();
                    tiles[i, j].Initialize( this );
                    tiles[i, j].Teleport( new Vector2( i, j ) );

                }
            }
        }


        void OnGUI()
        {
            if ( GUI.Button( new Rect( 0, 100, 200, 100 ), "Change Color" ) )
            {
                transform.FindChild( "Game Tile" ).GetComponent<GameTile>().SetColor( TileColor.Red, TileColors.First( x => x.Color == TileColor.Red ).Sprite );
            }
        }
    }
}
