using UnityEngine;

public class GameManager : MonoBehaviour
{

    Font smallFont;
    Font largeFont;
    Font scoreFont;
    public void Awake()
    {
        /* 
            // initialize our nice-looking retro text fonts
            smallFont = love.graphics.newFont('font.ttf', 8)
            largeFont = love.graphics.newFont('font.ttf', 16)
            scoreFont = love.graphics.newFont('font.ttf', 32)
            love.graphics.setFont(smallFont) 
        */

        // set up our sound effects; later, we can just index this table and
        // call each entry's `play` method
        sounds = {
        ['paddle_hit'] = love.audio.newSource('sounds/paddle_hit.wav', 'static'),
        ['score'] = love.audio.newSource('sounds/score.wav', 'static'),
        ['wall_hit'] = love.audio.newSource('sounds/wall_hit.wav', 'static')
        }

    // initialize our virtual resolution, which will be rendered within our
    // actual window no matter its dimensions
    push: setupScreen(VIRTUAL_WIDTH, VIRTUAL_HEIGHT, WINDOW_WIDTH, WINDOW_HEIGHT, {
            fullscreen = false,
        resizable = true,
        vsync = true
    })

    // initialize our player paddles; make them global so that they can be
    // detected by other functions and modules
    player1 = Paddle(10, 30, 5, 20)
        player2 = Paddle(VIRTUAL_WIDTH - 10, VIRTUAL_HEIGHT - 30, 5, 20)

    // place a ball in the middle of the screen
    ball = Ball(VIRTUAL_WIDTH / 2 - 2, VIRTUAL_HEIGHT / 2 - 2, 4, 4)


    aiController = PaddleAutoController(ball, player2, PADDLE_SPEED / DIFFICULTY_DAMP)
        //~ aiController = PaddleAutoController()

        // initialize score variables
    player1Score = 0
        player2Score = 0

    // either going to be 1 or 2; whomever is scored on gets to serve the
    // following turn
    servingPlayer = 1

    // player who won the game; not set to a proper value until we reach
    // that state in the game
    winningPlayer = 0

    //~ amount of real players
    singlePlayer = 0

    // the state of our game; can be any of the following:
    // 1. 'start' (the beginning of the game, before first serve)
    // 2. 'serve' (waiting on a key press to serve the ball)
    // 3. 'play' (the ball is in play, bouncing between paddles)
    // 4. 'done' (the game is over, with a victor, ready for restart)
    gameState = 'start'
    }

}