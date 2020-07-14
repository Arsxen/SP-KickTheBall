using System;
using System.Collections;
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

    private bool _canTurn = true;

    private static readonly int IsRunning = Animator.StringToHash("isRunning");

    private WaitForFixedUpdate _waitForNextFixedUpdate;

    private bool _isFlipped;

    // Start is called before the first frame update
    private void Start()
    {
        _rgBody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _ballFire = GetComponent<BallFire>();
        _waitForNextFixedUpdate = new WaitForFixedUpdate();
        _isFlipped = Math.Abs(transform.eulerAngles.y - 180.0f) < 10;
    }

    private void FixedUpdate()
    {
        _rgBody2D.MovePosition(_rgBody2D.position + _movementDirection * (movementSpeed * Time.fixedDeltaTime));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!GameManager.Main.enabledCatching) return;
        if (!other.CompareTag("Ball")) return;
        DisableMovement();
        StartCoroutine(SetUpBall());
        _ballFire.StartCharging();
        GameManager.Main.DisableCatching();
    }

    private IEnumerator SetUpBall()
    {
        GameManager.Main.Ball.StopBall();
        GameManager.Main.Ball.TeleportBall(GetFeetPosition());
        yield return _waitForNextFixedUpdate;
        yield return _waitForNextFixedUpdate;
        GameManager.Main.Ball.transform.parent = transform;
    }

    public void DisableMovement(bool canTurn = true)
    {
        _movementDirection = Vector2.zero;
        _canMove = false;
        _canTurn = canTurn;
        _animator.SetBool(IsRunning, false);
    }

    public void EnableMovement()
    {
        _canTurn = true;
        _canMove = true;    
    }

    private Vector2 GetFeetPosition()
    {
        var newOffset = feetOffset;
        if (_isFlipped)
        {
            newOffset.x *= -1;
        }
        return (Vector2) transform.position + newOffset;
    }

    private void TurnCharacter(float direction)
    {
        if (direction > 0)
        {
            transform.rotation = Quaternion.identity;
            _isFlipped = false;
        }
        else if (direction < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            _isFlipped = true;
        }
    }

    public void Move(Vector2 direction)
    {
        if (_canMove)
        {
            _movementDirection = direction;
            _animator.SetBool(IsRunning, direction != Vector2.zero);
        }

        if (_canTurn)
        {
            TurnCharacter(direction.x);
        }
    }

}
