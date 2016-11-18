﻿using UnityEngine;
using System.Collections;
using UnityEditor;

public class CreateGameGun : MonoBehaviour
{
    [MenuItem( "Assets/Create/Game Gun" )]
    public static void Create()
    {
        GameGunScriptableObject asset = ScriptableObject.CreateInstance<GameGunScriptableObject>();
        asset.Initialize( 5 );
        AssetDatabase.CreateAsset( asset, "Assets/Game Gun.asset" );
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }
}
