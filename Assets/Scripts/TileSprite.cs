using UnityEngine;
using System.Collections;
using System;

namespace TechDrop.Gameplay
{
    [Serializable]
    public class TileSprite
    {
        [SerializeField] private TileColor color;
        [SerializeField] private Sprite sprite;

        public TileColor Color
        {
            get
            {
                return color;
            }

            set
            {
                color = value;
            }
        }

        public Sprite Sprite
        {
            get
            {
                return sprite;
            }

            set
            {
                sprite = value;
            }
        }
    }

    [Serializable]
    public enum TileColor
    {
        Red,
        Purple,
        Green,
        Blue,
        Grey
    }
}
