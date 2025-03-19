using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallMono : MonoBehaviour
{
    [SerializeField] Ball model = new Ball(new PlayerAxis(Vector3.zero));
    [SerializeField] Transform meshes;
    [SerializeField] Transform ballPivot;
    [SerializeField] float radius;
    public PlayerMono cachedPlayerCollided;
    new Rigidbody rigidbody;

    public event Action<Collider> OnTriggerEnterEvent;
    public event Action<Collider> OnTriggerExitEvent;
    public Ball Model => model;
    public float Radius => radius;
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }
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
    public void FixedUpdate()
    {
        rigidbody.position = ballPivot.position + model.Position;
    }
    void OnTriggerEnter(Collider other)
    {
        OnTriggerEnterEvent?.Invoke(other);
    }
    void OnTriggerExit(Collider other)
    {
        OnTriggerExitEvent?.Invoke(other);
    }
    public void Reset()
    {
        model.Reset();
    }
}