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
        [SerializeField] Vector2 dimensions;
        [SerializeField] Vector3 anchor;
        [SerializeField] float blockSpeed = 1f;
        [SerializeField] List<TileSprite> tileColors = new List<TileSprite>();

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
            Assert.IsTrue( tileColors.Count > 0 );

            var random = new System.Random();

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
                    tiles[i, j].TileClicked += GameBoard_TileClicked;

                    var colorIndex = random.Next( 0, tileColors.Count );
                    var randomColor = tileColors[colorIndex];
                    tiles[i, j].SetColor( randomColor.Color, randomColor.Sprite );
                }
            }
        }

        private void GameBoard_TileClicked( GameTile tile )
        {
            if ( tile != null )
                tile.MoveTo( tile.BoardPosition + Vector2.one );
        }
    }
}
