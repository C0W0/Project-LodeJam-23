using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    
    private EntityStats _playerEntity;
    private GameObject _playerObject;
    private Rigidbody2D _rigidbody2D;
    
    public BaseHealthBar playerHealthbar;
    
    public bool IsPlayingBoss { get; private set; }

    private float _dashingTimerInSec;
    private Vector2 _moveDirection = Vector2.zero;
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

    void Awake()
    {
        Instance = this;
    }

    public void SetPlayer(EntityStats newPlayerEntity)
    {
        if(_playerObject != null && _playerObject.TryGetComponent<BaseAI>(out BaseAI oldBaseAI))
            oldBaseAI.enabled = true;
        IsPlayingBoss = newPlayerEntity.IsBoss();
        _playerEntity = newPlayerEntity;
        _playerObject = newPlayerEntity.gameObject;
        _rigidbody2D = _playerObject.GetComponent<Rigidbody2D>();
        CameraController.Instance.FollowObject(_playerObject.transform);
        if(_playerObject.TryGetComponent<BaseAI>(out BaseAI baseAi))
            baseAi.enabled = false;
        RefreshHealthBar();
    }

    // Update is called once per frame
    void Update()
    {
        if(!GameManager.IsGameOngoing || _playerEntity == null) return;

        CheckMovement();
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Attack(CameraController.MainCamera.ScreenToWorldPoint(Input.mousePosition));
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            CameraController.Instance.FollowObject(_playerObject.transform);
        }

        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            _playerEntity.TakeDamage(10);
        }

        if (IsPlayingBoss)
            return;
        
        // cycling only works when playing as adventurers
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GameManager.Instance.TryCycleAdvEntity(false);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            GameManager.Instance.TryCycleAdvEntity(true);
        }
    }
    
    private void FixedUpdate()
    {
        if(_playerEntity == null) return;

        if (_dashingTimerInSec > 0.0f)
        {
            Debug.Log("Dashing");
            _dashingTimerInSec -= Time.deltaTime;
            _rigidbody2D.velocity = _moveDirection * (_scaleFactor * 10);
            return;
        }
        _rigidbody2D.velocity = _moveDirection * _scaleFactor;
    }

    private void CheckMovement()
    {
        if(_playerEntity == null) return;
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
            _scaleFactor = _playerEntity.GetSpeed() / _moveDirection.magnitude;
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
        
    }
    
    private void Attack(Vector2 targetPos)
    {
        if(_playerEntity == null) return;
        _playerEntity.Attack(targetPos);
    }

    public void OnDamageTaken()
    {
        CameraController.Instance.ShakeCamera(0.1f, 0.1f);
    }

    public void OnPlayerHealthChange()
    {
        playerHealthbar.OnEntityHealthChange();
    }

    public void OnPlayerDeath()
    {
        if (!IsPlayingBoss)
        {
            if (GameManager.Instance.TryCycleAdvEntity(true))
            {
                return;
            }
        }
        _playerEntity = null;
        _playerObject = null;
        _rigidbody2D = null;
    }

    public void RefreshHealthBar()
    {
        playerHealthbar.OnAttachedEntitySwitch(_playerEntity);
    }
}
