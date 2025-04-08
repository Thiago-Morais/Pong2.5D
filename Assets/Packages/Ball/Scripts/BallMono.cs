using System;
using Unity.Cinemachine;
using UnityEngine;
using static GameManager;

[RequireComponent(typeof(Rigidbody))]
public class BallMono : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform meshes;
    [SerializeField] Transform ballPivot;
    [Header("Data")]
    [SerializeField] Ball model = new Ball(new PlayerAxis(Vector3.zero));
    [SerializeField] float radius;
    [SerializeField] float ballSpeedIncreaseOnHit = 1.03f;
    [SerializeField] float directionWeightPaddleForward = 1;
    [SerializeField] float directionWeightPaddleVelocity = 3;
    PlayerMono cachedPlayerCollided;
    new Rigidbody rigidbody;
    GameManager game;

    PlayerMono player1;
    PlayerMono player2;
    public event Action<BallMono, Collider, PlayerMono> OnBallHitPlayer;
    public event Action<BallMono, Collider, Goal> OnBallHitGoal;
    public event Action<BallMono, Collider, Wall> OnBallHitWall;
    public event Action<BallMono> OnReset;

    public Ball Model => model;
    public float Radius => radius;
    public PlayerMono CachedPlayerCollided => cachedPlayerCollided;
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }
    public void Constructor(GameManager game, PlayerMono player1, PlayerMono player2)
    {
        this.game = game;
        this.player1 = player1;
        this.player2 = player2;
    }
    void Start()
    {
        CalculateRadius();
    }
    [ContextMenu(nameof(CalculateRadius))]
    void CalculateRadius()
    {
        Bounds bounds = new Bounds(meshes.position, Vector3.zero);
        foreach (var renderer in meshes.GetComponentsInChildren<MeshRenderer>())
            bounds.Encapsulate(renderer.bounds);
        radius = Mathf.Max(bounds.extents.x, bounds.extents.y, bounds.extents.z);
    }
    public void FixedUpdate()
    {
        rigidbody.position = ballPivot.position + model.Position;
    }
    void OnTriggerEnter(Collider other)
    {
        if (game.GameState == GameStates.play)
            if (other.attachedRigidbody)
            {
                if (other.attachedRigidbody.TryGetComponent<PlayerMono>(out var player))
                {
                    SetCachedPlayerCollided(player);
                    Vector3 direction = transform.position - player.Paddle.transform.position;
                    if (player == player1)
                    {
                        Model.Position.SetTowardPlayers(PlayerAxis.GetTowardPlayers(player.Paddle.PointInFrontOfPaddle) - Radius);

                        direction += new PlayerAxis(-directionWeightPaddleForward, (player.Paddle.Model.CurrentVelocity / player.Paddle.Model.VelocityMultiplier) * directionWeightPaddleVelocity);
                    }
                    else if (player == player2)
                    {
                        Model.Position.SetTowardPlayers(PlayerAxis.GetTowardPlayers(player.Paddle.PointInFrontOfPaddle) + Radius);
                        direction += new PlayerAxis(directionWeightPaddleForward, (player.Paddle.Model.CurrentVelocity / player.Paddle.Model.VelocityMultiplier) * directionWeightPaddleVelocity);
                    }
                    Debug.DrawRay(player.Paddle.transform.position, direction, Color.red, 2f);
                    Model.SetDirection(new PlayerAxis(direction.normalized));
                    Model.SetSpeed(Model.Speed * ballSpeedIncreaseOnHit);
                    OnBallHitPlayer?.Invoke(this, other, player);
                }
                else if (other.attachedRigidbody.TryGetComponent<Wall>(out var wall))
                {
                    float radiusOffset = wall.IsUpperWall ? Radius : -Radius;
                    Model.Position.SetParallelToPlayers(PlayerAxis.GetParallelToPlayers(wall.InnerPoint) + radiusOffset);
                    Model.Direction.SetParallelToPlayers(-Model.Direction.ParallelToPlayers);
                    OnBallHitWall?.Invoke(this, other, wall);
                }
                else if (other.attachedRigidbody.CompareTag(Constants.GOAL_TAG))
                {
                    if (other.attachedRigidbody.TryGetComponent<Goal>(out var goal))
                    {
                        if (goal == player1.TargetGoal)
                            game.Player1Goal();
                        else if (goal == player2.TargetGoal)
                            game.Player2Goal();
                        OnBallHitGoal?.Invoke(this, other, goal);
                    }
                }
            }
    }
    void OnTriggerExit(Collider other)
    {
        if (game.GameState == GameStates.play)
            if (other.attachedRigidbody)
            {
                if (other.attachedRigidbody.TryGetComponent<PlayerMono>(out var player))
                    if (player == CachedPlayerCollided)
                        SetCachedPlayerCollided(null);
            }
    }
    public void SetCachedPlayerCollided(PlayerMono value) => cachedPlayerCollided = value;
    public void Reset()
    {
        model.Reset();
        OnReset?.Invoke(this);
    }
}