using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterInput : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    [SerializeField] private BallFire ballFire;

    [SerializeField] private InputActionAsset actionAsset;

    [SerializeField] private string actionMapName;

    private InputActionMap _inputActionMap;
    
    private IEnumerator _powerCoroutine;

    private IEnumerator _angleCoroutine;

    private void Awake()
    {
        _inputActionMap = actionAsset.FindActionMap(actionMapName, true);
        _inputActionMap["Movement"].performed += OnMovement;
        _inputActionMap["Shoot"].performed += OnShoot;
        _inputActionMap["PowerAndDirection"].performed += OnPowerAndDirection;
    }

    private void OnEnable()
    {
        _inputActionMap.Enable();
    }

    private void OnDisable()
    {
        _inputActionMap.Disable();
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
