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
        if (IsPaddleAfterBall())
            paddle.currentSpeed += basePaddleSpeed * accelerationFactor * dt;
        else if (IsPaddleBeforeBall())
            paddle.currentSpeed -= basePaddleSpeed * accelerationFactor * dt;
        else
        {
            if (paddle.currentSpeed > 0)
                paddle.currentSpeed -= basePaddleSpeed * decelerationFactor * dt;
            else if (paddle.currentSpeed < 0)
                paddle.currentSpeed += basePaddleSpeed * decelerationFactor * dt;
        }
    }
    public bool IsPaddleAfterBall() => paddle.GetPositionConstrained() > ball.PositionParallelToPlayers;
    public bool IsPaddleBeforeBall() => paddle.GetPositionConstrained() < ball.PositionParallelToPlayers;
}