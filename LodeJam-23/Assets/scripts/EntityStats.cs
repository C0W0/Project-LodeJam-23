using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStats : MonoBehaviour
{
    [SerializeField]
    private float maxHealth;
    private float currentHealth;
    [SerializeField]
    private float attack;
    [SerializeField]
    private float defense;
    [SerializeField]
    private float speed;


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float getMaxHealth()
    {
        return maxHealth;
    }

    public float getCurrentHealth()
    {
        return currentHealth;
    }

        public float getAttack()
    {
        return attack;
    }

    public float getDefense()
    {
        return defense;
    }

    public float getSpeed()
    {
        return speed;
    }

    public float takeDamage(float damage)
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
        }
        return damageTaken;
    }

    public float doAttack(EntityStats target)
    {
        return target.takeDamage(attack);
    }

    void onDeath()
    {
        // TODO
        Debug.Log("Entity object died.");
        Destroy(gameObject);
    }
}
