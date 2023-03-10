using UnityEngine;
using System.Collections;
using UnityEditor;
using TechDrop.Gameplay;

namespace TechDrop.EditorComponents
{
    [CustomEditor( typeof( GameBoard ) )]
    public class GameBoardEditor : Editor
    {
        void OnSceneGUI()
        {
            GameBoard gameBoard = target as GameBoard;

            float left = gameBoard.GameBoardArea.x;
            float bottom = gameBoard.GameBoardArea.y;
            float width = gameBoard.GameBoardArea.width;
            float height = gameBoard.GameBoardArea.height;

            Vector3[] vertices = new Vector3[4];
            vertices[0] = new Vector3( left, bottom - height, 1.0f ); //Bottom left
            vertices[1] = new Vector3( left, bottom, 1.0f ); //Upper left
            vertices[2] = new Vector3( left + width, bottom, 1.0f );  //Upper right
            vertices[3] = new Vector3( left + width, bottom - height, 1.0f ); //Bottom right

            //Draw the view limiting rectangle
            Handles.DrawSolidRectangleWithOutline( vertices, new Color( 1, 1, 1, 0.1f ), Color.red );

            Vector3 upperHandle = Vector3.Lerp( vertices[1], vertices[2], 0.5f );
            DrawRectHandle( upperHandle, Vector3.up, gameBoard );

            Vector3 bottomHandle = Vector3.Lerp( vertices[0], vertices[3], 0.5f );
            DrawRectHandle( bottomHandle, Vector3.down, gameBoard );

            Vector3 leftHandle = Vector3.Lerp( vertices[0], vertices[1], 0.5f );
            DrawRectHandle( leftHandle, Vector3.left, gameBoard );

            Vector3 rightHandle = Vector3.Lerp( vertices[2], vertices[3], 0.5f );
            DrawRectHandle( rightHandle, Vector3.right, gameBoard );

            //float columnPadding = ( width - ( gameBoard.BoardDimensions.Column * gameBoard.tileSize ) ) / ( gameBoard.BoardDimensions.Column + 1 );
            //float rowPadding = ( height - ( gameBoard.BoardDimensions.Row * gameBoard.tileSize ) ) / ( gameBoard.BoardDimensions.Row + 1 );

            //for ( int i = 0; i < gameBoard.BoardDimensions.Column; i++ )
            //{
            //    for ( int j = 0; j < gameBoard.BoardDimensions.Row; j++ )
            //    {
            //        Rect pos = new Rect( left + i * gameBoard.tileSize + ( ( i + 1 ) * columnPadding ), bottom - j * gameBoard.tileSize - ( ( j + 1 ) * rowPadding ), gameBoard.tileSize, -gameBoard.tileSize );
            //        Handles.DrawSolidRectangleWithOutline( pos, new Color( 1, 1, 1, 0.1f ), Color.white );
            //    }
            //}
        }

        private void DrawRectHandle( Vector3 position, Vector3 direction, GameBoard gameBoard )
        {
            Vector3 newPosition = Handles.Slider( position, direction, HandleUtility.GetHandleSize( Vector3.one ) * 0.03f, new Handles.CapFunction( Handles.DotHandleCap ), 0f );
            if ( GUI.changed )
            {
                Undo.RecordObject( gameBoard, "Resize ViewBox" );

                Vector3 sizeDelta = newPosition - position;
                Rect newAreaRect = new Rect( gameBoard.GameBoardArea );

                if ( direction == Vector3.down )
                    newAreaRect.yMax -= sizeDelta.y;
                else if ( direction == Vector3.up )
                {
                    newAreaRect.y += sizeDelta.y;
                    newAreaRect.yMax += sizeDelta.y;

                }
                else if ( direction == Vector3.left )
                    newAreaRect.xMin += sizeDelta.x;
                else if ( direction == Vector3.right )
                    newAreaRect.xMax += sizeDelta.x;

                gameBoard.GameBoardArea = newAreaRect;
            }

            GUI.changed = false;
        }
    }
}
