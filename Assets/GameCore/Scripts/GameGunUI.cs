using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;

namespace TechDrop.Gameplay
{
    public class GameGunUI : MonoBehaviour
    {
        [SerializeField] GameGun gameGun;
        [SerializeField] TextMesh timer;

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
