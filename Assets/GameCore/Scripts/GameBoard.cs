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
        bool isLocked = false;
        int blocksMoving = 0;
        System.Random random = new System.Random();

        public BoardPosition BoardDimensions
        {
            get
            {
                return boardDimensions;
            }
        }

        public float BlockSpeed
        {
            get
            {
                return blockSpeed;
            }
        }

        // Vertical space occupied by a single tile
        public float VerticalBlockSize
        {
            get
            {
                return GameBoardArea.height / (float)BoardDimensions.Row;
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

            // Initialize the tiles
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

            var sameColorNeighbours = FindNeighbours( FindPosition( tile ), new List<BoardPosition>() );

            if ( sameColorNeighbours.Count >= neighbourThreshold )
            {
                DestroyTiles( sameColorNeighbours );
            }
        }

        public bool FireGun( BoolMatrix effectMatrix )
        {
            if ( isLocked )  // If board is locked, cannot use gun
            {
                return false;
            }

            Assert.IsTrue( effectMatrix.Size <= BoardDimensions.Row );
            Assert.IsTrue( effectMatrix.Size <= BoardDimensions.Column );
            // TODO: Add logic when effect matrix size is smaller than the board

            var tilesToDestroy = new List<BoardPosition>();

            for ( int row = 0; row < effectMatrix.Size; row++ )
            {
                for ( int column = 0; column < effectMatrix.Size; column++ )
                {
                    if ( effectMatrix.Row[row].Column[column] )
                    {
                        tilesToDestroy.Add( new BoardPosition( column, row ) );
                    }
                }
            }

            DestroyTiles( tilesToDestroy );

            return true;
        }

        void DestroyTiles( IList<BoardPosition> tilesToDestroy )
        {
            // Destroy and nullify the tiles
            foreach ( var item in tilesToDestroy )
            {
                DespawnTile( tiles[item.Column, item.Row] );
                tiles[item.Column, item.Row] = null;
            }

            // Update the game board by shifting tiles down the screen
            for ( int column = 0; column < BoardDimensions.Column; column++ )
            {
                for ( int row = BoardDimensions.Row - 1; row >= 0; row-- ) // Start from the bottom so we don't overwrite existing blocks
                {
                    var tileToShift = tiles[column, row];
                    if ( tileToShift != null )
                    {
                        var shiftBy = BlocksDestroyedBelow( column, row );
                        if ( shiftBy > 0 )
                        {
                            tiles[column, row] = null;
                            tiles[column, row + shiftBy] = tileToShift;
                            var localPosition = BoardPositionToLocalPosition( new BoardPosition( column, row + shiftBy ) );
                            tileToShift.MoveTo( localPosition, shiftBy );
                            blocksMoving++; // Increase the number of blocks that is changing position
                        }
                    }
                }
            }

            // Iteratre over columns and spawn new tiles in the empty spots
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
                    var localPosition = BoardPositionToLocalPosition( new BoardPosition( i, destinationRow ) );
                    newTile.MoveTo( localPosition, destinationRow + j + 1 );
                    tiles[i, destinationRow] = newTile;
                    blocksMoving++;
                }
            }

            // Shall we lock the board?
            isLocked = blocksMoving > 0;
        }

        private GameTile SpawnTile( BoardPosition targetPosition )
        {
            var tileGameObject = UnityEngine.Object.Instantiate( Resources.Load( "Game Tile" ) ) as GameObject;
            tileGameObject.transform.SetParent( transform );

            var tileComponent = tileGameObject.GetComponent<GameTile>();
            tileComponent.Initialize( this );
            tileComponent.Teleport( BoardPositionToLocalPosition( targetPosition ) );
            tileComponent.TileClicked += GameBoard_TileClicked;
            tileComponent.MovingFinished += GameBoard_MovingFinished;

            var colorIndex = random.Next( 0, tileColors.Count );
            var randomColor = tileColors[colorIndex];
            tileComponent.SetColor( randomColor.Color, randomColor.Sprite );

            return tileComponent;
        }

        private void DespawnTile( GameTile tile )
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

        private int BlocksDestroyedBelow( int column, int row )
        {
            int result = 0;
            for ( int i = row; i < BoardDimensions.Row; i++ )
            {
                if ( tiles[column, i] == null )
                    result++;
            }

            return result;
        }

        private Vector3 BoardPositionToLocalPosition( BoardPosition pos )
        {
            var blockWidth = GameBoardArea.width / (float)BoardDimensions.Column;
            var blockHeight = GameBoardArea.height / (float)BoardDimensions.Row;
            var boardTopLeftAnchor = new Vector3( GameBoardArea.position.x, GameBoardArea.position.y ) - transform.localPosition;
            var tileSizeOffset = new Vector3( blockWidth / 2, -1 * blockHeight / 2 );
            var blockPosition = new Vector3( pos.Column * blockWidth, pos.Row * blockHeight * -1 );

            var localPosition = blockPosition + boardTopLeftAnchor + tileSizeOffset;

            return localPosition;
        }

        private List<BoardPosition> FindNeighbours( BoardPosition position, List<BoardPosition> alreadyVisited )
        {
            if ( alreadyVisited.Contains( position ) )
                return alreadyVisited;

            alreadyVisited.Add( position );
            var neighbours = GetImmidiateNeighbours( position );

            foreach ( var neighbour in neighbours )
            {
                if ( tiles[neighbour.Column, neighbour.Row].Color == tiles[position.Column, position.Row].Color )
                {
                    FindNeighbours( neighbour, alreadyVisited );
                }
            }

            return alreadyVisited;
        }

        private List<BoardPosition> GetImmidiateNeighbours( BoardPosition position )
        {
            var neighbours = new List<BoardPosition>();

            int column = position.Column;
            int row = position.Row;
            int maxColumnIndex = BoardDimensions.Column - 1;
            int maxRowIndex = BoardDimensions.Row - 1;

            if ( row < maxRowIndex )
                if ( tiles[column, row + 1] != null )
                    neighbours.Add( new BoardPosition( column, row + 1 ) );
            if ( row > 0 )
                if ( tiles[column, row - 1] != null )
                    neighbours.Add( new BoardPosition( column, row - 1 ) );
            if ( column < maxColumnIndex )
                if ( tiles[column + 1, row] != null )
                    neighbours.Add( new BoardPosition( column + 1, row ) );
            if ( column > 0 )
                if ( tiles[column - 1, row] != null )
                    neighbours.Add( new BoardPosition( column - 1, row ) );

            return neighbours;
        }

        BoardPosition FindPosition( GameTile tile )
        {
            // TODO: Could be implemented way more efficiently, such as:
            // could track positions for tiles in a dict (or something similar) OR
            // tiles could know their position
            // ATM an optimization is not needed.

            for ( int column = 0; column < BoardDimensions.Column; column++ )
            {
                for ( int row = BoardDimensions.Row - 1; row >= 0; row-- ) // Start from the bottom so we don't overwrite existing blocks
                {
                    if ( tiles[column, row] == tile )
                    {
                        return new BoardPosition( column, row );
                    }
                }
            }

            throw new InvalidOperationException( "The requested tile is not on the board!" );
        }
    }
}
