using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TestObject : MonoBehaviour
{
    [SerializeField]
    private float speed;

    private float _dashingTimerInSec = 0.0f;

    private Vector2? _dashDirection = null; 
    
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
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_dashingTimerInSec > 0.0f)
        {
            _dashingTimerInSec -= Time.deltaTime;
            // dash
            transform.Translate(_dashDirection.Value * 50 * Time.deltaTime);
            return;
        }
         // get cursor position
         Vector2 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);


        Vector2 moveDirection = Vector2.zero;
        foreach (var (key, direction) in _keycodeMap)
        {
            if (Input.GetKey(key))
            {
                moveDirection += direction;
            }
        }

        if (moveDirection.magnitude != 0f)
        {
            float scaleFactor = speed / moveDirection.magnitude;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                scaleFactor *= 2;
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                this._dashingTimerInSec = 0.1f;
                this._dashDirection = moveDirection;
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
}
