using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] Transform innerPoint;
    [SerializeField] bool isUpperWall;
    public Vector3 InnerPoint => innerPoint.position;
    public bool IsUpperWall => isUpperWall;
}