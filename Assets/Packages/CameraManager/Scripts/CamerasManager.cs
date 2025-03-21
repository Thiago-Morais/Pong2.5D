using Unity.Cinemachine;
using UnityEngine;

public class CamerasManager : MonoBehaviour
{
    [SerializeField] CinemachineCamera singlePlayerCamera;
    [SerializeField] CinemachineCamera multiPlayerCamera;
    void Start()
    {
        SetPlayerCount(1);
    }
    public void SetPlayerCount(int count)
    {
        if (count == 1)
        {
            singlePlayerCamera.Priority = 10;
            multiPlayerCamera.Priority = 0;
        }
        else
        {
            singlePlayerCamera.Priority = 0;
            multiPlayerCamera.Priority = 10;
        }
    }
}
