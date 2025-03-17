using UnityEngine;

public class PlayerMono : MonoBehaviour
{
    [SerializeField] PaddleMono paddle;
    public PaddleMono Paddle => paddle;
    void Update()
    {
        float axisPosition = paddle.Model.AxisPosition;
        Vector3 position = paddle.transform.position;
        position.z = axisPosition;
        paddle.transform.position = position;
    }
}