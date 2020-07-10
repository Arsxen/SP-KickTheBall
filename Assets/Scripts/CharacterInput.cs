using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterInput : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    [SerializeField] private BallFire ballFire;
    
    private PlayerInputActions _inputActions;

    private IEnumerator _powerCoroutine;

    private IEnumerator _angleCoroutine;

    private void Awake()
    {
        _inputActions = new PlayerInputActions();
        _inputActions.Player.Movement.performed += OnMovement;
        _inputActions.Player.Shoot.performed += OnShoot;
        _inputActions.Player.PowerAndDirection.performed += OnPowerAndDirection;
    }

    private void OnEnable()
    {
        _inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Player.Disable();
    }

    private void OnMovement(InputAction.CallbackContext context)
    {
        var movement = context.ReadValue<Vector2>();
        playerController.Move(movement);
    }

    private void OnShoot(InputAction.CallbackContext context)
    {
        if (!ballFire.IsCharging) return;
        ballFire.ShootBall();
        playerController.EnableMovement();
    }

    private void OnPowerAndDirection(InputAction.CallbackContext context)
    {
        if (!ballFire.IsCharging) return;
        var val = context.ReadValue<Vector2>();
        if (_powerCoroutine == null)
        {
            _powerCoroutine = ballFire.AdjustPower(val.x);
            StartCoroutine(_powerCoroutine);
        }
        else
        {
            StopCoroutine(_powerCoroutine);
            _powerCoroutine = null;
        }

        if (_angleCoroutine == null)
        {
            _angleCoroutine = ballFire.AdjustAngle(val.y);
            StartCoroutine(_angleCoroutine);
        }
        else
        {
            StopCoroutine(_angleCoroutine);
            _angleCoroutine = null;
        }

    }
}
