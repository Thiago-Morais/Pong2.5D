using UnityEngine;

public class PlayerMono : MonoBehaviour
{
    [SerializeField] PaddleMono paddle;
    public PaddleMono Paddle => paddle;
}