using UnityEngine;

public class EntityStats : MonoBehaviour
{
    [SerializeField]
    private float maxHealth = 100;
    private float _currentHealth;
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
        _currentHealth = maxHealth;
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

    public float TakeDamage(float damage)
    {
        if (GameManager.Instance.GetPlayerEntity() == this)
        {
            PlayerController.Instance.PlayerHealthbar.OnPlayerHealthChange();
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
        return damageTaken;
    }

    public void Attack(EntityStats target)
    {
        // TODO
    }

    void OnDeath()
    {
        Debug.Log(gameObject.name + " has died.");
        Destroy(gameObject);
    }
}
