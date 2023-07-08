using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class PlayerController : MonoBehaviour
{
    private EntityStats _playerEntity;
    private GameObject _playerObject;
    private Rigidbody2D _rigidbody2D;
    // TODO: get rid of this and use playerEntity
    [SerializeField]
    private float speed;


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

    public void SetPlayer(EntityStats newPlayerEntity)
    {
        _playerEntity = newPlayerEntity;
        _playerObject = newPlayerEntity.gameObject;
        _rigidbody2D = _playerObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
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

    private void CheckMovement()
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
        
    }

    public void IncreaseSpeed(int speedDelta)
    {
        speed += speedDelta;
    }

    public IEnumerator SpeedBoost(int speed, float speedTime)
    {
        IncreaseSpeed(speed);
        yield return new WaitForSeconds(speedTime);
        IncreaseSpeed(-speed);
    }
    
    private void Attack(Vector2 pos)
    {
        ProjectileManager.Instance.SpawnProjectile(transform.position, pos, _entityStats.GetAttackSpeed(), _entityStats.GetAttack(), gameObject);
    }
}
