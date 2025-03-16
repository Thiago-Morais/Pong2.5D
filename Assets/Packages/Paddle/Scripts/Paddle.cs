using System;
using static Constants;

public class Paddle
{
    public float x;
    public float y;
    public float width;
    public float height;
    public float currentSpeed = 0;

    public Paddle(float x, float y, float width, float height)
    {
        this.x = x;
        this.y = y;
        this.width = width;
        this.height = height;
    }
    public void Update(float dt)
    {
        if (currentSpeed < 0)
            y = Math.Max(0, y + currentSpeed * dt);
        // similar to before, this time we use math.min to ensure we don't
        // go any farther than the bottom of the screen minus the paddle's
        // height (or else it will go partially below, since position is
        // based on its top left corner)
        else
            y = Math.Min(VIRTUAL_HEIGHT - height, y + currentSpeed * dt);
    }
    public void Render()
    {
        // FIXME
        // love.graphics.rectangle('fill', x, y, width, height);
    }

    internal int GetPositionConstrained()
    {
        throw new NotImplementedException();
    }
}