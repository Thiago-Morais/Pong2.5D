using UnityEngine;

public class BallMono : MonoBehaviour
{
    [SerializeField] Ball model = new Ball(Vector2.zero);
    [SerializeField] Transform ballPivot;
    public Ball Model => model;
    public void Update()
    {
        float x = ballPivot.position.x + model.Position.x;
        float y = ballPivot.position.y + 0;
        float z = ballPivot.position.z + model.Position.y;
        transform.position = new Vector3(x, y, z);
    }
}