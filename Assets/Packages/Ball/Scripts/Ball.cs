using System;
using UnityEngine;

[Serializable]
public class Ball
{
    [SerializeField] float baseSpeed = 200;
    [SerializeField] BallAxis speed;
    [SerializeField] BallAxis position;
    [SerializeField] Paddle currentCollidedPaddle;
    public event Action<BallAxis, BallAxis> OnPositionChange;

    public float BaseSpeed => baseSpeed;
    public BallAxis Position => position;
    public BallAxis Speed => speed;
    public Paddle CurrentCollidedPaddle => currentCollidedPaddle;
    public Ball(BallAxis position)
    {
        this.position = position;
    }
    public bool Collides(Paddle paddle) => currentCollidedPaddle == paddle;
    public void Reset()
    {
        SetPosition(new BallAxis());
        SetSpeed(new BallAxis());
    }
    public void Update(float dt)
    {
        float newPositionTowardPlayers = position.TowardPlayers + speed.TowardPlayers * dt;
        float newPositionParallelToPlayers = position.ParallelToPlayers + speed.ParallelToPlayers * dt;
        SetPosition(new BallAxis(towardPlayers: newPositionTowardPlayers, parallelToPlayers: newPositionParallelToPlayers));
    }
    public void Render()
    {
        // FIXME
        // love.graphics.rectangle('fill', x, y, width, height);
    }
    public void SetCurrentCollidedPaddle(Paddle value) => currentCollidedPaddle = value;
    void SetSpeed(BallAxis value) => speed = value;
    void SetPosition(BallAxis value)
    {
        OnPositionChange?.Invoke(position, value);
        position = value;
    }
}
public struct BallAxis
{
    Vector2 axis;
    public BallAxis(float towardPlayers, float parallelToPlayers) : this(new Vector2(parallelToPlayers, towardPlayers)) { }
    public BallAxis(Vector2 axis) => this.axis = axis;
    public readonly float TowardPlayers => GameManager.GetTowardPlayers(axis);
    public readonly float ParallelToPlayers => GameManager.GetParallelToPlayers(axis);
    public readonly Vector2 AsVector2 => axis;
    public void SetTowardPlayers(float value) => axis = GameManager.SetTowardPlayers(axis, value);
    public void SetParallelToPlayers(float value) => axis = GameManager.SetParallelToPlayers(axis, value);
}