using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    public static ProjectileManager Instance;

    [SerializeField]
    private GameObject projectilePrefab;

    private HashSet<ProjectileBase> _projectiles;

	void Awake()
    {
        Instance = this;
        _projectiles = new HashSet<ProjectileBase>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnProjectile(Vector2 initPos, Vector2 targetPos, float speed, float damage, GameObject gameObject)
    {
        GameObject newProjectile = Instantiate(projectilePrefab);
        newProjectile.transform.position = initPos;
        ProjectileBase projectileComponent = newProjectile.GetComponent<ProjectileBase>();
        projectileComponent.damage = damage;
        projectileComponent.speed = speed;
        projectileComponent.source = gameObject;
        _projectiles.Add(projectileComponent);
        projectileComponent.Init(targetPos);
    }

    public void RemoveProjectile(ProjectileBase projectile)
    {
        _projectiles.Remove(projectile);
        Destroy(projectile.gameObject);
    }
}
