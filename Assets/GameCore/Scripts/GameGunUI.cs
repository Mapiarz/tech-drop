using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using System;

namespace TechDrop.Gameplay
{
    public class GameGunUI : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] GameGun gameGun;
        [SerializeField] TextMesh timerMesh;

        void Awake()
        {
            Assert.IsNotNull( gameGun );
            Assert.IsNotNull( timerMesh );
        }

        void Update()
        {
            // Potentially can be moved to a coroutine
            if ( gameGun.IsCoolingDown )
            {
                timerMesh.text = gameGun.TimeToCoolDown.ToString( "00" );
            }
        }

        public void OnPointerDown( PointerEventData eventData )
        {
            gameGun.Fire();
        }
    }
}
