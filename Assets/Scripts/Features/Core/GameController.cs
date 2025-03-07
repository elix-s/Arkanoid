using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameController : MonoBehaviour
{
    [Header("Links to the main components of the game scene")]
    [SerializeField] private BrickSpawner _brickSpawner;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private GameObject _ballPrefab;
    
    [Header("Services")]
    private IObjectResolver _container;
    private GameSessionService _gameSessionService;
    private UIService _uiService;
    private EventsDispatcher _dispatcher;
    private BallController _ballController;
    private Logger _logger;
    
    private List<GameObject> _instantiatedBricks = new List<GameObject>();
    private Vector2 _ballSpawnPosition = new Vector2(0.0f, -1.26f);

    [Inject]
    private void Construct(GameSessionService gameSessionService, IObjectResolver container, UIService uiService,
        EventsDispatcher dispatcher, Logger logger)
    {
        _gameSessionService = gameSessionService;
        _container = container;
        _uiService = uiService;
        _dispatcher = dispatcher;
        _logger = logger;
    }

    private void Awake()
    {
        _dispatcher.GameDispatcher.Subscribe<GameReload>(this, OnGameReload);
        _dispatcher.GameDispatcher.Subscribe<ExitGame>(this, OnGameExit);
        
        StartInit();
    }

    private async void StartInit()
    {
        _brickSpawner.CalculateGrid();
        _brickSpawner.GenerateAvailableCells();
        
        _ballController = _container.Instantiate(_ballPrefab).GetComponent<BallController>();
        _ballController.transform.localPosition = _ballSpawnPosition;

        var totalBricks = 0;
        
        if (_gameSessionService.TotalBricks > 0)
        {
            totalBricks = _gameSessionService.TotalBricks;
        }
        else
        {
            totalBricks = 1;
        }
        
        _instantiatedBricks = _brickSpawner.SpawnBricks(totalBricks);
        
        _playerController.Init(true);
    
        await UniTask.Delay(1000);
        _ballController.Init(this);
    }

    public void BrickCounter(GameObject brick)
    {
        _instantiatedBricks.Remove(brick);
        
        if(_instantiatedBricks.Count <= 0)
        {
            _logger.Log("Victory!");
            EndGame();
        }
    }

    public void EndGame()
    {
        _uiService.ShowUIPanelWithComponent<GameUIView>("GameUICanvas").Forget();
        _playerController.Init(false);
        Destroy(_ballController.gameObject);
    }

    public void Reload()
    {
        if (_instantiatedBricks.Count > 0)
        {
            foreach (var i in _instantiatedBricks)
            {
                Destroy(i);
            }
            
            _instantiatedBricks.Clear();
        }
        
        StartInit();
    }
    
    private void OnGameReload(GameReload e)
    {
       Reload();
    }
    
    private void OnGameExit(ExitGame e)
    {
        if (_instantiatedBricks.Count > 0)
        {
            foreach (var i in _instantiatedBricks)
            {
                Destroy(i);
            }
            
            _instantiatedBricks.Clear();
        }
    }
}
