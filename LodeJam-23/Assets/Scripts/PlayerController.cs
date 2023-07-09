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

    // test
    [SerializeField]
    private EntityStats _bossEntity, _advanturerEntity;
    
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
        if (newPlayerEntity.IsBoss() != IsPlayingBoss)
        {
            IsPlayingBoss = newPlayerEntity.IsBoss();
            // TODO: boss swap logic
        }
        _playerEntity = newPlayerEntity;
        _playerObject = newPlayerEntity.gameObject;
        _rigidbody2D = _playerObject.GetComponent<Rigidbody2D>();
        playerHealthbar.OnPlayerCharacterSwitch();
    }

    // Update is called once per frame
    void Update()
    {
        if(_playerEntity == null) return;

        CheckMovement();
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Attack(CameraController.MainCamera.ScreenToWorldPoint(Input.mousePosition));
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            CameraController.Instance.FollowObject(_playerObject.transform);
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            GameManager.Instance.SwapPlayer(IsPlayingBoss ? _advanturerEntity : _bossEntity);
        }

        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            _playerEntity.TakeDamage(10);
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

    public IEnumerator SpeedBoost(int speed, float speedTime)
    {
        if(_playerEntity == null) yield break;
        _playerEntity.ChangeSpeed(speed);
        yield return new WaitForSeconds(speedTime);
        _playerEntity.ChangeSpeed(-speed);
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
        playerHealthbar.OnPlayerHealthChange();
    }

    public void OnPlayerDeath()
    {
        Debug.Log("Player died");
        _playerEntity = null;
        _playerObject = null;
        _rigidbody2D = null;
    }
}
