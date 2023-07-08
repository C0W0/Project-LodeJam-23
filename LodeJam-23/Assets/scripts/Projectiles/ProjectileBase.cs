using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    [SerializeField]
    private float speed;
    
    private Vector3 _direction;

    void Awake()
    {
        
    }

    public void Init(Vector3 targetPos)
    {
        _direction = targetPos - transform.position;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(speed*Time.deltaTime*_direction);
        if (!CameraController.IsInScene(transform.position))
        {
            ProjectileManager.Instance.RemoveProjectile(this);
        }
    }
}
