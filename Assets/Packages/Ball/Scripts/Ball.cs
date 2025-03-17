using System;
using UnityEngine;
using static Constants;

[Serializable]
public class Ball
{
    [SerializeField] float baseSpeed = 200;
    [SerializeField] Vector2 speed;
    [SerializeField] Paddle currentCollidedPaddle;
    public event Action<Vector2, Vector2> OnPositionChange;
    [SerializeField] Vector2 position;

    public float BaseSpeed => baseSpeed;
    public Vector2 Position => position;
    public Vector2 Speed => speed;
    public float SpeedTowardPlayers => GameManager.GetTowardPlayers(speed);
    public float SpeedParallelToPlayers => GameManager.GetParallelToPlayers(speed);
    public float PositionTowardPlayers => GameManager.GetTowardPlayers(position);
    public float PositionParallelToPlayers => GameManager.GetParallelToPlayers(position);
    void SetSpeed(Vector2 value) => speed = value;
    void SetPosition(Vector2 value)
    {
        OnPositionChange?.Invoke(position, value);
        position = value;
    }
    public Ball(Vector2 position)
    {
        this.position = position;
    }
    public bool Collides(Paddle paddle) => currentCollidedPaddle == paddle;
    public void Reset()
    {
        SetPosition(Vector2.zero);
        SetSpeed(Vector2.zero);
    }
    public void Update(float dt)
    {
        Vector2 aux = position;
        aux.x += speed.x * dt;
        aux.y += speed.y * dt;
        SetPosition(aux);
    }
    public void Render()
    {
        // FIXME
        // love.graphics.rectangle('fill', x, y, width, height);
    }
    public void SetSpeedTowardPlayers(float value) => SetSpeed(GameManager.SetTowardPlayers(speed, value));
    public void SetSpeedParallelToPlayers(float value) => SetSpeed(GameManager.SetParallelToPlayers(speed, value));
    public void SetPositionTowardPlayers(float value) => SetPosition(GameManager.SetTowardPlayers(position, value));
    public void SetPositionParallelToPlayers(float value) => SetPosition(GameManager.SetParallelToPlayers(position, value));
}
