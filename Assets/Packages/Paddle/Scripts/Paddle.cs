using System;
using UnityEngine;
using static UnityEngine.ParticleSystem;

[Serializable]
public class Paddle
{
    float axisPosition;
    public MinMaxCurve axisBounds = new MinMaxCurve(-7, 7);
    [SerializeField] float smoothTime = 0.05f;
    float currentVelocity = 0;
    float targetVelocitySmooth = 0;
    float m_velocityVelocity;
    [SerializeField] float velocityMultiplier = 10;

    public float TargetVelocitySmooth => targetVelocitySmooth;
    public float CurrentVelocity => currentVelocity;
    public float AxisPosition => axisPosition;
    public float VelocityMultiplier => velocityMultiplier;

    public Paddle(float axisPosition = 0) : this(axisPosition, new MinMaxCurve(-7, 7)) { }
    public Paddle(MinMaxCurve axisBounds) : this(0, axisBounds) { }
    public Paddle(float axisPosition, MinMaxCurve axisBounds)
    {
        this.axisPosition = axisPosition;
        this.axisBounds = axisBounds;
    }
    public void Update(float dt)
    {
        if (targetVelocitySmooth != currentVelocity)
            SetSmoothVelocity(targetVelocitySmooth, dt);
        if (currentVelocity < 0)
            axisPosition = Math.Max(axisBounds.constantMin, axisPosition + currentVelocity * dt);
        else
            axisPosition = Math.Min(axisBounds.constantMax, axisPosition + currentVelocity * dt);
    }
    public void SetTargetVelocitySmooth(float value) => SetTargetVelocitySmooth(value, velocityMultiplier);
    public void SetTargetVelocitySmooth(float value, float velocityMultiplier) => targetVelocitySmooth = value * velocityMultiplier;

    public void SetSmoothVelocity(float value) => SetSmoothVelocity(value, Time.deltaTime);
    public void SetSmoothVelocity(float value, float dt) => currentVelocity = Mathf.SmoothDamp(currentVelocity, value, ref m_velocityVelocity, smoothTime, Mathf.Infinity, dt);
    public void SetCurrentVelocity(float value) => currentVelocity = value;
    public void SetAxisPosition(float value) => axisPosition = value;
}