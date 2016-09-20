﻿using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class GameGun : ScriptableObject
{
    //[SerializeField]
    //MultiDimensionalBool[] effectArray;
    public BoolMatrix effectArray;

    public int test;

    public void Initialize( int s )
    {
        effectArray = new BoolMatrix( s );
    }
}

[Serializable]
public class BoolMatrix
{
    [SerializeField]
    RowData[] row;

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
        [SerializeField]
        bool[] column;

        public RowData( int size )
        {
            Column = new bool[size];
        }

        public bool[] Column  // Column, so it can be accessed like Rows[0].Column[1];
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