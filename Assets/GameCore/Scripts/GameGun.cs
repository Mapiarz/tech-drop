using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;

namespace TechDrop.Gameplay
{
    public class GameGun : MonoBehaviour
    {
        [SerializeField] float coolDownDuration;
        [SerializeField] GameBoard gameBoard;
        [SerializeField] BoolMatrix gunEffect;

        float timeToCoolDown;

        public bool IsCoolingDown { get { return timeToCoolDown > 0f; } }
        public float TimeToCoolDown { get { return timeToCoolDown; } }

        void Awake()
        {
            Assert.IsNotNull( gameBoard );
        }

        public void Fire()
        {
            // This method could possibly return bool

            if ( IsCoolingDown )
            {
                return;
            }

            var fireSuccessful = gameBoard.FireGun( gunEffect );
            if ( fireSuccessful )
            {
                timeToCoolDown = coolDownDuration;
            }
        }

        void Update()
        {
            // Cool the gun down
            if ( IsCoolingDown )
            {
                timeToCoolDown -= Time.deltaTime;

                if ( timeToCoolDown < 0f )  // For clarity, set the time to cool down to 0
                {
                    timeToCoolDown = 0f;
                }
            }
        }
    }
}