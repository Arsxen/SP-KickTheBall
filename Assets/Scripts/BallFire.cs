using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BallFire : MonoBehaviour
{
    [SerializeField] private GameObject chargeBar;
    
    [SerializeField] private float minPower = 10;
    
    [SerializeField] private float maxPower = 100;

    [SerializeField] private float powerStep = 0.01f;
    
    [SerializeField] private Vector2 offset = Vector2.zero;

    [SerializeField] private float shootAngle = 90.0f;

    [SerializeField] private float angleStep = 1;

    private float MinAngle => 0 - (shootAngle / 2);

    private float MaxAngle => 0 + (shootAngle / 2);

    private float _power;
    
    public float Power { 
        get => _power;
        private set => _power = Mathf.Clamp(value, minPower, maxPower);
    }

    private Slider _chargeSlider;

    private RectTransform _chargeRectTransform;
    
    private Camera _mainCamera;

    private Transform _shootingArrow;

    private float _angle;

    public float Angle
    {
        get => _angle; 
        private set => _angle = Mathf.Clamp(value, MinAngle, MaxAngle);
    }

    public bool IsCharging { get; private set; }

    private void Start()
    {
        Power = minPower;
        _chargeSlider = chargeBar.GetComponent<Slider>();
        _chargeRectTransform = chargeBar.GetComponent<RectTransform>();
        _mainCamera = Camera.main;
        chargeBar.SetActive(IsCharging);
        _shootingArrow = transform.Find("ShootingArrow").transform;
        _shootingArrow.gameObject.SetActive(false);
        _shootingArrow.rotation = Quaternion.AngleAxis(MinAngle, Vector3.forward);
        Angle = MinAngle;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!IsCharging) return;

        // Power += powerStep * Input.GetAxisRaw("Horizontal");
        // Angle += angleStep * Input.GetAxisRaw("Vertical");

        UpdateChargeBar();
        UpdateShootingAngle();
    }

    private void UpdateChargeBar()
    {
        _chargeSlider.value = Mathf.InverseLerp(minPower, maxPower, Power);
        _chargeRectTransform.position = _mainCamera.WorldToScreenPoint(transform.position + (Vector3) offset);
    }

    private void UpdateShootingAngle()
    {
        _shootingArrow.rotation = Quaternion.AngleAxis(Angle, Vector3.forward);
    }

    public void StartCharging()
    {
        _shootingArrow.gameObject.SetActive(true);
        chargeBar.SetActive(true);
        IsCharging = true;
    }

    private void StopCharging()
    {
        _shootingArrow.gameObject.SetActive(false);
        chargeBar.SetActive(false);
        IsCharging = false;
    }

    public void ShootBall()
    {
        StopCharging();
        var force = Quaternion.AngleAxis(Angle, Vector3.forward) * Vector3.right;
        force *= Power;
        GameManager.Main.Ball.ShootBall(force);
    }

    public IEnumerator AdjustPower(float direction)
    {
        while (true)
        {
            Power += powerStep * Time.deltaTime * direction;
            yield return null;
        }
    }

    public IEnumerator AdjustAngle(float direction)
    {
        while (true)
        {
            Angle += angleStep * Time.deltaTime * direction;
            yield return null;
        }
    }
}
