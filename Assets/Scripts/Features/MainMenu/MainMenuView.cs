using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class MainMenuView : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Button _playButton;
    [SerializeField] private TMP_InputField _bricksCountInputField;
    private GameStateService _gameStateService;
    
    [Inject]
    private void Construct(GameStateService gameStateService)
    {
        _gameStateService = gameStateService;
    }

    private void Awake()
    {
        _playButton.onClick.AddListener(()=> _gameStateService.ChangeState<StartGameState>());
    }
}
