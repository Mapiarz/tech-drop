using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomPropertyDrawer( typeof( BoolMatrix ) )]
public class BoolMatrixPropertyDrawer : PropertyDrawer
{
    static readonly float CheckBoxSize = 18f;  // The checkbox is 18x18 pixels

    public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
    {
        // Draw property label before drawing the checkbox matrix
        EditorGUI.PrefixLabel( position, label );

        // Initialize new position for Size property of the matrix
        Rect newposition = position;
        newposition.y += CheckBoxSize;  // Increase Y position by one row
        newposition.height = CheckBoxSize;  // Set the height to checkbox height

        // Grab the data rows
        SerializedProperty rows = property.FindPropertyRelative( "row" );
        // Draw matrix Size property
        EditorGUI.PropertyField( newposition, rows.FindPropertyRelative( "Array.size" ) );

        newposition.y += CheckBoxSize;  // Move to the next row
        newposition.width = position.width / rows.arraySize;  // The matrix must be a square matrix so it's ok to use rows

        // Draw the checkbox matrix
        for ( int row = 0; row < rows.arraySize; row++ )
        {
            SerializedProperty columns = rows.GetArrayElementAtIndex( row ).FindPropertyRelative( "column" );

            var margin = newposition.width / 2f - CheckBoxSize / 2f;  // so the checkbox matrix is centered
            newposition.x += margin;

            // Assign number of columns - not sure what this was for. Maybe something to do with changing the matrix size?
            // columns.arraySize = rows.arraySize;

            for ( int column = 0; column < rows.arraySize; column++ )
            {
                var booleanProperty = columns.GetArrayElementAtIndex( column );
                EditorGUI.PropertyField( newposition, booleanProperty, GUIContent.none );  // Draw the checkbox
                newposition.x += newposition.width;  // Increment X position
            }

            newposition.x = position.x;  // Reset X position to the left side
            newposition.y += CheckBoxSize;  // Increase Y position to the next checkbox row
        }
    }

    public override float GetPropertyHeight( SerializedProperty property, GUIContent label )
    {
        SerializedProperty data = property.FindPropertyRelative( "row" );
        var extraRows = 2;  // +1 for Size property of the array and +1 so that the next property starts in new line
        return CheckBoxSize * ( data.arraySize + extraRows );
    }
}
