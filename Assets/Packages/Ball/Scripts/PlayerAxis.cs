using System;
using UnityEngine;

[Serializable]
public class PlayerAxis
{
    [SerializeField] Vector3 axis;
    public PlayerAxis() : this(0, 0) { }
    public PlayerAxis(float towardPlayers, float parallelToPlayers) : this(new Vector3(towardPlayers, 0, parallelToPlayers)) { }
    public PlayerAxis(Vector2 axis) : this(new Vector3(axis.x, 0, axis.y)) { }
    public PlayerAxis(Vector3 axis) => this.axis = axis;

    public float TowardPlayers => GetTowardPlayers(axis);
    public float ParallelToPlayers => GetParallelToPlayers(axis);
    public void SetTowardPlayers(Vector3 vector) => SetTowardPlayers(GetTowardPlayers(vector));
    public void SetParallelToPlayers(Vector3 vector) => SetParallelToPlayers(GetParallelToPlayers(vector));
    public void SetTowardPlayers(float value) => axis = SetTowardPlayers(axis, value);
    public void SetParallelToPlayers(float value) => axis = SetParallelToPlayers(axis, value);

    public static float GetTowardPlayers(Vector3 vector) => vector.x;
    public static float GetParallelToPlayers(Vector3 vector) => vector.z;
    public static Vector3 SetTowardPlayers(Vector3 vector, float value) => new Vector3(value, vector.y, vector.z);
    public static Vector3 SetParallelToPlayers(Vector3 vector, float value) => new Vector3(vector.x, vector.y, value);
    public static implicit operator Vector3(PlayerAxis axis) => axis.axis;
    public static implicit operator Vector2(PlayerAxis axis) => new Vector2(axis.TowardPlayers, axis.ParallelToPlayers);

    public override string ToString() => $"{nameof(TowardPlayers)}: {TowardPlayers}; {nameof(ParallelToPlayers)}: {ParallelToPlayers}";
    public PlayerAxis Normalized() => new PlayerAxis(axis.normalized);
}