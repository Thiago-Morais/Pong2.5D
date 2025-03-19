using UnityEngine;
using UnityEngine.UIElements;
using GameStates = GameManager.GameStates;

public class UIManager : MonoBehaviour
{
    [SerializeField] UIDocument uiDocument;
    [SerializeField] VisualTreeAsset startUI;
    [SerializeField] VisualTreeAsset menuUI;
    [SerializeField] VisualTreeAsset serveUI;
    [SerializeField] VisualTreeAsset playUI;
    [SerializeField] VisualTreeAsset doneUI;
    VisualElement Root => uiDocument.rootVisualElement;
    [ContextMenu(nameof(Test_start))] void Test_start() => SetState(GameStates.start);
    [ContextMenu(nameof(Test_menu))] void Test_menu() => SetState(GameStates.menu);
    [ContextMenu(nameof(Test_serve))] void Test_serve() => SetState(GameStates.serve);
    [ContextMenu(nameof(Test_play))] void Test_play() => SetState(GameStates.play);
    [ContextMenu(nameof(Test_done))] void Test_done() => SetState(GameStates.done);
    void Update()
    {
        UpdateFPSDisplay();
    }
    public void SetState(GameStates uiState)
    {
        switch (uiState)
        {
            case GameStates.start:
                uiDocument.visualTreeAsset = startUI;
                break;
            case GameStates.menu:
                uiDocument.visualTreeAsset = menuUI;
                break;
            case GameStates.serve:
                uiDocument.visualTreeAsset = serveUI;
                TextElement serveText = Root.Q<TextElement>("serve-text");
                serveText.text = $"Player {GameManager.Instance.ServingPlayerId}'s serve!";
                break;
            case GameStates.play:
                uiDocument.visualTreeAsset = playUI;
                break;
            case GameStates.done:
                uiDocument.visualTreeAsset = doneUI;
                TextElement winningText = Root.Q<TextElement>("winning-text");
                winningText.text = $"Player {GameManager.Instance.WinningPlayerId} wins!";
                break;
            default:
                break;
        }
        UpdateScore();
    }
    public void UpdateScore()
    {
        Root.Q<TextElement>("score-player-1").text = $"{GameManager.Instance.Player1Score}";
        Root.Q<TextElement>("score-player-2").text = $"{GameManager.Instance.Player2Score}";
    }
    public void UpdateFPSDisplay() => Root.Q<TextElement>("fps-display").text = $"FPS: {(int)(1 / Time.smoothDeltaTime)}";
}