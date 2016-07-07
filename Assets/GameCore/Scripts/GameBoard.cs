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
        [SerializeField] BoardPosition boardDimensions;
        [SerializeField] Rect gameBoardArea;
        [SerializeField] float blockSpeed = 1f;
        [SerializeField] int neighbourThreshold = 3;
        [SerializeField] List<TileSprite> tileColors = new List<TileSprite>();

        GameTile[,] tiles;

        public BoardPosition BoardDimensions
        {
            get
            {
                return boardDimensions;
            }

            set
            {
                boardDimensions = value;
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

        public Rect GameBoardArea
        {
            get
            {
                return gameBoardArea;
            }

            set
            {
                gameBoardArea = value;
            }
        }

        void Awake()
        {
            Assert.IsTrue( BlockSpeed > 0f );
            Assert.IsTrue( tileColors.Count > 0 );

            var random = new System.Random();

            tiles = new GameTile[BoardDimensions.Column, BoardDimensions.Row];

            for ( int i = 0; i < BoardDimensions.Column; i++ )
            {
                for ( int j = 0; j < BoardDimensions.Row; j++ )
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
            var sameColorNeighbours = FindNeighbours( tile, new List<GameTile>() );
            Debug.Log( "Neighbour count: " + sameColorNeighbours.Count.ToString() );

            // Destroy the tiles
            if ( sameColorNeighbours.Count >= neighbourThreshold )
            {
                foreach ( var item in sameColorNeighbours )
                {
                    tiles[item.BoardPosition.Column, item.BoardPosition.Row] = null;
                }

                // Go over colums which had blocks destroyed and update them
                foreach ( var destroyedBlock in sameColorNeighbours )
                {
                    for ( int i = destroyedBlock.BoardPosition.Row - 1; i >= 0; i-- ) // Start from the bottom so we don't overwrite existing blocks
                    {
                        var tileToShift = tiles[destroyedBlock.BoardPosition.Column, i];
                        if ( tileToShift != null )
                        {
                            var shiftBy = BlocksDestroyedBelow( tileToShift.BoardPosition );
                            tiles[tileToShift.BoardPosition.Column, tileToShift.BoardPosition.Row] = null;
                            tiles[tileToShift.BoardPosition.Column, tileToShift.BoardPosition.Row + shiftBy] = tileToShift;
                            tileToShift.MoveTo( new BoardPosition( tileToShift.BoardPosition.Column, tileToShift.BoardPosition.Row + shiftBy ) );
                        }
                    }
                }

                foreach ( var item in sameColorNeighbours )
                {
                    //GameObject.Destroy( item.gameObject );
                    item.gameObject.SetActive( false );
                }
            }

            //TODO: Add finished moving callback so we cant click, if something is moving
            //TODO: Add 90 degree rotation per row fallen.
        }

        private int BlocksDestroyedBelow( BoardPosition pos )
        {
            int result = 0;
            for ( int i = pos.Row; i < BoardDimensions.Row; i++ )
            {
                if ( tiles[pos.Column, i] == null )
                    result++;
            }

            return result;
        }

        private List<GameTile> FindNeighbours( GameTile tile, List<GameTile> alreadyVisited )
        {
            if ( tile == null || alreadyVisited.Contains( tile ) )
                return alreadyVisited;

            alreadyVisited.Add( tile );
            var neighbours = GetImmidiateNeighbours( tile );

            foreach ( var neighbour in neighbours )
            {
                if ( neighbour.Color == tile.Color )
                {
                    FindNeighbours( neighbour, alreadyVisited );
                }
            }

            return alreadyVisited;
        }

        private List<GameTile> GetImmidiateNeighbours( GameTile tile )
        {
            var neighbours = new List<GameTile>();

            int positionX = tile.BoardPosition.Column;
            int positionY = tile.BoardPosition.Row;
            int maxColumnIndex = BoardDimensions.Column - 1;
            int maxRowIndex = BoardDimensions.Row - 1;

            if ( positionY < maxRowIndex )
                if ( tiles[positionX, positionY + 1] != null )
                    neighbours.Add( tiles[positionX, positionY + 1] );
            if ( positionY > 0 )
                if ( tiles[positionX, positionY - 1] != null )
                    neighbours.Add( tiles[positionX, positionY - 1] );
            if ( positionX < maxColumnIndex )
                if ( tiles[positionX + 1, positionY] != null )
                    neighbours.Add( tiles[positionX + 1, positionY] );
            if ( positionX > 0 )
                if ( tiles[positionX - 1, positionY] != null )
                    neighbours.Add( tiles[positionX - 1, positionY] );

            return neighbours;
        }
    }
}
