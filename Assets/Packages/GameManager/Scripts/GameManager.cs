using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [Header("Static Data")]
    [SerializeField] int maxScore = 10;
    [Header("Dynamic Data")]
    [Tooltip(
@"the state of our game; can be any of the following:
1. 'start' (the beginning of the game, before first serve)
2. 'menu' (select amount of players)
3. 'serve' (waiting on a key press to serve the ball)
4. 'play' (the ball is in play, bouncing between paddles)
5. 'done' (the game is over, with a victor, ready for restart)")]
    [SerializeField] GameStates gameState = GameStates.start;
    [SerializeField] int player1Score;
    [SerializeField] int player2Score;
    [SerializeField] int servingPlayer = 1;
    [SerializeField] int playerCount;
    [SerializeField] int winningPlayer;
    PaddleAutoController aiController;
    [Header("References")]
    PlayerMono player1;
    PlayerMono player2;
    BallMono ball;
    UIManager uiManager;
    CamerasManager camerasManager;
    static GameManager instance;
    public static GameManager Instance => instance;
    public int ServingPlayerId => servingPlayer;
    public int WinningPlayerId => winningPlayer;
    public int Player1Score => player1Score;
    public int Player2Score => player2Score;
    public GameStates GameState => gameState;
    public event Action<GameStates> OnGameStateChanged;

    public enum GameStates { start, menu, serve, play, done }
    public void Constructor(PlayerMono player1, PlayerMono player2, BallMono ball, UIManager uiManager, CamerasManager camerasManager)
    {
        this.player1 = player1;
        this.player2 = player2;
        this.ball = ball;
        this.uiManager = uiManager;
        this.camerasManager = camerasManager;
    }
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);


        player1Score = 0;
        player2Score = 0;
        servingPlayer = 1;
        winningPlayer = 0;
        playerCount = 0;

        aiController = new PaddleAutoController.Builder(ball.Model, player2.Paddle.Model).Build();

        SetGameState(GameStates.start);
    }
    public void SetGameState(GameStates value)
    {
        gameState = value;
        uiManager.SetState(gameState);
        OnGameStateChanged?.Invoke(gameState);

        switch (gameState)
        {
            case GameStates.start:
                break;
            case GameStates.menu:
                break;
            case GameStates.serve:
                player1.Reset();
                player2.Reset();
                ball.Reset();
                break;
            case GameStates.play:
                break;
            case GameStates.done:
                break;
            default: break;
        }
    }
    public void Player1Goal()
    {
        player1Score++;
        servingPlayer = 2;
        if (player1Score == maxScore)
        {
            winningPlayer = 1;
            SetGameState(GameStates.done);
        }
        else
            SetGameState(GameStates.serve);
    }
    public void Player2Goal()
    {
        player2Score++;
        servingPlayer = 1;
        if (player2Score == maxScore)
        {
            winningPlayer = 2;
            SetGameState(GameStates.done);
        }
        else
            SetGameState(GameStates.serve);
    }
    void Update()
    {
        UpdateGameState();
        if (GameState == GameStates.play)
        {
            ball.Model.Update(Time.deltaTime);
            player1.Paddle.Model.Update(Time.deltaTime);
            if (playerCount == 1)
                aiController.Update(Time.deltaTime);
            else if (playerCount == 2)
                player2.Paddle.Model.Update(Time.deltaTime);
        }
    }
    void UpdateGameState()
    {
        switch (GameState)
        {
            case GameStates.serve:
                float parallelDirection = Random.Range(-1f, 1f);
                float towardDirection = Random.Range(-1f, -.5f);
                if (servingPlayer == 2) towardDirection = -towardDirection;

                Vector2 direction = new Vector2(parallelDirection, towardDirection).normalized;
                ball.Model.Direction.SetParallelToPlayers(direction.x);
                ball.Model.Direction.SetTowardPlayers(direction.y);
                float randomSpeed = Random.Range(.75f, 1.5f) * ball.Model.BaseSpeed;
                ball.Model.SetSpeed(randomSpeed);
                if (playerCount == 1)
                    aiController.SetVelocityDump(Random.Range(.5f, .8f));
                return;
            case GameStates.play:
                return;
            default:
                return;
        }
    }
    public void Continue()
    {
        switch (GameState)
        {
            default: break;
            case GameStates.start:
                SetGameState(GameStates.menu);
                break;
            case GameStates.serve:
                SetGameState(GameStates.play);
                break;
            case GameStates.done:
                SetGameState(GameStates.menu);

                ball.Reset();

                player1Score = 0;
                player2Score = 0;

                if (winningPlayer == 1)
                    servingPlayer = 2;
                else
                    servingPlayer = 1;
                break;
        }
    }
    public void SelectSinglePlayer()
    {
        if (gameState == GameStates.menu)
        {
            playerCount = 1;
            SetGameState(GameStates.serve);
        }
        camerasManager.SetPlayerCount(playerCount);
    }
    public void SelectMultiPlayer()
    {
        if (gameState == GameStates.menu)
        {
            playerCount = 2;
            SetGameState(GameStates.serve);
        }
        camerasManager.SetPlayerCount(playerCount);
    }
    public void MovePlayer1(float velocity)
    {
        if (gameState == GameStates.play)
            player1.Paddle.Model.SetTargetVelocitySmooth(velocity);
    }
    public void MovePlayer2(float velocity)
    {
        if (gameState == GameStates.play)
            if (playerCount != 2)
                Debug.Log($"Game is not in multiplayer mode", this);
            else
                player2.Paddle.Model.SetTargetVelocitySmooth(velocity);
    }
    public void Quit()
    {
        Debug.Log($"Quit", this);
        Application.Quit();
    }
    public void BallOutOfBounds()
    {
        if (gameState == GameStates.play)
            SetGameState(GameStates.serve);
    }
}
