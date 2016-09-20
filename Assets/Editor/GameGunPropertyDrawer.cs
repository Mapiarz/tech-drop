using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomPropertyDrawer( typeof( BoolMatrix ) )]
public class GameGunPropertyDrawer : PropertyDrawer
{
    public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
    {
        EditorGUI.PrefixLabel( position, label );
        Rect newposition = position;
        newposition.y += 18f;
        SerializedProperty data = property.FindPropertyRelative( "row" );

        newposition.height = 18f;
        EditorGUI.PropertyField( newposition, data.FindPropertyRelative( "Array.size" ) );
        newposition.y += 18f;

        for ( int j = 0; j < data.arraySize; j++ )
        {
            SerializedProperty row = data.GetArrayElementAtIndex( j ).FindPropertyRelative( "column" );

            newposition.height = 18f;
            newposition.width = position.width / data.arraySize;
            newposition.x += newposition.width / 2f - 18f / 2f;

            // Assign number of columns
            row.arraySize = data.arraySize;

            for ( int i = 0; i < data.arraySize; i++ )
            {

                var booleanProperty = row.GetArrayElementAtIndex( i );
                EditorGUI.PropertyField( newposition, booleanProperty, GUIContent.none );
                newposition.x += newposition.width;
            }

            newposition.x = position.x;
            newposition.y += 18f;
        }
    }

    public override float GetPropertyHeight( SerializedProperty property, GUIContent label )
    {
        SerializedProperty data = property.FindPropertyRelative( "row" );
        return 18f * ( data.arraySize + 2 );
    }
}
