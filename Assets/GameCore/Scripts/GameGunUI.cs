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
        [SerializeField] TextMesh timer;

        public void OnPointerDown( PointerEventData eventData )
        {
            gameGun.Fire();
        }

        void Awake()
        {
            Assert.IsNotNull( gameGun );
            Assert.IsNotNull( timer );
        }

        // TODO: Move this to a coroutine
        void Update()
        {
            if ( gameGun.IsCoolingDown )
            {
                var time = gameGun.TimeToCoolDown.ToString( "00" );
                timer.text = time;
            }
            else
            {
                timer.text = "00";
            }
        }
    }
}
