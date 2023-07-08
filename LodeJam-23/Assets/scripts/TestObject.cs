using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestObject : MonoBehaviour
{
    [SerializeField]
    private float speed;
    
    private Dictionary<KeyCode, Vector2> _keycodeMap = new Dictionary<KeyCode, Vector2>
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
            transform.Translate(moveDirection*scaleFactor);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            CameraController.Instance.FollowObject(this.transform);
        }
    }
}
