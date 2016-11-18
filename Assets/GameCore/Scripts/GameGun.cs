﻿using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class GameGun : ScriptableObject
{
    public BoolMatrix effectArray;

    public string asd;
    public int test;

    public void Initialize( int s )
    {
        effectArray = new BoolMatrix( s );
    }
}
