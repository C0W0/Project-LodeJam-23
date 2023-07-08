using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed;

    private float _dashingTimerInSec;
    private Vector2 _moveDirection = Vector2.zero;
    private Rigidbody2D _rigidbody2D;
    private float _scaleFactor = 1.0f;
    
    private readonly Dictionary<KeyCode, Vector2> _keycodeMap = new Dictionary<KeyCode, Vector2>
    {
        {
            KeyCode.W, Vector2.up
        },
        {
            KeyCode.A, Vector2.left
        },
        {
            KeyCode.S, Vector2.down
        },
        {
            KeyCode.D, Vector2.right
        }
    };

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _dashingTimerInSec = 0f;
    }
    
    public int maxHealth = 50;
    public int currentHealth;
    public HealthBar healthBar;
    
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        _moveDirection = Vector2.zero;
        foreach (var (key, direction) in _keycodeMap)
        {
            if (Input.GetKey(key))
            {
                _moveDirection += direction;
            }
        }
        if (_moveDirection.magnitude != 0f)
        {
            _scaleFactor = speed / _moveDirection.magnitude;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                _scaleFactor *= 2;
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Dash");
                _dashingTimerInSec = 0.1f;
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Attack(CameraController.MainCamera.ScreenToWorldPoint(Input.mousePosition));
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            CameraController.Instance.FollowObject(this.transform);
        }

        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            TakeDamage(10);
        }
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }

    private void FixedUpdate()
    {
        if (_dashingTimerInSec > 0.0f)
        {
            Debug.Log("Dashing");
            _dashingTimerInSec -= Time.deltaTime;
            _rigidbody2D.velocity = _moveDirection * (_scaleFactor * 10);
            return;
        }
        _rigidbody2D.velocity = _moveDirection * _scaleFactor;
    }

    private void Attack(Vector2 pos)
    {
        ProjectileManager.Instance.SpawnProjectile(transform.position, pos);
    }
}
