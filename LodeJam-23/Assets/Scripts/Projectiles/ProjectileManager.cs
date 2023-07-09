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

    public void AddProjectile(ProjectileBase projectile)
    {
        _projectiles.Add(projectile);
    }

    public void RemoveProjectile(ProjectileBase projectile)
    {
        _projectiles.Remove(projectile);
        Destroy(projectile.gameObject);
    }

    public void RemoveAllProjectiles()
    {
        foreach (var projectile in _projectiles)
        {
            Destroy(projectile.gameObject);
        }
        _projectiles = new HashSet<ProjectileBase>();
    }
}
