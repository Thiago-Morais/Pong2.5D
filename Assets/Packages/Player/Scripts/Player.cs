using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMono : MonoBehaviour
{
    [SerializeField] PaddleMono paddle;
    [SerializeField] Goal targetGoal;
    new Rigidbody rigidbody;

    public PaddleMono Paddle => paddle;
    public Goal TargetGoal => targetGoal;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        float axisPosition = paddle.Model.AxisPosition;
        Vector3 position = rigidbody.position;
        position.z = axisPosition;
        rigidbody.position = position;
    }
    public void Reset() => paddle.Reset();
}