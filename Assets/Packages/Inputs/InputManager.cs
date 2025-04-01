using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;
using static GameManager;

public class InputManager : MonoBehaviour
{
    Controls player1Controls;
    Controls player2Controls;
    InputUser player1Input;
    InputUser player2Input;
    GameManager game;
    public void Constructor(GameManager game) => this.game = game;
    void Awake()
    {
        SetUpInputSystem();
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
    void OnEnable()
    {
        game.OnGameStateChanged += OnGameStateChanged;
        player1Controls.MapAwaitContinue.Continue.performed += Continue;
        player1Controls.MapPlayerSelection.SelectSinglePlayer.performed += SelectSinglePlayer;
        player1Controls.MapPlayerSelection.SelectMultiPlayer.performed += SelectMultiPlayer;
        player1Controls.MapPlayer.Move.performed += MovePlayer1;
        player2Controls.MapPlayer.Move.performed += MovePlayer2;
        player1Controls.Always.Quit.performed += Quit;

        InputUser.onUnpairedDeviceUsed += OnUnpairedDeviceUsed;
    }
    void OnDisable()
    {
        game.OnGameStateChanged -= OnGameStateChanged;
        player1Controls.MapAwaitContinue.Continue.performed -= Continue;
        player1Controls.MapPlayerSelection.SelectSinglePlayer.performed -= SelectSinglePlayer;
        player1Controls.MapPlayerSelection.SelectMultiPlayer.performed -= SelectMultiPlayer; ;
        player1Controls.MapPlayer.Move.performed -= MovePlayer1;
        player2Controls.MapPlayer.Move.performed -= MovePlayer2;
        player1Controls.Always.Quit.performed -= Quit;

        InputUser.onUnpairedDeviceUsed -= OnUnpairedDeviceUsed;
    }
    void OnGameStateChanged(GameManager.GameStates gameState)
    {
        player1Controls.Disable();
        player2Controls.Disable();
        player1Controls.Always.Enable();
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
    void Continue(InputAction.CallbackContext context)
    {
        Debug.Log($"Continue: {context.phase}", this);
        if (context.performed)
            game.Continue();
    }
    void SelectSinglePlayer(InputAction.CallbackContext context)
    {
        if (context.performed)
            game.SelectSinglePlayer();
    }
    void SelectMultiPlayer(InputAction.CallbackContext context)
    {
        if (context.performed)
            game.SelectMultiPlayer();
    }
    void MovePlayer1(InputAction.CallbackContext context)
    {
        if (context.performed)
            game.MovePlayer1(context.ReadValue<float>());
    }
    void MovePlayer2(InputAction.CallbackContext context)
    {
        if (context.performed)
            game.MovePlayer2(context.ReadValue<float>());
    }
    void Quit(InputAction.CallbackContext context)
    {
        if (context.performed)
            game.Quit();
    }
    void OnUnpairedDeviceUsed(InputControl control, InputEventPtr ptr)
    {
        if (player1Input == null)
            player1Input = InputUser.PerformPairingWithDevice(control.device);
        else
            player2Input = InputUser.PerformPairingWithDevice(control.device);
    }
}