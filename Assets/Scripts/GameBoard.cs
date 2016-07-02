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
        [SerializeField] BoardPosition dimensions;
        [SerializeField] Vector3 anchor;
        [SerializeField] float blockSpeed = 1f;
        [SerializeField] List<TileSprite> tileColors = new List<TileSprite>();

        GameTile[,] tiles;

        public BoardPosition Dimensions
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

            tiles = new GameTile[Dimensions.X, Dimensions.Y];

            for ( int i = 0; i < Dimensions.X; i++ )
            {
                for ( int j = 0; j < Dimensions.Y; j++ )
                {
                    var tileGameObject = UnityEngine.Object.Instantiate( Resources.Load( "Game Tile" ) ) as GameObject;
                    tileGameObject.transform.SetParent( transform );

                    tiles[i, j] = tileGameObject.GetComponent<GameTile>();
                    tiles[i, j].Initialize( this );
                    tiles[i, j].Teleport( new BoardPosition( i, j ) );
                    tiles[i, j].TileClicked += GameBoard_TileClicked;

                    var colorIndex = random.Next( 0, tileColors.Count );
                    var randomColor = tileColors[colorIndex];
                    tiles[i, j].SetColor( randomColor.Color, randomColor.Sprite );
                }
            }
        }

        private void GameBoard_TileClicked( GameTile tile )
        {
            //if ( tile != null )
            //    tile.MoveTo( tile.BoardPosition + Vector2.one );

            //Debug.Log( neighbours.Count );

            var sameColorNeighbours = FindNeighbours( tile, new List<GameTile>() );
            Debug.Log( "Neighbour count: " + sameColorNeighbours.Count.ToString() );

            foreach ( var item in sameColorNeighbours )
            {
                GameObject.Destroy( item.gameObject );

            }
        }

        private List<GameTile> FindNeighbours( GameTile tile, List<GameTile> alreadyVisited )
        {
            if ( alreadyVisited.Contains( tile ) )
                return alreadyVisited;

            alreadyVisited.Add( tile );
            var neighbours = GetImmidiateNeighbours( tile );

            foreach ( var neighbour in neighbours )
            {
                if(neighbour.Color == tile.Color)
                {
                    FindNeighbours( neighbour, alreadyVisited );
                }
            }

            return alreadyVisited;
        }

        private List<GameTile> GetImmidiateNeighbours(GameTile tile)
        {
            var result = new List<GameTile>();

            int positionX = tile.BoardPosition.X;
            int positionY = tile.BoardPosition.Y;
            int maxColumnIndex = Dimensions.X - 1;
            int maxRowIndex = Dimensions.Y - 1;


            if ( positionY < maxRowIndex )
                result.Add( tiles[positionX, positionY + 1] );
            if ( positionY > 0 )
                result.Add( tiles[positionX, positionY - 1] );
            if ( positionX < maxColumnIndex )
                result.Add( tiles[positionX + 1, positionY] );
            if ( positionX > 0 )
                result.Add( tiles[positionX - 1, positionY] );

            return result;
        }
    }
}
