using UnityEngine;
using TMPro;
using VContainer;

public class BricksNumberView : MonoBehaviour
{
    [SerializeField] private TMP_InputField _bricksCountInputField;
    private GameSessionService _gameSessionService;

    [Inject]
    private void Construct(GameSessionService gameSessionService)
    {
        _gameSessionService = gameSessionService;
    }
    
    private void Awake()
    {
        _bricksCountInputField.text = _gameSessionService.TotalBricks.ToString();
        _bricksCountInputField.onValueChanged.AddListener(OnBricksCountChanged);
    }

    private void OnBricksCountChanged(string value)
    {
        int parsedValue;
        
        if (int.TryParse(value, out parsedValue))
        {
            _gameSessionService.TotalBricks = parsedValue;
        }
        else
        {
            _gameSessionService.TotalBricks = 30; 
        }
    }
}
