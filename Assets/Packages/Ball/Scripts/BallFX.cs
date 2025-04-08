using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
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
    [SerializeField] AudioMixer ballHitMixer;
    [SerializeField] MeshRenderer ballRenderer;
    [Header("Data")]
    [SerializeField] MinMaxCurve speedRemapIntensity = new MinMaxCurve(22, 30);
    [SerializeField] MinMaxCurve intensityCurve = new MinMaxCurve(1, AnimationCurve.Linear(0, 0, 1, 1));
    [SerializeField] float impulseForce = .2f;
    [SerializeField] AnimationCurve impulseForceCurve = AnimationCurve.Linear(0, 0, 1, 1);
    [SerializeField] MinMaxCurve ballHitStandardVolumeRange = new MinMaxCurve(0, 1);
    [SerializeField] MinMaxCurve ballHitIntenseVolumeRange = new MinMaxCurve(0, .25f);
    [SerializeField] bool setInitialBallColorOnStart = true;
    [SerializeField] MinMaxGradient ballIntensityColorRange = new MinMaxGradient(Color.green, Color.red);
    [SerializeField] MinMaxGradient ballIntensityEmissiveColorRange = new MinMaxGradient(Color.green, Color.red);
    Material ballMaterial;
    const string STANDARD_VOLUME_KEY = "BallHitStandardVolume";
    const string INTENSE_VOLUME_KEY = "BallHitIntenseVolume";

    void Awake()
    {
        if (!ball) ball = GetComponent<BallMono>();
        ballMaterial = ballRenderer.material;
    }
    void Start()
    {
        if (setInitialBallColorOnStart)
        {
            ballIntensityColorRange.colorMin = ballMaterial.color;
            ballIntensityEmissiveColorRange.colorMin = ballMaterial.GetColor("_EmissiveColor");
        }
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
        float intensity = GetIntensity(ball);
        paddleHitAudio.Play();
        if (intensity <= 0) return;

        UpdateFXValues(intensity);
        Vector3 contactPoint = other.ClosestPointOnBounds(ball.transform.position);
        particlesManager.PlayHitParticlesAt(contactPoint, intensity);
        impulseSource.GenerateImpulseWithVelocity(impulseForce * impulseForceCurve.Evaluate(intensity) * Random.onUnitSphere);
    }
    public void OnBallHitGoal(BallMono ball, Collider other, Goal goal)
    {
        float intensity = GetIntensity(ball);
        scoreAudio.Play();
        if (intensity <= 0) return;

        ballMaterial.color = ballIntensityColorRange.Evaluate(intensity);
        impulseSource.GenerateImpulseWithVelocity(impulseForce * impulseForceCurve.Evaluate(intensity) * Random.onUnitSphere);
    }
    public void OnBallHitWall(BallMono ball, Collider other, Wall wall)
    {
        float intensity = GetIntensity(ball);
        wallHitAudio.Play();
        if (intensity <= 0) return;

        UpdateFXValues(intensity);
        Vector3 contactPoint = other.ClosestPointOnBounds(ball.transform.position);
        particlesManager.PlayHitParticlesAt(contactPoint, intensity);
        impulseSource.GenerateImpulseWithVelocity(impulseForce * impulseForceCurve.Evaluate(intensity) * Random.onUnitSphere);
    }
    public void Reset(BallMono ball)
    {
        particlesManager.KillAllParticles();
        UpdateFXValues(0);
    }
    float GetIntensity(BallMono ball)
    {
        float baseIntensity = Mathf.InverseLerp(speedRemapIntensity.constantMin, speedRemapIntensity.constantMax, ball.Model.Speed);
        intensityCurve.Evaluate(baseIntensity, baseIntensity);
        return baseIntensity;
    }
    void UpdateFXValues(float intensity)
    {
        SetSFXIntensity(intensity);
        ballMaterial.color = ballIntensityColorRange.Evaluate(intensity, intensity);
        ballMaterial.SetColor("_EmissiveColor", ballIntensityEmissiveColorRange.Evaluate(intensity, intensity));
    }
    void SetSFXIntensity(float intensity)
    {
        ballHitMixer.SetFloat(STANDARD_VOLUME_KEY, CalculateVolumeWithLog(Remap(intensity, 0, 1, ballHitStandardVolumeRange.Evaluate(1, 1), ballHitStandardVolumeRange.Evaluate(0, 0))));
        ballHitMixer.SetFloat(INTENSE_VOLUME_KEY, CalculateVolumeWithLog(Remap(intensity, 0, 1, ballHitIntenseVolumeRange.Evaluate(0, 0), ballHitIntenseVolumeRange.Evaluate(1, 1))));
    }
    static float Remap(float value, float from1, float to1, float from2, float to2) => from2 + (value - from1) * ((to2 - from2) / (to1 - from1));
    float CalculateVolumeWithLog(float intensity)
    {
        if (intensity == 0) return -80f;
        return Mathf.Log10(intensity) * 20f;
    }
}
