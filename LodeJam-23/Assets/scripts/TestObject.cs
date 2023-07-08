using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class TestObject : MonoBehaviour
{
    [SerializeField]
    private float speed;

    private float _dashingTimerInSec = 0.0f;


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
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
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
            this._scaleFactor = speed / _moveDirection.magnitude;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                this._scaleFactor *= 2;
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                this._dashingTimerInSec = 0.1f;
            }
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                print("Attack");
            }
            transform.Translate(moveDirection * scaleFactor * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            CameraController.Instance.FollowObject(this.transform);
        }
    }

    protected virtual void Attack(Vector2 pos)
    {
        ProjectileManager.Instance.SpawnProjectile(transform.position, pos);
    }
}
