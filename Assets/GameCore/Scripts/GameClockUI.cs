using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;

namespace TechDrop.Gameplay
{
    public class GameClockUI : MonoBehaviour
    {
        [SerializeField] GameClock clock;
        [SerializeField] TextMesh textMesh;
        [SerializeField] SpriteRenderer rendererComponent;
        [SerializeField] Sprite tickSprite;
        [SerializeField] Sprite tockSprite;

        int lastKnownTime = int.MinValue;

        void Awake()
        {
            Assert.IsNotNull( clock );
            Assert.IsNotNull( textMesh );
            Assert.IsNotNull( rendererComponent );
            Assert.IsNotNull( tickSprite );
            Assert.IsNotNull( tockSprite );

            lastKnownTime = Mathf.CeilToInt( clock.TimeRemaining );
            textMesh.text = lastKnownTime.ToString( "00" );
            rendererComponent.sprite = tickSprite;
        }

        void Update()
        {
            var secondsRemaining = Mathf.CeilToInt( clock.TimeRemaining );

            if ( secondsRemaining != lastKnownTime )
            {
                if ( rendererComponent.sprite == tickSprite )
                {
                    rendererComponent.sprite = tockSprite;
                }
                else
                {
                    rendererComponent.sprite = tickSprite;
                }

                textMesh.text = secondsRemaining.ToString( "00" );

                lastKnownTime = secondsRemaining;
            }
        }
    }
}
