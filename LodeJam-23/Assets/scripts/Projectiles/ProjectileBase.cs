using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    public float damage;
    public float speed;
    public GameObject source;
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
        transform.Translate(speed * Time.deltaTime * _direction);
        if (!CameraController.IsInScene(transform.position))
        {
            ProjectileManager.Instance.RemoveProjectile(this);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.Equals(source))
        {
            return;
        }
        if (collision.gameObject.TryGetComponent<EntityStats>(out EntityStats entityStats))
        {
            entityStats.TakeDamage(damage);
        }
    }
}
