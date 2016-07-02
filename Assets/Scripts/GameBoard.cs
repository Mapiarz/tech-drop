using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;

public class GameBoard : MonoBehaviour
{
    public Vector2 Dimensions;
    public Vector3 Anchor;
    public float BlockSpeed = 1f;
    public AnimationCurve MoveCurve;

    void Awake()
    {
        Assert.IsNotNull( MoveCurve );
        Assert.IsTrue( BlockSpeed > 0f );
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
