using System;
using UnityEngine;

public class EntityStats : MonoBehaviour
{
    [SerializeField]
    private float maxHealth = 10;
    private float _currentHealth;
    [SerializeField]
    private float attackDamage = 10;
    [SerializeField]
    private float defense = 5;
    [SerializeField]
    private float speed = 3;
    [SerializeField]
    private float attackSpeed = 5;
    [SerializeField]
    private GameObject projectilePrefab;
    [SerializeField]
    private bool boss;

    // Start is called before the first frame update
    void Awake()
    {
        _currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(float damage)
    {
        float damageTaken = damage - defense;
        if (damageTaken < 0)
        {
            damageTaken = 0;
        }
        
        _currentHealth -= damageTaken;
        if (_currentHealth < 0)
        {
            _currentHealth = 0;
            OnDeath();
        }
        
        if (GameManager.Instance.GetPlayerEntity() == this)
        {
            PlayerController.Instance.OnPlayerHealthChange();
            PlayerController.Instance.OnDamageTaken();
        }
    }

    public void Heal(float healAmount)
    {
        _currentHealth = Math.Min(maxHealth, _currentHealth + healAmount);
        if (GameManager.Instance.GetPlayerEntity() == this)
        {
            PlayerController.Instance.OnPlayerHealthChange();
        }
    }

    void OnDeath()
    {
        if (GameManager.Instance.GetPlayerEntity() == this)
        {
            PlayerController.Instance.OnPlayerDeath();
        }
        Debug.Log(gameObject.name + " has died.");
        GameManager.Instance.OnEntityDeath(this);
    }

    public void Attack(EntityStats target)
    {
        Attack(target.transform.position);
    }

    public void Attack(Vector2 targetPos)
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        ProjectileBase projectileBase = projectile.GetComponent<ProjectileBase>();
        projectileBase.Init(targetPos, this);
    }

    public void ChangeSpeed(int speedIncrease)
    {
        speed += speedIncrease;
    }

    public bool IsBoss()
    {
        return boss;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetCurrentHealth()
    {
        return _currentHealth;
    }

    public float GetAttack()
    {
        return attackDamage;
    }

    public float GetDefense()
    {
        return defense;
    }

    public float GetSpeed()
    {
        return speed;
    }

    public float GetAttackSpeed()
    {
        return attackSpeed;
    }
}
