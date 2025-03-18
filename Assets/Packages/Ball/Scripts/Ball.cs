using System;
using UnityEngine;

[Serializable]
public class Ball
{
    [SerializeField] float baseSpeed = 200;
    [SerializeField] PlayerAxis speed = new PlayerAxis(Vector3.zero);
    [SerializeField] PlayerAxis position = new PlayerAxis(Vector3.zero);
    [SerializeField] Paddle currentCollidedPaddle;
    public event Action<PlayerAxis, PlayerAxis> OnPositionChange;

    public float BaseSpeed => baseSpeed;
    public PlayerAxis Position => position;
    public PlayerAxis Speed => speed;
    public Paddle CurrentCollidedPaddle => currentCollidedPaddle;
    public Ball(PlayerAxis position)
    {
        this.position = position;
    }
    public bool Collides(Paddle paddle) => currentCollidedPaddle == paddle;
    public void Reset()
    {
        SetPosition(new PlayerAxis(Vector3.zero));
        SetSpeed(new PlayerAxis(Vector3.zero));
    }
    public void Update(float dt)
    {
        position.SetTowardPlayers(position.TowardPlayers + speed.TowardPlayers * dt);
        position.SetParallelToPlayers(position.ParallelToPlayers + speed.ParallelToPlayers * dt);
    }
    public void Render()
    {
        // FIXME
        // love.graphics.rectangle('fill', x, y, width, height);
    }
    public void SetCurrentCollidedPaddle(Paddle value) => currentCollidedPaddle = value;
    public void SetSpeed(PlayerAxis value) => speed = value;
    public void SetPosition(PlayerAxis value)
    {
        OnPositionChange?.Invoke(position, value);
        position = value;
    }
}
