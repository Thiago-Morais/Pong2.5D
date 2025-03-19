using System;
using UnityEngine;

[Serializable]
public class PaddleAutoController
{
    [SerializeField] bool isEnabled = true;
    [SerializeField] Ball ball;
    [SerializeField] Paddle paddle;
    [SerializeField] float velocityMultiplier;
    public bool IsEnabled => isEnabled;

    PaddleAutoController() { }
    public void Update(float dt)
    {
        if (!isEnabled) return;

        float direction = 0;
        if (IsPaddleAfterBall())
            direction = -1;
        else if (IsPaddleBeforeBall())
            direction = 1;
        paddle.SetTargetVelocitySmooth(direction, velocityMultiplier);
        paddle.Update(dt);
    }
    public bool IsPaddleAfterBall() => paddle.AxisPosition > ball.Position.ParallelToPlayers;
    public bool IsPaddleBeforeBall() => paddle.AxisPosition < ball.Position.ParallelToPlayers;
    public void SetEnabled(bool enabled) => isEnabled = enabled;

    public class Builder
    {
        readonly PaddleAutoController paddleAutoController = new();

        public Builder(Ball ball, Paddle paddle)
        {
            paddleAutoController.ball = ball;
            paddleAutoController.paddle = paddle;
            paddleAutoController.velocityMultiplier = paddle.VelocityMultiplier * .35f;
        }
        public Builder WithVelocityMultiplier(float velocityMultiplier)
        {
            paddleAutoController.velocityMultiplier = velocityMultiplier;
            return this;
        }
        public PaddleAutoController Build() => paddleAutoController;
    }
}