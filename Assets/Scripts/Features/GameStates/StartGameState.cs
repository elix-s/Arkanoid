using Common.AssetsSystem;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class StartGameState : IGameState
{
    private readonly IAssetProvider _assetProvider;
    private readonly IAssetUnloader _assetUnloader;
    private Logger _logger;
    private IObjectResolver _container;
    
    public StartGameState(Logger logger, IAssetProvider assetProvider, IAssetUnloader assetUnloader, 
        IObjectResolver container)
    {
        _logger = logger;
        _assetProvider = assetProvider;
        _assetUnloader = assetUnloader;
        _container = container;
    }

    public async void Enter(object obj)
    {
        _logger.Log("Entering GameState");
        var gameState = await _assetProvider.GetAssetAsync<GameObject>("GameState");
        _assetUnloader.AddResource(gameState);
        var prefab = _container.Instantiate(gameState);
        _assetUnloader.AttachInstance(prefab);
    }
    
    public void Update() {}

    public void Exit()
    {
        _assetUnloader.Dispose();
    }
}