using Unity.Cinemachine;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class BallFX : MonoBehaviour
{
    [Header("References")]
    [SerializeField] BallMono ball;
    [SerializeField] CinemachineImpulseSource impulseSource;
    [SerializeField] BallParticles particlesManager;
    [SerializeField] AudioSource paddleHitAudio;
    [SerializeField] AudioSource scoreAudio;
    [SerializeField] AudioSource wallHitAudio;
    [Header("Data")]
    [SerializeField] MinMaxCurve speedRemapIntensity = new MinMaxCurve(22, 30);
    [SerializeField] float impulseForce = .2f;
    [SerializeField] AnimationCurve impulseForceCurve = AnimationCurve.Linear(0, 0, 1, 1);
    void Awake()
    {
        if (!ball) ball = GetComponent<BallMono>();
    }
    void OnEnable()
    {
        ball.OnBallHitPlayer += OnBallHitPlayer;
        ball.OnBallHitGoal += OnBallHitGoal;
        ball.OnBallHitWall += OnBallHitWall;
        ball.OnReset += Reset;
    }
    public void OnBallHitPlayer(BallMono ball, Collider other, PlayerMono player)
    {
        paddleHitAudio.Play();
        float intensity = GetIntensity(ball);
        if (intensity < 0) return;

        Vector3 contactPoint = other.ClosestPointOnBounds(ball.transform.position);
        particlesManager.PlayHitParticlesAt(contactPoint, intensity);
        impulseSource.GenerateImpulseWithVelocity(impulseForce * impulseForceCurve.Evaluate(intensity) * Random.onUnitSphere);
    }
    public void OnBallHitGoal(BallMono ball, Collider other, Goal goal)
    {
        scoreAudio.Play();
        float intensity = GetIntensity(ball);
        if (intensity < 0) return;

        impulseSource.GenerateImpulseWithVelocity(impulseForce * impulseForceCurve.Evaluate(intensity) * Random.onUnitSphere);
    }
    public void OnBallHitWall(BallMono ball, Collider other, Wall wall)
    {
        wallHitAudio.Play();

        float intensity = GetIntensity(ball);
        if (intensity < 0) return;

        Vector3 contactPoint = other.ClosestPointOnBounds(ball.transform.position);
        particlesManager.PlayHitParticlesAt(contactPoint, intensity);
        impulseSource.GenerateImpulseWithVelocity(impulseForce * impulseForceCurve.Evaluate(intensity) * Random.onUnitSphere);
    }
    public void Reset(BallMono ball)
    {
        particlesManager.KillAllParticles();
    }
    float GetIntensity(BallMono ball) => Mathf.InverseLerp(speedRemapIntensity.constantMin, speedRemapIntensity.constantMax, ball.Model.Speed);
}
