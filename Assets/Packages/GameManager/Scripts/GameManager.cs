using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] AudioClip paddle_hit;
    [SerializeField] AudioClip score;
    [SerializeField] AudioClip wall_hit;
    [SerializeField] PlayerMono player1;
    [SerializeField] PlayerMono player2;
    [SerializeField] BallMono ball;
    [SerializeField] PaddleAutoController aiController;
    // [SerializeField] UIManager uiManager;
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
    [SerializeField] string gameState = "start";
    static GameManager instance;
    public static GameManager Instance => instance;
    public int ServingPlayerId => servingPlayer;
    public int WinningPlayerId => winningPlayer;

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
        gameState = "start";

    }
    void Update()
    {
        Draw();
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
}