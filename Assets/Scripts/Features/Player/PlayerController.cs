using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _moveStep = 1f; 
    [SerializeField] private float _moveSpeed = 10f;
    
    [Header("Components")]
    [SerializeField] Collider2D _platformCollider;
    
    private float _screenPadding = 0.5f; 
    private Vector3 _targetPosition;
    private bool _isMoving = false;
    private float _minXBound;
    private float _maxXBound;

    private bool _playerIsActive;

    public void Init(bool activity)
    {
        if (activity)
        {
            _playerIsActive = true;
            _platformCollider = GetComponent<Collider2D>();
            CalculateMovementBounds();
        }
        else
        {
            _playerIsActive = false;
        }
    }

    private void CalculateMovementBounds()
    {
        float platformHalfWidth = _platformCollider.bounds.extents.x;
        Vector3 leftBound = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 rightBound = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0));
        
        _minXBound = leftBound.x + platformHalfWidth + _screenPadding;
        _maxXBound = rightBound.x - platformHalfWidth - _screenPadding;
    }

    private void Update()
    {
        if (_playerIsActive)
        {
            HandleInput();
            MovePlatform();
        }
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 touchPosition = GetTouchWorldPosition();
            CalculateTargetPosition(touchPosition);
        }
    }

    private Vector3 GetTouchWorldPosition()
    {
        Vector3 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        touchPos.z = transform.position.z;
        return touchPos;
    }

    private void CalculateTargetPosition(Vector3 touchPosition)
    {
        float direction = touchPosition.x < transform.position.x ? -1 : 1;
        
        _targetPosition = transform.position + Vector3.right * direction * _moveStep;
        _targetPosition.x = Mathf.Clamp(_targetPosition.x, _minXBound, _maxXBound);
        _isMoving = true;
    }

    private void MovePlatform()
    {
        if (!_isMoving) return;

        transform.position = Vector3.MoveTowards(
            transform.position, _targetPosition, _moveSpeed * Time.deltaTime
        );
        
        if (Vector3.Distance(transform.position, _targetPosition) < 0.01f)
        {
            _isMoving = false;
        }
    }
}
