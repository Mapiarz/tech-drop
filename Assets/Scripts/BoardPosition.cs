using UnityEngine;
using System.Collections;
using System;

namespace TechDrop.Gameplay
{
    [Serializable]
    public struct BoardPosition
    {
        [SerializeField] int x;
        [SerializeField] int y;

        public int X
        {
            get { return x; }
            set { x = value; }
        }
        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        public BoardPosition( int x, int y )
        {
            this.x = x;
            this.y = y;
        }

        public static BoardPosition operator +( BoardPosition bp1, BoardPosition bp2 )
        {
            return new BoardPosition( bp1.X + bp2.X, bp1.Y + bp2.Y );
        }

        public static BoardPosition operator -( BoardPosition bp1, BoardPosition bp2 )
        {
            return new BoardPosition( bp1.X - bp2.X, bp1.Y - bp2.Y );
        }
    }
}
