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

        public void OnPointerDown( PointerEventData eventData )
        {
            var fireSuccessful = gameGun.Fire();
            if ( fireSuccessful )
            {
                StopAllCoroutines();
                StartCoroutine( UpdateGunTimer() );
            }
        }

        IEnumerator UpdateGunTimer()
        {
            // a quarter second is enough to avoid going from 01 to 05 when user clicks repeatedly.
            var updateCycle = 0.25f;

            timerMesh.text = Mathf.CeilToInt( gameGun.TimeToCoolDown ).ToString( "00" );

            yield return new WaitForSeconds( updateCycle );

            while ( gameGun.IsCoolingDown )
            {
                timerMesh.text = Mathf.CeilToInt( gameGun.TimeToCoolDown ).ToString( "00" );
                yield return new WaitForSeconds( updateCycle );
            }

            timerMesh.text = "00";
        }
    }
}
