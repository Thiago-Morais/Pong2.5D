using UnityEngine;
using static UnityEngine.ParticleSystem;

public class PaddleMono : MonoBehaviour
{
    [SerializeField] Paddle model = new Paddle(new MinMaxCurve(-7, 7));
    public Paddle Model => model;
    public Vector2 PointInFrontOfPaddle { get; set; }
}