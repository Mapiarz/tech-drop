using UnityEngine;
using System.Collections;
using System;

namespace TechDrop.Gameplay
{
    [Serializable]
    public struct BoardPosition
    {
        [SerializeField] int column;
        [SerializeField] int row;

        public int Column
        {
            get { return column; }
            set { column = value; }
        }
        public int Row
        {
            get { return row; }
            set { row = value; }
        }

        public BoardPosition( int column, int row )
        {
            this.column = column;
            this.row = row;
        }

        public static BoardPosition operator +( BoardPosition bp1, BoardPosition bp2 )
        {
            return new BoardPosition( bp1.Column + bp2.Column, bp1.Row + bp2.Row );
        }

        public static BoardPosition operator -( BoardPosition bp1, BoardPosition bp2 )
        {
            return new BoardPosition( bp1.Column - bp2.Column, bp1.Row - bp2.Row );
        }
    }
}
