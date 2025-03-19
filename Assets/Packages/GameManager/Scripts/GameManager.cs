using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;
using Random = UnityEngine.Random;

// !! This is a God Object. The original project was like this so I kept like this.
public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] AudioClip paddle_hit;
    [SerializeField] AudioClip score;
    [SerializeField] AudioClip wall_hit;
    [SerializeField] PlayerMono player1;
    [SerializeField] Goal player1TargetGoal;
    [SerializeField] PlayerMono player2;
    [SerializeField] Goal player2TargetGoal;
    [SerializeField] BallMono ball;
    [SerializeField] UIManager uiManager;
    [Header("Data")]
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
    [SerializeField] int winningPlayer;
    [SerializeField] int playerCount;
    [SerializeField] int maxScore = 10;
    [SerializeField] float ballSpeedIncrease = 1.03f;
    [SerializeField] float directionWeightPaddleForward = 1;
    [SerializeField] float directionWeightPaddleVelocity = 1;
    PaddleAutoController aiController;
    Controls player1Controls;
    Controls player2Controls;
    InputUser player1Input;
    InputUser player2Input;
    View view;
    static GameManager instance;

    public static GameManager Instance => instance;
    public int ServingPlayerId => servingPlayer;
    public int WinningPlayerId => winningPlayer;
    public int Player1Score => player1Score;
    public int Player2Score => player2Score;
    public GameStates GameState => gameState;

    public enum GameStates { start, menu, serve, play, done }
    enum View { ThirdPerson, TopDown, }
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
        // initialize score variables
        player1Score = 0;
        player2Score = 0;

        // either going to be 1 or 2; whomever is scored on gets to serve the
        // following turn
        servingPlayer = 1;

        // player who won the game; not set to a proper value until we reach
        // that state in the game
        winningPlayer = 0;

        //~ amount of real players
        playerCount = 0;

        aiController = new PaddleAutoController.Builder(ball.Model, player2.Paddle.Model).Build();

        SetUpInputSystem();
        SetGameState(GameStates.start);
    }
    void SetUpInputSystem()
    {
        player1Controls = new();
        player1Input = InputUser.PerformPairingWithDevice(Keyboard.current);
        player1Input.ActivateControlScheme(player1Controls.KeyboardMouse1Scheme);
        player1Input.AssociateActionsWithUser(player1Controls);
        player2Controls = new();
        player2Input = InputUser.PerformPairingWithDevice(Keyboard.current);
        player2Input.ActivateControlScheme(player2Controls.KeyboardMouse2Scheme);
        player2Input.AssociateActionsWithUser(player2Controls);
    }
    void Update()
    {
        UpdateGameState();
        Draw();
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
                float towardDirection = Random.Range(.5f, 1f);
                if (servingPlayer == 2) towardDirection = -towardDirection;

                Vector2 direction = new Vector2(parallelDirection, towardDirection).normalized;
                ball.Model.Direction.SetParallelToPlayers(direction.x);
                ball.Model.Direction.SetTowardPlayers(direction.y);
                float randomSpeed = Random.Range(.66f, 1.33f) * ball.Model.BaseSpeed;
                ball.Model.SetSpeed(randomSpeed);
                if (playerCount == 1)
                    aiController.SetVelocityDump(Random.Range(.3f, .55f));
                return;
            case GameStates.play:
                Debug.Log($"{nameof(aiController)}.{nameof(aiController.VelocityDump)} = " + aiController.VelocityDump, this);
                return;
            default:
                return;
        }
    }
    void Draw()
    {
        // uiManager.SetState(gameState);

        // render different things depending on which part of the game we're in


        //     // show the score before ball is rendered so it can move over the text
        //     displayScore()

        // player1: render()
        // player2: render()
        // ball: render()

        // // display FPS for debugging; simply comment out to remove
        // displayFPS()

    }
    public void SetGameState(GameStates value)
    {
        gameState = value;
        uiManager.SetState(gameState);

        player1Controls.Disable();
        player2Controls.Disable();
        switch (gameState)
        {
            case GameStates.start:
                player1Controls.MapAwaitContinue.Enable();
                player2Controls.MapAwaitContinue.Enable();
                break;
            case GameStates.menu:
                player1Controls.MapPlayerSelection.Enable();
                player2Controls.MapPlayerSelection.Enable();
                break;
            case GameStates.serve:
                player1Controls.MapAwaitContinue.Enable();
                player2Controls.MapAwaitContinue.Enable();
                player1.Reset();
                player2.Reset();
                break;
            case GameStates.play:
                player1Controls.MapPlayer.Enable();
                player2Controls.MapPlayer.Enable();
                break;
            case GameStates.done:
                player1Controls.MapAwaitContinue.Enable();
                player2Controls.MapAwaitContinue.Enable();
                break;
            default: break;
        }
    }
    void OnEnable()
    {
        player1Controls.MapAwaitContinue.Continue.performed += Continue;
        player1Controls.MapPlayerSelection.SelectSinglePlayer.performed += SelectSinglePlayer;
        player1Controls.MapPlayerSelection.SelectMultiPlayer.performed += SelectMultiPlayer;
        player1Controls.MapPlayer.Move.performed += MovePlayer1;
        player2Controls.MapPlayer.Move.performed += MovePlayer2;

        InputUser.onUnpairedDeviceUsed += OnUnpairedDeviceUsed;

        ball.OnTriggerEnterEvent += Ball_OnTriggerEnterEvent;
        ball.OnTriggerExitEvent += Ball_OnTriggerExitEvent;
    }
    void OnDisable()
    {
        player1Controls.MapAwaitContinue.Continue.performed -= Continue;
        player1Controls.MapPlayerSelection.SelectSinglePlayer.performed -= SelectSinglePlayer;
        player1Controls.MapPlayerSelection.SelectMultiPlayer.performed -= SelectMultiPlayer; ;
        player1Controls.MapPlayer.Move.performed -= MovePlayer1;
        player2Controls.MapPlayer.Move.performed -= MovePlayer2;

        InputUser.onUnpairedDeviceUsed -= OnUnpairedDeviceUsed;

        ball.OnTriggerEnterEvent -= Ball_OnTriggerEnterEvent;
        ball.OnTriggerExitEvent -= Ball_OnTriggerExitEvent;
    }
    void Continue(InputAction.CallbackContext context)
    {
        Debug.Log($"Continue: {context.phase}", this);
        if (context.performed)
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
    }
    void SelectSinglePlayer(InputAction.CallbackContext context)
    {
        if (context.performed)
            if (gameState == GameStates.menu)
            {
                playerCount = 1;
                SetGameState(GameStates.serve);
            }
    }
    void SelectMultiPlayer(InputAction.CallbackContext context)
    {
        if (context.performed)
            if (gameState == GameStates.menu)
            {
                playerCount = 2;
                SetGameState(GameStates.serve);
            }
    }
    void MovePlayer1(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (gameState == GameStates.play)
                player1.Paddle.Model.SetTargetVelocitySmooth(context.ReadValue<float>());
        }
        else if (context.canceled)
            player1.Paddle.Model.SetTargetVelocitySmooth(0);
    }
    void MovePlayer2(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (gameState == GameStates.play)
                if (playerCount != 2)
                    Debug.Log($"Game is not in multiplayer mode", this);
                else
                    player2.Paddle.Model.SetTargetVelocitySmooth(context.ReadValue<float>());
        }
        else if (context.canceled)
            player2.Paddle.Model.SetTargetVelocitySmooth(0);
    }
    void OnUnpairedDeviceUsed(InputControl control, InputEventPtr ptr)
    {
        if (player1Input == null)
            player1Input = InputUser.PerformPairingWithDevice(control.device);
        else
            player2Input = InputUser.PerformPairingWithDevice(control.device);
    }
    void Ball_OnTriggerEnterEvent(Collider other)
    {
        if (gameState == GameStates.play)
            if (other.attachedRigidbody)
            {
                if (other.attachedRigidbody.TryGetComponent<PlayerMono>(out var player))
                {
                    Debug.Log($"Hit Player: {player}", this);
                    ball.cachedPlayerCollided = player;
                    Vector3 direction = ball.transform.position - player.Paddle.transform.position;
                    if (player == player1)
                    {
                        ball.Model.Position.SetTowardPlayers(PlayerAxis.GetTowardPlayers(player.Paddle.PointInFrontOfPaddle) - ball.Radius);
                        direction += new PlayerAxis(-directionWeightPaddleForward, (player.Paddle.Model.CurrentVelocity / player.Paddle.Model.VelocityMultiplier) * directionWeightPaddleVelocity);
                    }
                    else if (player == player2)
                    {
                        ball.Model.Position.SetTowardPlayers(PlayerAxis.GetTowardPlayers(player.Paddle.PointInFrontOfPaddle) + ball.Radius);
                        direction += new PlayerAxis(directionWeightPaddleForward, (player.Paddle.Model.CurrentVelocity / player.Paddle.Model.VelocityMultiplier) * directionWeightPaddleVelocity);
                    }
                    // ball.Model.Direction.SetTowardPlayers(-ball.Model.Direction.TowardPlayers);
                    Debug.DrawRay(player.Paddle.transform.position, direction, Color.red, 2f);
                    ball.Model.SetDirection(new PlayerAxis(direction.normalized));
                    ball.Model.SetSpeed(ball.Model.Speed * ballSpeedIncrease);
                }
                else if (other.attachedRigidbody.TryGetComponent<Wall>(out var wall))
                {
                    Debug.Log($"{nameof(wall)} = " + wall, this);
                    float radiusOffset = wall.IsUpperWall ? ball.Radius : -ball.Radius;
                    ball.Model.Position.SetParallelToPlayers(PlayerAxis.GetParallelToPlayers(wall.InnerPoint) + radiusOffset);
                    ball.Model.Direction.SetParallelToPlayers(-ball.Model.Direction.ParallelToPlayers);
                }
                else if (other.attachedRigidbody.CompareTag(Constants.GOAL_TAG))
                {
                    if (other.attachedRigidbody.TryGetComponent<Goal>(out var goal))
                    {
                        if (goal == player1TargetGoal)
                        {
                            Debug.Log($"Goal player2Goal: {goal}", this);
                            servingPlayer = 1;
                            player1Score++;
                            if (player1Score == maxScore)
                                SetGameState(GameStates.done);
                            else
                                SetGameState(GameStates.serve);
                        }
                        else if (goal == player2TargetGoal)
                        {
                            Debug.Log($"Goal player1Goal: {goal}", this);
                            servingPlayer = 2;
                            player2Score++;
                            if (player2Score == maxScore)
                                SetGameState(GameStates.done);
                            else
                                SetGameState(GameStates.serve);
                        }
                        ball.Reset();
                        player1.Paddle.Model.SetCurrentVelocity(0);
                        player2.Paddle.Model.SetCurrentVelocity(0);
                    }
                }
            }
    }
    // This may cause problems if the ball hits two paddles at the same time. But it wont, so... ¯\_(ツ)_/¯
    void Ball_OnTriggerExitEvent(Collider other)
    {
        if (gameState == GameStates.play)
            if (other.attachedRigidbody)
            {
                if (other.attachedRigidbody.TryGetComponent<PlayerMono>(out var player))
                    if (player == ball.cachedPlayerCollided)
                        ball.cachedPlayerCollided = null;
            }
    }
}
