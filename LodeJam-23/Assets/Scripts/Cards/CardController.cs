using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.EventSystems;

public class CardController : MonoBehaviour
{

    private int _index = -1;
    public int index
    {
        get { return _index; }
        set { _index = value; }
    }

    private readonly Dictionary<int, KeyCode> _keycodeMap = new Dictionary<int, KeyCode>
    {
        {
            1, KeyCode.Alpha1
        },
        {
            2, KeyCode.Alpha2
        },
        {
            3, KeyCode.Alpha3
        },
    };

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_index != -1)
        {
            if (Input.GetKeyDown(_keycodeMap[_index]))
            {
                gameObject.SetActive(false);
            }
        }
    }
}
