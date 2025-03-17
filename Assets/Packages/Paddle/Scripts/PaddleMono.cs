using UnityEngine;

public class PaddleMono : MonoBehaviour
{
    Paddle model = new Paddle(0, 0, 0, 0);
    public Paddle Model => model;
    public Vector2 PointInFrontOfPaddle { get; internal set; }
}