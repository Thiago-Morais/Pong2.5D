using Unity.VisualScripting;
using UnityEngine;
using static Constants;

public class Ball
{
    public float x;
    public float y;
    public float width;
    public float height;
    public float dy = 0;
    public float dx = 0;

    public Ball(float x, float y, float width, float height)
    {
        this.x = x;
        this.y = y;
        this.width = width;
        this.height = height;
    }
    [ContextMenu(nameof(Collides))]
    public bool Collides(Paddle paddle)
    {
        //  first, check to see if the left edge of either is farther to the right
        // than the right edge of the other
        if (x > paddle.x + paddle.width || paddle.x > x + width)
            return false;
        //  then check to see if the bottom edge of either is higher than the top
        // edge of the other
        if (y > paddle.y + paddle.height || paddle.y > y + height)
            return false;

        //  if the above aren't true, they're overlapping
        return true;
    }
    [ContextMenu(nameof(Reset))]
    public void Reset()
    {
        x = VIRTUAL_WIDTH / 2 - width / 2;
        y = VIRTUAL_HEIGHT / 2 - height / 2;
        dy = 0;
        dx = 0;
    }
    public void Update(float dt)
    {
        x = x + dx * dt;
        y = y + dy * dt;
    }
    public void Render()
    {
        // FIXME
        // love.graphics.rectangle('fill', x, y, width, height);
    }
}
