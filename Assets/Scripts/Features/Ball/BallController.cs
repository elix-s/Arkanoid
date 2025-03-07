using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BallController : MonoBehaviour
{
    [Header("Ball Settings")]
    [SerializeField] private float baseSpeed = 5f;
    [Header("Components")]
    [SerializeField] private Rigidbody2D _rb;
    
    private float minAngle = 20f;
    private GameController _gameController;
    private Vector2 lastFrameVelocity;
    private bool _startBall = false;
    
    public void Init(GameController gameController)
    {
        _gameController = gameController;
        LaunchBall();
        _startBall = true;
    }

    private void LaunchBall()
    {
        Vector2 direction = GetCorrectDirection(Random.insideUnitCircle.normalized);
        _rb.velocity = direction * baseSpeed;
    }

    private void Update()
    {
        if (_startBall) lastFrameVelocity = _rb.velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 newDirection = Vector2.Reflect(lastFrameVelocity.normalized, collision.contacts[0].normal);
        newDirection = GetCorrectDirection(newDirection);
        
        _rb.velocity = newDirection * baseSpeed;

        if (collision.gameObject.layer == LayerMask.NameToLayer("Brick"))
        {
            _gameController.BrickCounter(collision.gameObject);
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("DeathZone"))
        {
            _gameController.EndGame();
        }
    }

    private Vector2 GetCorrectDirection(Vector2 originalDirection)
    {
        float angle = Vector2.Angle(originalDirection, Vector2.right);
        
        if(angle < minAngle || angle > 180 - minAngle)
        {
            return CalculateCorrectedDirection(originalDirection);
        }
        
        return originalDirection.normalized;
    }

    private Vector2 CalculateCorrectedDirection(Vector2 dangerousDirection)
    {
        float targetAngle = Mathf.Clamp(Vector2.SignedAngle(Vector2.right, dangerousDirection), 
            minAngle, 180 - minAngle
        );
        
        float rad = targetAngle * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
    }
}
