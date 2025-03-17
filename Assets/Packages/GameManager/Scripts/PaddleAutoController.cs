using UnityEngine;

public class PaddleAutoController
{
    [SerializeField] Ball ball;
    [SerializeField] Paddle paddle;
    [SerializeField] float basePaddleSpeed;
    [SerializeField] float accelerationFactor = 0.9f;
    [SerializeField] float decelerationFactor = 0.1f;
    public PaddleAutoController(Ball ball, Paddle paddle, float basePaddleSpeed, float accelerationFactor = 0.9f, float decelerationFactor = 0.1f)
    {
        this.ball = ball;
        this.paddle = paddle;
        this.basePaddleSpeed = basePaddleSpeed;
        this.accelerationFactor = accelerationFactor;
        this.decelerationFactor = decelerationFactor;
    }
    public void Update(float dt)
    {
        float newSpeed = paddle.CurrentVelocity;
        if (IsPaddleAfterBall())
            newSpeed += basePaddleSpeed * accelerationFactor * dt;
        else if (IsPaddleBeforeBall())
            newSpeed -= basePaddleSpeed * accelerationFactor * dt;
        else
        {
            if (paddle.CurrentVelocity > 0)
                newSpeed -= basePaddleSpeed * decelerationFactor * dt;
            else if (paddle.CurrentVelocity < 0)
                newSpeed += basePaddleSpeed * decelerationFactor * dt;
        }
        paddle.SetCurrentVelocitySmooth(newSpeed);
    }
    public bool IsPaddleAfterBall() => paddle.AxisPosition > ball.Position.ParallelToPlayers;
    public bool IsPaddleBeforeBall() => paddle.AxisPosition < ball.Position.ParallelToPlayers;
}