using System.Collections.Generic;
using UnityEngine;

public class BrickSpawner : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _playArea;
    [SerializeField] private GameObject _brickPrefab;
    
    private Vector2Int _gridSize = new Vector2Int(10, 10);
    [Range(0f, 0.5f)] private float _padding = 0.1f;
    private Vector2 _cellSize;
    private Vector2 _playAreaSize;
    private List<Vector2Int> _availableCells = new List<Vector2Int>();

    public void CalculateGrid()
    {
        _playAreaSize = _playArea.bounds.size;
        
        _cellSize = new Vector2(
            _playAreaSize.x / _gridSize.x,
            _playAreaSize.y / _gridSize.y
        );
    }

    public void GenerateAvailableCells()
    {
        if (_availableCells.Count > 0)
        {
            _availableCells.Clear();
        }
        
        for (int x = 0; x < _gridSize.x; x++)
        {
            for (int y = 0; y < _gridSize.y; y++)
            {
                _availableCells.Add(new Vector2Int(x, y));
            }
        }
    }

    public List<GameObject> SpawnBricks(int amount)
    {
        amount = Mathf.Clamp(amount, 0, _availableCells.Count);
        List<GameObject> instantiatedBricks = new List<GameObject>();
        
        for (int i = 0; i < amount; i++)
        {
            int randomIndex = Random.Range(0, _availableCells.Count);
            Vector2Int cell = _availableCells[randomIndex];
            _availableCells.RemoveAt(randomIndex);
            
            Vector2 position = _playArea.transform.position;
            position.x += cell.x * _cellSize.x - _playAreaSize.x / 2 + _cellSize.x / 2;
            position.y += cell.y * _cellSize.y - _playAreaSize.y / 2 + _cellSize.y / 2;
            
            GameObject brick = Instantiate(_brickPrefab, position, Quaternion.identity);
            instantiatedBricks.Add(brick);
            
            brick.transform.localScale = new Vector2(
                _cellSize.x * (1 - _padding),
                _cellSize.y * (1 - _padding)
            );
        }
        
        return instantiatedBricks;
    }
}
