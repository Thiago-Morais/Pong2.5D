using System;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class PaddleMono : MonoBehaviour
{
    [SerializeField] Paddle model = new Paddle(new MinMaxCurve(-7, 7));
    [SerializeField] Transform pointInFrontOfPaddle;

    public Paddle Model => model;
    public Vector3 PointInFrontOfPaddle => pointInFrontOfPaddle.position;

    public void Reset() => model.Reset();
}