﻿using UnityEngine;
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
        bool isLocked = false;
        int blocksMoving = 0;
        System.Random random = new System.Random();

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

            tiles = new GameTile[BoardDimensions.Column, BoardDimensions.Row];

            for ( int i = 0; i < BoardDimensions.Column; i++ )
            {
                for ( int j = 0; j < BoardDimensions.Row; j++ )
                {
                    tiles[i, j] = SpawnTile( new BoardPosition( i, j ) );
                }
            }
        }

        private void GameBoard_TileClicked( GameTile tile )
        {
            // If there are blocks moving and the board is locked, do nothing
            if ( isLocked )
                return;

            var sameColorNeighbours = FindNeighbours( tile, new List<GameTile>() );

            // Update the board by nullifying the tiles to be removed and shifting tiles down
            if ( sameColorNeighbours.Count >= neighbourThreshold )
            {
                foreach ( var item in sameColorNeighbours )
                {
                    tiles[item.BoardPosition.Column, item.BoardPosition.Row] = null;
                }

                for ( int i = 0; i < BoardDimensions.Column; i++ )
                {
                    for ( int j = BoardDimensions.Row - 1; j >= 0; j-- ) // Start from the bottom so we don't overwrite existing blocks
                    {
                        var tileToShift = tiles[i, j];
                        if ( tileToShift != null )
                        {
                            var shiftBy = BlocksDestroyedBelow( tileToShift.BoardPosition );
                            if ( shiftBy > 0 )
                            {
                                tiles[tileToShift.BoardPosition.Column, tileToShift.BoardPosition.Row] = null;
                                tiles[tileToShift.BoardPosition.Column, tileToShift.BoardPosition.Row + shiftBy] = tileToShift;
                                tileToShift.MoveTo( new BoardPosition( tileToShift.BoardPosition.Column, tileToShift.BoardPosition.Row + shiftBy ) );
                                blocksMoving++; // Increase the number of blocks that is changing position
                            }
                        }
                    }
                }

                // Destroy tile game objects
                foreach ( var item in sameColorNeighbours )
                    DestroyTile( item );

                // Iteratre over columns and spawn new tiles
                for ( int i = 0; i < BoardDimensions.Column; i++ )
                {
                    int nullCount = 0;
                    for ( int j = 0; j < BoardDimensions.Row; j++ )
                    {
                        if ( tiles[i, j] == null )
                            nullCount++;
                    }

                    for ( int j = 0; j < nullCount; j++ )
                    {
                        var newTile = SpawnTile( new BoardPosition( i, -( j + 1 ) ) );
                        var destinationRow = nullCount - 1 - j;
                        newTile.MoveTo( new BoardPosition( i, destinationRow ) );
                        tiles[i, destinationRow] = newTile;
                        blocksMoving++;
                    }
                }

                // Shall we lock the board?
                isLocked = blocksMoving > 0;
            }
        }

        private GameTile SpawnTile( BoardPosition targetPosition )
        {
            var tileGameObject = UnityEngine.Object.Instantiate( Resources.Load( "Game Tile" ) ) as GameObject;
            tileGameObject.transform.SetParent( transform );

            var tileComponent = tileGameObject.GetComponent<GameTile>();
            tileComponent.Initialize( this );
            tileComponent.Teleport( targetPosition );
            tileComponent.TileClicked += GameBoard_TileClicked;
            tileComponent.MovingFinished += GameBoard_MovingFinished;

            var colorIndex = random.Next( 0, tileColors.Count );
            var randomColor = tileColors[colorIndex];
            tileComponent.SetColor( randomColor.Color, randomColor.Sprite );

            return tileComponent;
        }

        private void DestroyTile( GameTile tile )
        {
            // Unregister the events
            tile.TileClicked -= GameBoard_TileClicked;
            tile.MovingFinished -= GameBoard_MovingFinished;

            GameObject.Destroy( tile.gameObject );
            //tile.gameObject.SetActive( false );
        }

        private void GameBoard_MovingFinished( GameTile tile )
        {
            blocksMoving--;

            if ( blocksMoving == 0 )
                isLocked = false;
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
