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

    public float CurrentVelocity => currentVelocity;
    public float AxisPosition => axisPosition;
    public float TargetVelocitySmooth => targetVelocitySmooth;
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
            SetCurrentVelocitySmooth(targetVelocitySmooth, dt);
        if (currentVelocity < 0)
            axisPosition = Math.Max(axisBounds.constantMin, axisPosition + currentVelocity * dt);
        // similar to before, this time we use math.min to ensure we don't
        // go any farther than the bottom of the screen minus the paddle's
        // height (or else it will go partially below, since position is
        // based on its top left corner)
        else
            axisPosition = Math.Min(axisBounds.constantMax, axisPosition + currentVelocity * dt);
    }
    public void Render()
    {
        // FIXME
        // love.graphics.rectangle('fill', x, y, width, height);
    }
    public void SetTargetVelocitySmooth(float value) => SetTargetVelocitySmooth(value, velocityMultiplier);
    public void SetTargetVelocitySmooth(float value, float velocityMultiplier) => targetVelocitySmooth = value * velocityMultiplier;
    public void SetCurrentVelocitySmooth(float value) => SetCurrentVelocitySmooth(value, Time.deltaTime);
    public void SetCurrentVelocitySmooth(float value, float dt) => currentVelocity = Mathf.SmoothDamp(currentVelocity, value, ref m_velocityVelocity, smoothTime, Mathf.Infinity, dt);
    public void SetCurrentVelocity(float value) => currentVelocity = value;
    public void SetAxisPosition(float value) => axisPosition = value;
}