using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    private float _damage;
    private float _speed;
    private GameObject _owner;
    private Vector2 _direction;
    public int bounceCount = 0;
    public int bounceMultiplier = 1;
    public int pierceCount = 0;
    public float bounceSpeedMultiplier = 1f;

    void Awake()
    {

    }

    public void Init(Vector2 targetPos, EntityStats ownerStats)
    {
        _owner = ownerStats.gameObject;
        _damage = ownerStats.GetAttack();
        _speed = ownerStats.GetAttackSpeed();
        _direction = targetPos - (Vector2)transform.position;
        ProjectileManager.Instance.AddProjectile(this);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(_speed * Time.deltaTime * _direction);
        if (!CameraController.IsInScene(transform.position))
            ProjectileManager.Instance.RemoveProjectile(this);
    }

    private void OnCollisionLeave2D(Collision2D collision)
    {
        if (collision.gameObject.Equals(_owner))
            _owner = null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.Equals(_owner))
            return;
        if (collision.gameObject.TryGetComponent<ProjectileBase>(out ProjectileBase projectileBase))
            return;
        if (collision.gameObject.TryGetComponent<EntityStats>(out EntityStats entityStats))
        {
            // Hit an entity
            entityStats.TakeDamage(_damage);
            if (pierceCount <= 0)
                ProjectileManager.Instance.RemoveProjectile(this);
            pierceCount--;
        }
        else
        {
            // Bouncing off walls
            if (bounceCount <= 0)
                ProjectileManager.Instance.RemoveProjectile(this);

            bounceCount--;
            _direction = Vector3.Reflect(_direction, collision.contacts[0].normal);
            // spawn more projectiles
            for (int i = 1; i < bounceMultiplier; i++)
            {
                // spread the new projectiles out in a 30 degree arc
                Vector3 spreadDirection = Quaternion.AngleAxis(-15f + i * (30f / (bounceMultiplier - 1)), Vector3.forward) * _direction;
                GameObject newProjectile = Instantiate(gameObject);
                newProjectile.transform.position = transform.position;
                ProjectileBase projectileComponent = newProjectile.GetComponent<ProjectileBase>();
                projectileComponent._speed = _speed * bounceSpeedMultiplier;
                projectileComponent._direction = spreadDirection;
                ProjectileManager.Instance.AddProjectile(projectileComponent);
            }
        }

    }
}
