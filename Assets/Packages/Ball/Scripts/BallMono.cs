using System;
using UnityEngine;

public class BallMono : MonoBehaviour
{
    [SerializeField] Ball model = new Ball(new BallAxis());
    [SerializeField] Transform ballPivot;
    public event Action<Collider> OnTriggerEnterEvent;
    public event Action<Collider> OnTriggerExitEvent;
    public Ball Model => model;
    public void Update()
    {
        float x = ballPivot.position.x + model.Position.AsVector2.x;
        float y = ballPivot.position.y + 0;
        float z = ballPivot.position.z + model.Position.AsVector2.y;
        transform.position = new Vector3(x, y, z);
    }
    void OnTriggerEnter(Collider other)
    {
        OnTriggerEnterEvent?.Invoke(other);
        if (other.TryGetComponent<PaddleMono>(out var paddle))
            model.SetCurrentCollidedPaddle(paddle.Model);
    }
    // This may cause problems if the ball hits two paddles at the same time. But it wont, so... ¯\_(ツ)_/¯
    void OnTriggerExit(Collider other)
    {
        OnTriggerExitEvent?.Invoke(other);
        if (other.TryGetComponent<PaddleMono>(out var paddle))
            if (paddle.Model == model.CurrentCollidedPaddle)
                model.SetCurrentCollidedPaddle(null);
    }
    public void Reset()
    {
        model.Reset();
    }
}