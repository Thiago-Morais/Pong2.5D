using System;
using UnityEngine;

[Serializable]
public class PaddleAutoController
{
    [SerializeField] bool isEnabled = true;
    [SerializeField] Ball ball;
    [SerializeField] Paddle paddle;
    [SerializeField] float baseVelocityMultiplier;
    [SerializeField] float velocityDump;
    public bool IsEnabled => isEnabled;
    public float VelocityDump => velocityDump;

    PaddleAutoController() { }
    public void Update(float dt)
    {
        if (!isEnabled) return;

        float direction = 0;
        if (IsPaddleAfterBall())
            direction = -1;
        else if (IsPaddleBeforeBall())
            direction = 1;
        paddle.SetTargetVelocitySmooth(direction, baseVelocityMultiplier * velocityDump);
        paddle.Update(dt);
    }
    public bool IsPaddleAfterBall() => paddle.AxisPosition > ball.Position.ParallelToPlayers;
    public bool IsPaddleBeforeBall() => paddle.AxisPosition < ball.Position.ParallelToPlayers;
    public void SetEnabled(bool enabled) => isEnabled = enabled;
    public void SetVelocityDump(float value) => velocityDump = value;

    public class Builder
    {
        readonly PaddleAutoController paddleAutoController = new();

        public Builder(Ball ball, Paddle paddle)
        {
            paddleAutoController.ball = ball;
            paddleAutoController.paddle = paddle;
            paddleAutoController.baseVelocityMultiplier = paddle.VelocityMultiplier;
            paddleAutoController.velocityDump = .45f;
        }
        public Builder WithVelocityMultiplier(float velocityMultiplier)
        {
            paddleAutoController.baseVelocityMultiplier = velocityMultiplier;
            return this;
        }
        public Builder WithVelocityDump(float velocityDump)
        {
            paddleAutoController.velocityDump = velocityDump;
            return this;
        }
        public PaddleAutoController Build() => paddleAutoController;
    }
}