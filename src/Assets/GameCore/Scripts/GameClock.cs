using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;
using System;

namespace TechDrop.Gameplay
{
    public class GameClock : MonoBehaviour
    {
        [SerializeField] float gameDuration;
        [SerializeField] GameBoard gameBoard;

        public float TimeRemaining { get; private set; }
        public bool IsClockRunning { get; private set; }

        public event EventHandler ClockFinished;

        void Awake()
        {
            Assert.IsNotNull( gameBoard );
            Assert.IsTrue( gameDuration > 0f );

            // Initialize game clock
            TimeRemaining = gameDuration;
        }

        public void StartClock()
        {
            Assert.IsFalse( IsClockRunning );
            IsClockRunning = true;
        }

        public void ResetClock()
        {
            TimeRemaining = gameDuration;
        }

        public void StopClock()
        {
            Assert.IsTrue( IsClockRunning );
            IsClockRunning = false;
        }

        void Update()
        {
            // For accuracy time gets updated on every frame
            if ( IsClockRunning && TimeRemaining != 0f )
            {
                TimeRemaining -= Time.deltaTime;

                if ( TimeRemaining < 0f )  // For clarity, set the remaining time to 0 and disable game
                {
                    TimeRemaining = 0f;
                    IsClockRunning = false;
                    ClockFinished?.Invoke( this, new EventArgs() );
                }
            }
        }
    }
}
