using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class GameUIView : MonoBehaviour
{
    [Header("Links to buttons")]
    [SerializeField] private Button _reloadButton;
    [SerializeField] private Button _toMenuButton;
    
    private EventsDispatcher _dispatcher;
    private GameStateService _gameStateService;

    [Inject]
    private void Construct(EventsDispatcher dispatcher, GameStateService gameStateService)
    {
        _dispatcher = dispatcher;
        _gameStateService = gameStateService;
    }

    private void Awake()
    {
        _reloadButton.onClick.AddListener(()=> GameReload());
        _toMenuButton.onClick.AddListener(()=> ExitGame());
    }

    private void GameReload()
    {
        var e = _dispatcher.GameDispatcher.Get<GameReload>();
        _dispatcher.GameDispatcher.Invoke(e).Forget();
        Destroy(gameObject);
    }

    private void ExitGame()
    {
        var e = _dispatcher.GameDispatcher.Get<ExitGame>();
        _dispatcher.GameDispatcher.Invoke(e).Forget();
        _gameStateService.ChangeState<MenuState>();
    }
}
