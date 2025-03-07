using Cysharp.Threading.Tasks;

public class MenuState : IGameState
{
    private Logger _logger;
    private UIService _uiService;
    
    public MenuState(Logger logger, UIService uiService)
    {
        _logger = logger;
        _uiService = uiService;
    }

    public void Enter(object obj)
    {
        _logger.Log("Entering MenuState");
        _uiService.ShowUIPanelWithComponent<MainMenuView>("MainMenu").Forget();
    }
    
    public void Update()
    {
        
    }

    public async void Exit()
    {
        await _uiService.HideUIPanel();
    }
}
