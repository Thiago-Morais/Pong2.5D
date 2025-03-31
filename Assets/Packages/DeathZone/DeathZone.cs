using UnityEngine;

public class DeathZone : MonoBehaviour
{
    void OnTriggerExit(Collider other)
    {
        if (other.attachedRigidbody)
        {
            if (other.attachedRigidbody.TryGetComponent<BallMono>(out var ball))
                GameManager.Instance.BallOutOfBounds();
        }
    }
}
