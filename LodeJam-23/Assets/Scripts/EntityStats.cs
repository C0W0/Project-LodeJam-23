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

    public void TakeDamage(float damage, GameObject attacker)
    {
        if (gameObject.GetComponent<BossAI>() != null)
        {
            gameObject.GetComponent<BossAI>().target = attacker;
        }
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
        GameManager.Instance.OnEntityDeath(this);
    }

    public void Attack(EntityStats target)
    {
        Attack(target.transform.position);
    }

    public void Attack(Vector2 targetPos)
    {
        if (IsBoss())
        {
            // Attack in all 4 directions
            for (int i = 0; i < 4; i++)
            {
                Vector2 rotatedTargetPos = (Vector2) (Quaternion.Euler(0, 0, 90 * i) * (targetPos - (Vector2)transform.position)) + (Vector2)transform.position;
                GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                ProjectileBase projectileBase = projectile.GetComponent<ProjectileBase>();
                projectileBase.Init(rotatedTargetPos, this);
            }
        }
        else
        {
            // Attack in the specified direction
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            ProjectileBase projectileBase = projectile.GetComponent<ProjectileBase>();
            projectileBase.Init(targetPos, this);
        }
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

    public void ChangeDamage(float change)
    {
        attackDamage += change;
    }
    
    public void ChangeDefence(float change)
    {
        defense += change;
    }
    
    public void ChangeSpeed(float change)
    {
        speed += change;
    }
    
    public void ChangeBulletSpeed(float change)
    {
        attackSpeed += change;
    }

    public void ChangeMaxHp(float change)
    {
        maxHealth += change;
        _currentHealth = maxHealth;
        if (GameManager.Instance.GetPlayerEntity() == this)
        {
            PlayerController.Instance.RefreshHealthBar();
        }
    }
}
