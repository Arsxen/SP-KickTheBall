using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 1.0f;
    
    [SerializeField] private Vector2 feetOffset = new Vector2(1.0f, -0.5f);
    
    private Vector2 _movementDirection = Vector2.zero;

    private Rigidbody2D _rgBody2D;

    private Animator _animator;

    private BallFire _ballFire;

    private bool _canMove = true;

    private static readonly int IsRunning = Animator.StringToHash("isRunning");

    // Start is called before the first frame update
    void Start()
    {
        _rgBody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _ballFire = GetComponent<BallFire>();
    }

    private void FixedUpdate()
    {
        _rgBody2D.MovePosition(_rgBody2D.position + _movementDirection * (movementSpeed * Time.fixedDeltaTime));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Ball")) return;
        DisableMovement();
        GameManager.Main.Ball.StopBall();
        GameManager.Main.Ball.TeleportBall(GetFeetPosition());
        TurnCharacter(1);
        _ballFire.StartCharging();
    }

    private void DisableMovement()
    {
        _movementDirection = Vector2.zero;
        _canMove = false;
        _animator.SetBool(IsRunning, false);
    }

    public void EnableMovement()
    {
        _canMove = true;
    }

    private Vector2 GetFeetPosition()
    {
        return (Vector2) transform.position + feetOffset;
    }

    private void TurnCharacter(float direction)
    {
        if (direction > 0)
        {
            transform.rotation = Quaternion.identity;
        }
        else if (direction < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    public void Move(Vector2 direction)
    {
        if (!_canMove) return;
        _movementDirection = direction;
        _animator.SetBool(IsRunning, direction != Vector2.zero);
        TurnCharacter(direction.x);
    }

}
