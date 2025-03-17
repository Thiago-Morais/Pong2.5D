using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] AudioClip paddle_hit;
    [SerializeField] AudioClip score;
    [SerializeField] AudioClip wall_hit;
    [SerializeField] PlayerMono player1;
    [SerializeField] PlayerMono player2;
    [SerializeField] BallMono ball;
    [SerializeField] PaddleAutoController aiController;
    [SerializeField] UIManager uiManager;
    [SerializeField] int player1Score;
    [SerializeField] int player2Score;
    [SerializeField] int servingPlayer = 1;
    [SerializeField] int winningPlayer;
    [SerializeField] int singlePlayer;
    [Tooltip(
@"the state of our game; can be any of the following:
1. 'start' (the beginning of the game, before first serve)
2. 'serve' (waiting on a key press to serve the ball)
3. 'play' (the ball is in play, bouncing between paddles)
4. 'done' (the game is over, with a victor, ready for restart)")]
    [SerializeField] GameStates gameState = GameStates.start;
    static GameManager instance;
    [SerializeField] float ballSpeedIncrease = 1.03f;
    [SerializeField] PlayerInput playerInput;
    ProjectInputs projectInputs;

    public static GameManager Instance => instance;
    public int ServingPlayerId => servingPlayer;
    public int WinningPlayerId => winningPlayer;
    public int Player1Score => player1Score;
    public int Player2Score => player2Score;
    public GameStates GameState => gameState;

    public enum GameStates { start, menu, serve, play, done };
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
        /* 
            // initialize our nice-looking retro text fonts
            smallFont = love.graphics.newFont('font.ttf', 8)
            largeFont = love.graphics.newFont('font.ttf', 16)
            scoreFont = love.graphics.newFont('font.ttf', 32)
            love.graphics.setFont(smallFont) 
        */
        /* 
            // set up our sound effects; later, we can just index this table and
            // call each entry's `play` method
            sounds = {
            ['paddle_hit'] = love.audio.newSource('sounds/paddle_hit.wav', 'static'),
            ['score'] = love.audio.newSource('sounds/score.wav', 'static'),
            ['wall_hit'] = love.audio.newSource('sounds/wall_hit.wav', 'static')
            }
        */
        /* 
                aiController = PaddleAutoController(ball, player2, PADDLE_SPEED / DIFFICULTY_DAMP)
                                //~ aiController = PaddleAutoController()
         */
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
        singlePlayer = 0;

        // the state of our game; can be any of the following:
        // 1. 'start' (the beginning of the game, before first serve)
        // 2. 'serve' (waiting on a key press to serve the ball)
        // 3. 'play' (the ball is in play, bouncing between paddles)
        // 4. 'done' (the game is over, with a victor, ready for restart)

        /* gameState = GameStates.start; */
        GetInputActions();


    }
    void GetInputActions()
    {
        projectInputs = new ProjectInputs();
        projectInputs.Enable();
    }
    void Update()
    {
        UpdateGameState();
        Draw();
        if (GameState == GameStates.play)
            ball.Model.Update(Time.deltaTime);
    }
    void UpdateGameState()
    {
        switch (GameState)
        {
            case GameStates.serve:
                float parallelDirection = Random.Range(-1f, 1f);
                float towardDirection = servingPlayer == 1 ? Random.Range(-1f, -.5f) : Random.Range(.5f, 1f);

                float randomSpeed = Random.Range(.66f, 1.33f) * ball.Model.BaseSpeed;
                Vector2 direction = new Vector2(parallelDirection, towardDirection).normalized;
                ball.Model.SetSpeedParallelToPlayers(direction.x * randomSpeed);
                ball.Model.SetSpeedTowardPlayers(direction.y * randomSpeed);
                return;
            case GameStates.play:
                if (ball.Model.Collides(player1.Paddle.Model))
                {
                    ball.Model.SetSpeedTowardPlayers(-ball.Model.SpeedTowardPlayers * ballSpeedIncrease);
                    SnapBallInFrontOfPaddle(player1.Paddle);
                }
                return;
            default:
                return;
        }
    }
    void SnapBallInFrontOfPaddle(PaddleMono paddle)
    {
        float pointInFrontOfPaddleTowardPlayers = GetTowardPlayers(paddle.PointInFrontOfPaddle);
        ball.Model.SetPositionTowardPlayers(pointInFrontOfPaddleTowardPlayers);
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
    }
    public static float GetTowardPlayers(Vector2 vector2) => vector2.y;
    public static float GetParallelToPlayers(Vector2 vector2) => vector2.x;
    public static Vector2 SetTowardPlayers(Vector2 vector2, float value) => new Vector2(vector2.x, value);
    public static Vector2 SetParallelToPlayers(Vector2 vector2, float value) => new Vector2(value, vector2.y);
    void OnEnable()
    {
        Debug.Log($"OnEnable", this);
        projectInputs.AwaitContinue.Continue.performed += Continue;
    }
    void OnDisable()
    {
        Debug.Log($"OnDisable", this);
        projectInputs.AwaitContinue.Continue.performed -= Continue;
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
}