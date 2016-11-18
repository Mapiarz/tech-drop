using System;
using UnityEngine;

[Serializable]
public class BoolMatrix
{
    [SerializeField] RowData[] row;

    public RowData[] Row
    {
        get
        {
            return row;
        }
        set
        {
            row = value;
        }
    }

    public int Size { get { return Row.Length; } }

    public BoolMatrix( int size )
    {
        Row = new RowData[size];
        for ( int i = 0; i < size; i++ )
        {
            Row[i] = new RowData( size );
        }
    }

    [Serializable]
    public class RowData
    {
        [SerializeField] bool[] column;

        public RowData( int size )
        {
            Column = new bool[size];
        }

        public bool[] Column  // Column, so it can be accessed like Row[0].Column[1];
        {
            get
            {
                return column;
            }
            set
            {
                column = value;
            }
        }
    }
}