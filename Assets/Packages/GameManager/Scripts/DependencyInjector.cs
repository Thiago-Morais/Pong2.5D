using UnityEngine;

public class DependencyInjector : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] PlayerMono player1;
    [SerializeField] PlayerMono player2;
    [SerializeField] BallMono ball;
    [SerializeField] UIManager uiManager;
    [SerializeField] CamerasManager camerasManager;
    [SerializeField] InputManager inputManager;
    void Awake()
    {
        ball.Constructor(gameManager, player1, player2);
        inputManager.Constructor(gameManager);
        gameManager.Constructor(player1, player2, ball, uiManager, camerasManager);
    }
}
