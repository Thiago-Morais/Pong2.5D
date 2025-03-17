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
        float newSpeed = paddle.CurrentSpeed;
        if (IsPaddleAfterBall())
            newSpeed += basePaddleSpeed * accelerationFactor * dt;
        else if (IsPaddleBeforeBall())
            newSpeed -= basePaddleSpeed * accelerationFactor * dt;
        else
        {
            if (paddle.CurrentSpeed > 0)
                newSpeed -= basePaddleSpeed * decelerationFactor * dt;
            else if (paddle.CurrentSpeed < 0)
                newSpeed += basePaddleSpeed * decelerationFactor * dt;
        }
        paddle.SetCurrentSpeed(newSpeed);
    }
    public bool IsPaddleAfterBall() => paddle.GetAxisPosition() > ball.PositionParallelToPlayers;
    public bool IsPaddleBeforeBall() => paddle.GetAxisPosition() < ball.PositionParallelToPlayers;
}