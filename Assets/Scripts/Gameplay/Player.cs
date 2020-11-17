using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{

    public event Action PlayerHitEvent;

    [SerializeField] private float _fireRate = 0.2f;
    [SerializeField] private SpriteRenderer _playerSprite;
    [SerializeField] private SpriteRenderer _thrusterSprite;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private float _turnSpeed = 200f;
    [SerializeField] private float _thrustSpeed = 300f;
    
    [SerializeField] private GameObject _particles;
    [SerializeField] private float _invincibilityTime = 5f;
    [SerializeField] private float _invincibilityFlashDelay = 0.3f;
    [SerializeField] private ObjectPoolManager _objectPoolManager;
    
    private readonly Color _invincibleColor = new Color(1f,1f,1f,0.6f);
    private InputActions _inputActions;
    private Rigidbody2D _rb2d;
    private bool _invincible;
    private float _nextFireTime;
    
    void Awake()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _inputActions = new InputActions();
    }
    
    private void OnEnable()
    {
        _inputActions.Enable();
        _invincible = true;
        StartCoroutine(SpawnCo());
    }

    private void OnDisable() => _inputActions.Disable();
    

    private IEnumerator SpawnCo()
    {
        float endTime = Time.time + _invincibilityTime;
        while (Time.time < endTime)
        {
            _playerSprite.color = _invincibleColor;
            _thrusterSprite.color = _invincibleColor;
            yield return new WaitForSeconds(_invincibilityFlashDelay);
            _playerSprite.color = Color.white;
            _thrusterSprite.color = Color.white;
            yield return new WaitForSeconds(_invincibilityFlashDelay);
        }

        _invincible = false;
    }
    
    void FixedUpdate()
    {
        CheckRotation();
        CheckThrust();
    }

    private void CheckThrust()
    {
        bool thrusting = _inputActions.Player.Thrust.ReadValue<float>() > 0.1f;
        if (thrusting)
        {
            float angle = _rb2d.rotation * Mathf.Deg2Rad;
            var dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            _rb2d.AddForce(dir * (_thrustSpeed * Time.fixedDeltaTime));
            
        }

        _thrusterSprite.gameObject.SetActive(thrusting);
    }

    private void CheckRotation()
    {
        float direction = _inputActions.Player.Rotate.ReadValue<float>();
        if (Mathf.Abs(direction) > 0.1f)
        {
            _rb2d.SetRotation(_rb2d.rotation + (direction * _turnSpeed * Time.fixedDeltaTime));
        }
    }

    void Update()
    {
        CheckFire();
    }

    private void CheckFire()
    {
        bool firing = _inputActions.Player.Fire.ReadValue<float>() > 0.1f;
        if (Time.time > _nextFireTime && firing)
        {
            Fire();
        }
    }

    private void Fire()
    {
        _nextFireTime = Time.time + _fireRate;
        
        float angleDeg = _rb2d.rotation;
        float angleRad = angleDeg * Mathf.Deg2Rad;
        var laser = _objectPoolManager.GetPooledObject(_laserPrefab, transform.position, Quaternion.Euler(0f, 0f, angleDeg)).GetComponent<Laser>();
        var fireDir = new Vector2(Mathf.Cos(angleRad),Mathf.Sin(angleRad));
        laser.Setup(fireDir);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_invincible)
        {
            return;
        }

        if (other.CompareTag("Meteor"))
        {
            PlayerHitEvent?.Invoke();
            Instantiate(_particles, transform.position, Quaternion.identity);
        }
    }
    
}
