using System;
using UnityEngine;

[Serializable]
public class Ball
{
    [SerializeField] float baseSpeed = 20;
    [SerializeField] float speed;
    [SerializeField] PlayerAxis direction = new PlayerAxis(Vector3.zero);
    [SerializeField] PlayerAxis position = new PlayerAxis(Vector3.zero);
    public event Action<PlayerAxis, PlayerAxis> OnPositionChange;

    public float BaseSpeed => baseSpeed;
    public float Speed => speed;
    public PlayerAxis Position => position;
    public PlayerAxis Direction => direction;

    public Ball(PlayerAxis position)
    {
        this.position = position;
    }
    public void Reset()
    {
        SetPosition(new PlayerAxis(Vector3.zero));
        SetDirection(new PlayerAxis(Vector3.zero));
        SetSpeed(0);
    }
    public void Update(float dt)
    {
        direction = direction.Normalized();
        position.SetTowardPlayers(position.TowardPlayers + direction.TowardPlayers * speed * dt);
        position.SetParallelToPlayers(position.ParallelToPlayers + direction.ParallelToPlayers * speed * dt);
    }
    public void SetPosition(PlayerAxis value)
    {
        OnPositionChange?.Invoke(position, value);
        position = value;
    }
    public void SetDirection(PlayerAxis value) => direction = value.Normalized();
    public void SetSpeed(float value) => speed = value;
}
