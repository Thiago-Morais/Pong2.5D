using System;
using UnityEngine;

public class BallMono : MonoBehaviour
{
    [SerializeField] Ball model = new Ball(new PlayerAxis(Vector3.zero));
    [SerializeField] Transform meshes;
    [SerializeField] Transform ballPivot;
    [SerializeField] float radius;

    public event Action<Collider> OnTriggerEnterEvent;
    public event Action<Collider> OnTriggerExitEvent;
    public Ball Model => model;
    public float Radius => radius;
    void Start()
    {
        CalculateRadius();
    }
    [ContextMenu(nameof(CalculateRadius))]
    void CalculateRadius()
    {
        Bounds bounds = new Bounds(meshes.position, Vector3.zero);
        foreach (var renderer in meshes.GetComponentsInChildren<MeshRenderer>())
            bounds.Encapsulate(renderer.bounds);
        radius = Mathf.Max(bounds.extents.x, bounds.extents.y, bounds.extents.z);
    }
    public void Update()
    {
        transform.position = ballPivot.position + model.Position;
    }
    void OnTriggerEnter(Collider other)
    {
        OnTriggerEnterEvent?.Invoke(other);
        if (other.TryGetComponent<PaddleMono>(out var paddle))
        {
            model.SetCurrentCollidedPaddle(paddle.Model);
        }
    }
    // This may cause problems if the ball hits two paddles at the same time. But it wont, so... ¯\_(ツ)_/¯
    void OnTriggerExit(Collider other)
    {
        OnTriggerExitEvent?.Invoke(other);
        if (other.TryGetComponent<PaddleMono>(out var paddle))
        {
            if (paddle.Model == model.CurrentCollidedPaddle)
                model.SetCurrentCollidedPaddle(null);
        }
    }
    public void Reset()
    {
        model.Reset();
    }
}