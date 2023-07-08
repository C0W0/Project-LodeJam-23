using UnityEngine;

public class EntityStats : MonoBehaviour
{
    [SerializeField]
    private float maxHealth = 100;
    private float currentHealth;
    [SerializeField]
    private float attackDamage = 10;
    [SerializeField]
    private float defense = 5;
    [SerializeField]
    private float speed = 3;
    [SerializeField]
    private float attackSpeed = 5;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
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

    public float TakeDamage(float damage)
    {
        float damageTaken = damage - defense;
        if (damageTaken < 0)
        {
            damageTaken = 0;
        }
        currentHealth -= damageTaken;
        if (currentHealth < 0)
        {
            currentHealth = 0;
            OnDeath();
        }
        return damageTaken;
    }

    public float Attack(EntityStats target)
    {
        return target.TakeDamage(attackDamage);
    }

    void OnDeath()
    {
        Debug.Log(gameObject.name + " has died.");
        Destroy(gameObject);
    }
}
