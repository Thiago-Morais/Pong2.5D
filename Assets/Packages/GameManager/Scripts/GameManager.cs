using System;
using UnityEngine;
using Random = UnityEngine.Random;

// !! This is a God Object. The original project was like this so I kept like this.
public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] PlayerMono player1;
    [SerializeField] PlayerMono player2;
    [SerializeField] BallMono ball;
    [SerializeField] UIManager uiManager;
    [SerializeField] CamerasManager camerasManager;
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
    static GameManager instance;

    public static GameManager Instance => instance;
    public int ServingPlayerId => servingPlayer;
    public int WinningPlayerId => winningPlayer;
    public int Player1Score => player1Score;
    public int Player2Score => player2Score;
    public GameStates GameState => gameState;
    public event Action<GameStates> OnGameStateChanged;

    public enum GameStates { start, menu, serve, play, done }
    [ContextMenu(nameof(IncreaseScorePlayer1))]
    void IncreaseScorePlayer1()
    {
        player1Score++;
        uiManager.UpdateScore();
    }
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        Screen.fullScreenMode = FullScreenMode.Windowed;

        player1Score = 0;
        player2Score = 0;
        servingPlayer = 1;
        winningPlayer = 0;
        playerCount = 0;

        aiController = new PaddleAutoController.Builder(ball.Model, player2.Paddle.Model).Build();
        ball.Constructor(this, player1, player2);

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
        player1.Paddle.Model.SetCurrentVelocity(0);
        player2.Paddle.Model.SetCurrentVelocity(0);
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
        player1.Paddle.Model.SetCurrentVelocity(0);
        player2.Paddle.Model.SetCurrentVelocity(0);
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
            case GameStates.start:
                SetGameState(GameStates.menu);
                break;
            case GameStates.serve:
                SetGameState(GameStates.play);
                break;
            case GameStates.done:
                SetGameState(GameStates.serve);

                ball.Reset();

                player1Score = 0;
                player2Score = 0;

                if (winningPlayer == 1)
                    servingPlayer = 2;
                else
                    servingPlayer = 1;
                break;
            default:
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
    public void ToggleFullscreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
        Debug.Log($"Fullscreen: {Screen.fullScreen}", this);
    }
    public void Quit()
    {
        Debug.Log($"Quit", this);
        Application.Quit();
    }
}
